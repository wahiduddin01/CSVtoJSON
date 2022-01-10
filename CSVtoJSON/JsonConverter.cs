using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CSVtoJSON
{
    public class JsonConverter
    {
        public List<string[]> csvEntries;
        public string[] properties;
        public Dictionary<string, List<string>> groups;
        public List<Dictionary<string, Object>> resultList;
        public string[] csvLines;

        public JsonConverter()
        {
        }

        //This method converts the CSV file to a JSON string and saves it as a new file
        public void CSVToJson(string filePath)
        {
            csvLines = System.IO.File.ReadAllLines(filePath);
            csvEntries = new List<string[]>();

            foreach (string line in csvLines)
            {
                csvEntries.Add(line.Split(','));
            }

            properties = csvLines[0].Split(',');
            GetGroups(properties);
            AddData();

            string jsonText = JsonConvert.SerializeObject(resultList, Formatting.Indented);
            SaveFile(filePath, jsonText);
        }

        //This method collects all the grouped headers and sorts them into their respective groups
        public void GetGroups(string[] properties)
        {
            List<string> groupedProperties = new List<string>();

            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].Contains("_"))
                {
                    groupedProperties.Add(properties[i]);
                }
            }

            groups = new Dictionary<string, List<string>>();

            for (int i = 0; i < groupedProperties.Count(); i++)
            {
                string prefix = groupedProperties[i].Substring(0, groupedProperties[i].IndexOf("_"));

                if (!groups.ContainsKey(prefix))
                {
                    List<string> suffix = new List<string>();
                    for (int j = 0; j < groupedProperties.Count(); j++)
                    {
                        if ((groupedProperties[j].Substring(0, groupedProperties[j].IndexOf("_")).Equals(prefix)))
                        {
                            suffix.Add(groupedProperties[j].Substring(groupedProperties[j].IndexOf('_') + 1));
                        }
                    }
                    groups.Add(prefix, suffix);
                }

            }
        }

        //This method adds the CVS file data to each of the headers for each entry
        public void AddData()
        {
            resultList = new List<Dictionary<string, Object>>();

            for (int i = 1; i < csvLines.Length; i++)
            {
                Dictionary<string, Object> data = new Dictionary<string, object>();
                for (int j = 0; j < properties.Length; j++)
                {
                    if (!properties[j].Contains("_"))
                    {
                        data.Add(properties[j], csvEntries[i][j]);
                    }
                    else
                    {
                        string prefix = properties[j].Substring(0, properties[j].IndexOf("_"));
                        string suffix = properties[j].Substring(properties[j].IndexOf('_') + 1);
                        List<string> prefixGroups = groups[prefix];
                        for (int k = 0; k < prefixGroups.Count(); k++)
                        {
                            if (prefixGroups[k].Equals(suffix))
                            {
                                Dictionary<string, string> groupedData = new Dictionary<string, string>();
                                groupedData.Add(suffix, csvEntries[i][j]);
                                if (!data.ContainsKey(prefix))
                                {
                                    data.Add(prefix, groupedData);
                                }
                                else
                                {
                                    Dictionary<string, string> currentValue = (Dictionary<string, string>)data[prefix];
                                    currentValue.Add(suffix, csvEntries[i][j]);
                                    data[prefix] = currentValue;
                                }
                            }
                        }

                    }
                }
                resultList.Add(data);
            }
        }

        //This method writes the JSON string to a new file and renames it with a JSON extention before saving it.
        public void SaveFile(string filePath, string jsonText)
        {
            string removeExtention = filePath.Substring(0, filePath.Length - 3);
            string newFilePath = removeExtention + "json";
            System.IO.File.WriteAllText(newFilePath, jsonText);
        }

    }
}
