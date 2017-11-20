using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Ambition.DatabaseMigrator
{
    class Program
    {
        static void Main(string[] args)
        {
            //TODO: Разные БД, разные логгеры, получение файлов миграций

            string connectionString = string.Empty;

            if (args.Length > 0)
            {
                connectionString = args[0];
            }
            else
            {
                connectionString = ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;

                //Узнать инфу из конекшнстринга
                var builder = new SqlConnectionStringBuilder(connectionString);
                var dataSource = builder.DataSource;
            }

            string migrationsFolder = "Default";

            if (args.Length > 1)
            {
                migrationsFolder = args[2];
            }

            #region Установка таблицы для миграций и процедуры миграции в БД

            //Выполнить установку процедуры в БД
            string createMigrationTableText = File.ReadAllText("Tools/CreateMigrationsTable.sql");
            string dropMigrationSpText = File.ReadAllText("Tools/DropMigrationProcedure.sql");
            string createMigrationSpText = File.ReadAllText("Tools/CreateMigrationProcedure.sql");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.Execute(createMigrationTableText);
                connection.Execute(dropMigrationSpText);
                connection.Execute(createMigrationSpText);
                connection.Close();
            }

            #endregion

            #region Получение миграций из БД

            List<Migration> migrationsList = new List<Migration>();
            //Get migrations from DB
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                migrationsList = connection.Query<Migration>("SELECT * FROM Migrations ORDER BY FileName").ToList();
                connection.Close();
            }

            var migrationsDictionary = migrationsList.ToDictionary(x => x.FileName, c => c.Hash, StringComparer.OrdinalIgnoreCase);

            #endregion

            #region Получение файлов со скриптами миграций

            Debug.WriteLine($"Get migration files from folder {migrationsFolder}");
            var files = Directory.GetFiles($@"../../../Migrations/{migrationsFolder}");

            #endregion

            #region Обработка скриптов миграций

            foreach (var filePath in files)
            {
                FileInfo file = new FileInfo(filePath);
                Debug.WriteLine($"INFO: Check {file.Name}");

                string hash = GetSHA1Hash(filePath);

                //Если изменили содержимое уже установленного файла, то нужно поднять тревогу.
                if (migrationsDictionary.ContainsKey(file.Name) && migrationsDictionary[file.Name] != hash)
                {
                    Debug.WriteLine($"ERROR: {file.Name} was changed!");
                    throw new Exception($"{file.Name} was changed!");
                }

                if (!migrationsDictionary.ContainsKey(file.Name))
                {
                    //Проверить, что файла с таким названием не было.
                    //Если был, то проверить, что хэши разные
                    Migration migration = new Migration()
                    {
                        FileName = file.Name,
                        Hash = hash,
                        ExecutionDate = DateTime.UtcNow
                    };

                    string scriptText = File.ReadAllText(filePath);

                    Debug.WriteLine($"INFO: Execute script {file.Name}");

                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        var parameters = new DynamicParameters();
                        parameters.Add("@fileName", file.Name);
                        parameters.Add("@hash", hash);
                        parameters.Add("@scriptText", scriptText);

                        connection.Execute("ExecuteMigrationScript", parameters, commandType: CommandType.StoredProcedure);


                        //using (var scope = new TransactionScope())
                        //{
                        //    log.Info($"Execute script {file.Name}");
                        //    connection.Execute(scriptText);

                        //    connection.Insert<Migration>(migration);
                        //    log.Info($"SMTH: {smth}");

                        //    scope.Complete();
                        //}
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// Получить хэш файла
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetSHA1Hash(string filePath)
        {
            using (FileStream stream = File.OpenRead(filePath))
            {
                SHA256Managed sha = new SHA256Managed();
                byte[] hash = sha.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
    }

    public class Migration
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Hash { get; set; }
        public DateTime ExecutionDate { get; set; }
        public int Duration { get; set; }
    }
}
