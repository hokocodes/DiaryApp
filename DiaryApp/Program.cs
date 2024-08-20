using System;
using Microsoft.Data.SqlClient;

namespace DiaryApp
{
    class Program
    {
        private static string connectionString = "Server=localhost;Database=DiaryDB;Trusted_Connection=True;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Diary Application");
                Console.WriteLine("1. Write a new entry");
                Console.WriteLine("2. View all entries");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        WriteEntry();
                        break;
                    case "2":
                        ViewEntries();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void WriteEntry()
        {
            Console.Write("Enter your diary entry: ");
            var content = Console.ReadLine();
            var entryDate = DateTime.Now;

            using (var connection = new SqlConnection(connectionString))
            {
                var query = "INSERT INTO Entries (Date, Entry) VALUES (@Date, @Entry)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Date", entryDate);
                    command.Parameters.AddWithValue("@Entry", content);

                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.WriteLine("Entry saved successfully.");
                }
            }
        }

        private static void ViewEntries()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var query = "SELECT Date, Entry FROM Entries ORDER BY Date DESC";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var date = reader.GetDateTime(0);
                            var content = reader.GetString(1);
                            Console.WriteLine($"{date}: {content}");
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                }
            }
        }
    }
}
