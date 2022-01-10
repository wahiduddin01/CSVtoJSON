using System;
namespace CSVtoJSON
{
    public class UI
    {
        public UI()
        {

        }

        //This method handles the user interaction in the terminal
        //I would have used OpenFileDialog class to allow the user to browse files from terminal but it is not availible for mac
        public void Run()
        {
            Console.WriteLine("Enter the file path of the CSV file you wish to convert to JSON");
            string userInput = Console.ReadLine();
            string filePath = userInput.Trim();
            string extention = filePath.Substring(filePath.Length - 3);
            extention.ToLower();
            if (extention.Equals("csv"))
            {
                JsonConverter jsonConverter = new JsonConverter();
                jsonConverter.CSVToJson(filePath);
                Console.WriteLine("Conversion Successful");
            }
            else
            {
                Console.WriteLine("Invalid file type");
                Run();
            }
        }
    }
}
