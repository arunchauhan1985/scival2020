using System;
using System.IO;
using MySql.Data.MySqlClient;

namespace DatabaseGenerator
{
    class DatabaseGenerator
    {
        string server = "localhost";
        string database = "scival";// "scivaltest1";
        string userId = "root";
        string password = "pass";
        string seprator = "=========================================";

        string scriptsFolderPath = @"D:\Work\Scival\MySqlDal\Scripts\StoredProcedures\Test\";
        string errorFolderPath = @"D:\ScivalErrorScripts\";

        MySqlConnection connection = new MySqlConnection();
        MySqlCommand command;

        public void StartGeneration()
        {
            try
            {
                Console.WriteLine(seprator);
                Console.WriteLine("Database Generation - Start");
                Console.WriteLine(seprator);

                Directory.CreateDirectory(errorFolderPath);

                OpenConnection();

                ExecuteCommands();

                CloseConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(seprator);
                Console.WriteLine(seprator);
                Console.WriteLine(ex.Message);
                Console.WriteLine(seprator);
                Console.WriteLine(seprator);
                Console.ReadLine();
            }
            finally
            {
                connection.Close();
                Console.ReadLine();
            }
        }

        public void OpenConnection()
        {
            Console.WriteLine("Making Database Connection");
            Console.WriteLine(seprator);

            string connectionString = string.Format("server={0};user id={1};password={2};persistsecurityinfo=True;database={3};", server, userId, password, database);

            connection.ConnectionString = connectionString;
            connection.Open();
        }

        public void CloseConnection()
        {
            Console.WriteLine("Closing Database Connection");
            Console.WriteLine(seprator);

            connection.Close();
        }

        private void ExecuteCommands()
        {
            Console.WriteLine("Loading all database script files");
            Console.WriteLine(seprator);

            string[] filePaths = Directory.GetFiles(scriptsFolderPath, "*.sql", SearchOption.AllDirectories);

            foreach (string file in filePaths)
            {
                var fileName = Path.GetFileName(file);

                try
                {
                    Console.WriteLine(fileName);

                    string content = File.ReadAllText(file);

                    command = new MySqlCommand();
                    command.Connection = connection;
                    command.CommandText = content;
                    command.ExecuteNonQuery();

                    Console.WriteLine(fileName + " - Done");
                    Console.WriteLine(seprator);
                }
                catch
                {
                    File.Copy(file, errorFolderPath + fileName);
                }
            }
        }
    }
}
