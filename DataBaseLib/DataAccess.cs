using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace DataBaseLib
{
    public class DataAccess:IDataAccess
    {
        private static DataAccess _instance;
        public static DataAccess Instance => _instance ??= new DataAccess();
        public const string DB_NAME = "DataBase.db";
        private const int DB_VERSION = 12;
        public static event Action NewDataLoaded;
        private static bool _canOpenConnection = true;
        public static bool IsActualDb
        {
            get
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                return !(localSettings.Values["DataBaseVercion"] == null || (int)localSettings.Values["DataBaseVercion"] != DB_VERSION);

            }
        }
        public static string DbPath => Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_NAME);
        private DataAccess() { InitializeDatabase(); }
        public async Task InitializeDatabase()
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

        public List<object[]> GetData(string table, string whereReq = null, string sortBy = null, params string[] columns)
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
                var command = CreateCommand(table, whereReq, sortBy, columns);
                SqliteCommand selectCommand = new SqliteCommand
                    (command, db);
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

        public static string CreateCommand(string table, string whereReq, string sortBy, string[] columns)
        {
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
            var command = $"SELECT {columnsString} from {table} {whereReq} {sortBy}";
            return command;
        }

        public List<object[]> GetData(string request)
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

        public void RawRequest(string request)
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
                query.Close();
            }
        }

        public int GetLastId() => Convert.ToInt32(DataAccess.Instance.GetData("SELECT last_insert_rowid()")[0][0];

        public async Task RawRequestAsync(string request)
        {
            await Task.Run(() => RawRequest(request));
        }
    }
}