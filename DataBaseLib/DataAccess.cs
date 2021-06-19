using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;

namespace DataBaseLib
{
    static public class DataAccess
    {

        public async static void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("sqliteSample.db", CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                string tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS MyTable (Primary_Key INTEGER PRIMARY KEY, " +
                    "Text_Entry NVARCHAR(2048) NULL)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public static void AddData(string inputText)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @Entry);";
                insertCommand.Parameters.AddWithValue("@Entry", inputText);

                insertCommand.ExecuteReader();

                db.Close();
            }

        }

        public static List<string[]> GetData(string table, string whereReq = null, params string[] columns)
        {
            List<string[]> entries = new List<string[]>();
            //for (int i = 0; i < entries.Count; i++)
            //    entries[i] = new string[columns.Length];

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                string columnsString = "";
                foreach (var str in columns)
                {
                    columnsString += str + ", ";
                }
                columnsString = columnsString.Trim();
                columnsString = columnsString.Remove(columnsString.Length - 1);
                if (whereReq != null)
                    whereReq = "WHERE " + whereReq;
                SqliteCommand selectCommand = new SqliteCommand
                    ($"SELECT {columnsString} from {table} {whereReq}", db);

                SqliteDataReader query = selectCommand.ExecuteReader();
                while (query.Read())
                {
                    string[] arr = new string[query.FieldCount];
                    for (int i = 0; i < arr.Length; i++)
                    {
                        arr[i] = query.GetString(i);
                    }
                    entries.Add(arr);
                }

                db.Close();
            }

            return entries;
        }

        public static SqliteDataReader RawRequest(string request)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = request;

                SqliteDataReader query = insertCommand.ExecuteReader();
                db.Close();
                return query;
            }
        }
    }
}
