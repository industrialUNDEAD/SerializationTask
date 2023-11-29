using SerializationTaskConsole.Model;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace SerializationTaskConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            //1) Create collection of randomly generated objects in memory by provided models, number of ofjects 10000;
            const int OBJECTS_COUNT = 10000;
            const string FILE_NAME = "Persons.json";

            var randomPersons = ModelGenerator.GetRandomPersons(OBJECTS_COUNT);

            //2) Serialyze it to JSON format;
            //3) Write the serialization result to the current user desktop directory, the text file name should be "Persons.json";
            var jsonSaver = new SerializableSaver<Person[]>();

            jsonSaver.Save(randomPersons, $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\{FILE_NAME}");

            //4) Clear the in memory collection;
            Array.Clear(randomPersons);

            //5) Read objects from file;
            var persons = jsonSaver.Load($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\{FILE_NAME}");

            //6) Display in console persons count, persons credit card count, the average value of child age.
            var averageChildAge = DateTime.Now.Year - DateUnixConverter.UnixTimeStampToDateTime(
                ((long)persons.Average
                (p => p.Children.Average(c => c.BirthDate)))).Year;

            Console.WriteLine($"Persons count: {persons.Count()}\n" +
                $"Persons credit card count: {persons.Sum(c => c.CreditCardNumbers.Length)}\n" +
                $"Average value of child age: {averageChildAge}");

        }
    }
}