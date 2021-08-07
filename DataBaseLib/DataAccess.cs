using Microsoft.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Windows.ApplicationModel;
using Windows.Storage;

namespace DataBaseLib
{
    static public class DataAccess
    {
        const string DB_NAME = "sqliteSample.db";
        public async static void InitializeDatabase()
        {
            //await ApplicationData.Current.LocalFolder.CreateFileAsync("sqliteSample.db", CreationCollisionOption.OpenIfExists);
            if (await ApplicationData.Current.LocalFolder.TryGetItemAsync(DB_NAME) == null)
            {
                StorageFile databaseFile = await Package.Current.InstalledLocation.GetFileAsync(/*$"Assets/*/DB_NAME/*"*/);
                await databaseFile.CopyAsync(ApplicationData.Current.LocalFolder);
            }
           
        }

        public static void AddData(object[] input)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_NAME);
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand
                {
                    Connection = db,

                    // Use parameterized query to prevent SQL injection attacks
                    CommandText = "INSERT INTO MyTable VALUES @Entry"
                };
                insertCommand.Parameters.AddWithValue("@Entry", input);

                insertCommand.ExecuteReader();

                db.Close();
            }

        }

        public static List<object[]> GetData(string table, string whereReq = null,string sortBy = null, params string[] columns)
        {
            List<object[]> entries = new List<object[]>();
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
                if (!string.IsNullOrEmpty(whereReq))
                    whereReq = "WHERE " + whereReq;
                else whereReq = "";
                if (sortBy != null)
                    sortBy = "ORDER BY " + sortBy;
                else sortBy = "";
                SqliteCommand selectCommand = new SqliteCommand
                    ($"SELECT {columnsString} from {table} {whereReq} {sortBy}", db);
                SqliteDataReader query = selectCommand.ExecuteReader();
                while (query.Read())
                {
                    object[] arr = new object[query.FieldCount];
                    for (int i = 0; i < arr.Length; i++)
                    {
                        arr[i] = query.GetValue(i);
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
