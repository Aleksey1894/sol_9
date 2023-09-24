using System.Collections.Generic;
using System;
using System.Text;
using System.Transactions;

namespace Lesson_9_Text_And_Files
{
    internal class Program
    {
        static string database = "db.txt";
        static (string name, string phone, DateTime birth)[] contacts;

        static void Main(string[] args)
        {

            string[] records = ReadDatabaseAllTextLines(database);
            contacts = ConvertStringsToContacts(records);

            while (true)
            {
                UserInteraction();
            }
        }

        static void UserInteraction()
        {
            Console.WriteLine("1. Write all contacts");
            Console.WriteLine("2. Add new contact");
            Console.WriteLine("3. Edit contact");
            Console.WriteLine("4. Search by name");
            Console.WriteLine("6. Save");

            int input = int.Parse(Console.ReadLine());
            switch (input)
            {
                case 1:
                    WriteAllContactsToConsole();
                    break;
                case 2:
                    AddNewContact();
                    break;
                case 3:
                    EditContact();
                    break;
                case 4:
                    SearchContatct();
                    break;
                case 6:
                    SaveContactsToFile();
                    break;
                default:
                    Console.WriteLine("No such operation.");
                    break;
            }
        }

        static void AddNewContact()
        {
            static void ResizeArray<T>(ref T[] contacts, int newSize)
            {
                T[] newArray = new T[newSize];
                for (int i = 0; i < contacts.Length; i++)
                {
                    newArray[i] = contacts[i];
                }
            }
            Console.WriteLine("Enter new name ");
            string name = Console.ReadLine();
            Console.WriteLine("Enter new phone ");
            string phone = Console.ReadLine();
            Console.WriteLine("Enter your birthday ");            
            DateTime date = DateTime.Parse(Console.ReadLine());


            ResizeArray(ref contacts, contacts.Length + 1);

            contacts[^1] = (name, phone, date);

        }

        static void EditContact()
        {
            int id = SearchContatct();
            if (id == -1)
            {
                Console.WriteLine("Sorry, nothing is found.");
                return;
            }

            Console.Write("Enter name: ");
            string name = Console.ReadLine();
            Console.Write("Enter phone: ");
            string phone = Console.ReadLine();
            Console.Write("Enter date of birth: ");
            var date = DateTime.Parse(Console.ReadLine());           
            
            contacts[id] = (name, phone, date);
        }

        static int SearchContatct()
        {
            Console.Write("Enter search query: ");
            string searchQuery = Console.ReadLine().Trim();
                  
            for (int i = 0; i < contacts.Length; ++i)
            {
            if (contacts[i].name.IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase)>=0 ||
                contacts[i].phone.IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    Console.WriteLine($" Contact found{i + 1}: {contacts[i].name}, {contacts[i].phone}, {contacts[i].birth}");
                    return i;
                }
            }
            return -1;
        }

        static void WriteAllContactsToConsole()
        {
            for (int i = 0; i < contacts.Length; i++)
            {
                int age = DateTime.Now.Year - contacts[i].birth.Year;
                Console.WriteLine($"#{i + 1}: Name: {contacts[i].Item1}, Phone: {contacts[i].Item2}, Age: {age}");
            }
        }

        static (string name, string phone, DateTime date)[] ConvertStringsToContacts(string[] records)
        {
            var contacts = new (string name, string phone, DateTime date)[records.Length];
            for (int i = 0; i < records.Length; ++i)
            {
                string[] array = records[i].Split(','); // "Oleksii", "+38090873928", "30.03.1993"
                if (array.Length != 3)
                {
                    Console.WriteLine($"Line #{i + 1}: '{records[i]}' cannot be parsed");
                    continue;
                }
                contacts[i].name = array[0];
                contacts[i].phone = array[1];
                contacts[i].date = DateTime.Parse(array[2]);
            }
            return contacts;
        }

        static void SaveContactsToFile()
        {
            string[] lines = new string[contacts.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = $"{contacts[i].Item1},{contacts[i].Item2},{contacts[i].Item3}";
            }
            File.WriteAllLines(database, lines);
        }

        static string[] ReadDatabaseAllTextLines(string file)
        {
            if (!File.Exists(file))
            {
                File.WriteAllText(file, "");
            }
            return File.ReadAllLines(file);
        }
    }
}
