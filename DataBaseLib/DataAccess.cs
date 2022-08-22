using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;

namespace DataBaseLib
{
    public static class DataAccess
    {
        public const string DB_NAME = "DataBase.db";
        private const int DB_VERSION = 11;
        public static event Action NewDataLoaded;
        private static bool _canOpenConnection = true;
        public static bool IsActualDb { get
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                return !(localSettings.Values["DataBaseVercion"] == null || (int)localSettings.Values["DataBaseVercion"] != DB_VERSION);

            } }
        public static string DbPath => Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_NAME);
        static DataAccess(){InitializeDatabase(); }
        public static async Task InitializeDatabase()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (!IsActualDb)
            {
                _canOpenConnection = false;
                await DeleteDb();
            }
            if (await ApplicationData.Current.LocalFolder.TryGetItemAsync(DB_NAME) == null)
            {
                await LoadNewDb(localSettings);
                _canOpenConnection = true;
            }
        }

        private static async Task DeleteDb()
        {
                var storage = await ApplicationData.Current.LocalFolder.TryGetItemAsync(DB_NAME);
                if (storage != null)
                    await storage.DeleteAsync();
        }

        private static async Task LoadNewDb(ApplicationDataContainer localSettings)
        {
                StorageFile databaseFile = await Package.Current.InstalledLocation.GetFileAsync(DB_NAME);
                File.Copy(databaseFile.Path, ApplicationData.Current.LocalFolder.Path + $"\\{DB_NAME}");
                localSettings.Values["DataBaseVercion"] = DB_VERSION;
                NewDataLoaded?.Invoke();
        }

        public static async Task UpdateDb(string text)
        {
            //await text.RenameAsync(DB_NAME);
            //await text.CopyAsync(
            //    await StorageFolder.GetFolderFromPathAsync(ApplicationData.Current.LocalFolder.Path + $"\\{DB_NAME}"));
            //ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            //localSettings.Values["DataBaseVercion"] = DB_VERSION;
            StorageFile databaseFile = await Package.Current.InstalledLocation.GetFileAsync(DB_NAME);
            await FileIO.WriteTextAsync(databaseFile, text);
            NewDataLoaded?.Invoke();
        }
        public static void AddData(string table, string[] rows, object[] input)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_NAME);
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                string rowEx = "";
                string param = "";
                if (rows != null)
                {
                    foreach (var i in rows)
                    {
                        rowEx += i + ",";
                    }
                    rowEx = "(" + rowEx.Trim(',') + ")";
                }

                foreach (var i in input)
                {
                    param += i.ToString() + ',';
                }
                param = "(" + param.Trim(',') + ")";
                SqliteCommand insertCommand = new SqliteCommand
                {
                    Connection = db,

                    // Use parameterized query to prevent SQL injection attacks
                    CommandText = $"INSERT INTO {table} {rowEx} VALUES {param}",
                    CommandType = System.Data.CommandType.Text
                };
                insertCommand.ExecuteReader();

                db.Close();
            }
        }
        public static List<object[]> GetData(string table, string whereReq = null, string sortBy = null, params string[] columns)
        {
            List<object[]> entries = new List<object[]>();
            //for (int i = 0; i < entries.Count; i++)
            //    entries[i] = new string[columns.Length];
            if (!_canOpenConnection)
                return entries;

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_NAME);

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

        public static List<object[]> GetData(string request)
        {
            List<object[]> entries = new List<object[]>();
            //for (int i = 0; i < entries.Count; i++)
            //    entries[i] = new string[columns.Length];
            if (!_canOpenConnection)
                return entries;
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_NAME);

            using (SqliteConnection db =
                new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand selectCommand = new SqliteCommand
                    (request, db);
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
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_NAME);
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

        public static async Task<SqliteDataReader> RawRequestAsync(string request)
        {
            return await Task.Run(() => RawRequest(request));
        }
    }
}