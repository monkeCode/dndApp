using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBaseLib
{
    internal class OnlineDataAccess
    {

        public static MySqlConnection GetDBConnection()
        {
            string connect = "Server=87.249.38.253;Database=cp32600_dnd;Uid=cp32600_dnd;Pwd=HyR8rUDw;";
            MySqlConnection connection = new MySqlConnection(connect);
            connection.Open();
            return connection;
        }

        public static List<object[]> GetData(string table, string whereReq = null, string sortBy = null,
            params string[] columns)
        {
            List<object[]> entries = new List<object[]>();
            var connection = GetDBConnection();
            var command = connection.CreateCommand();
            command.CommandText = DataAccess.CreateCommand(table, whereReq, sortBy, columns);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var entry = new object[reader.FieldCount];
                reader.GetValues(entry);
                entries.Add(entry);
            }
            reader.Close();
            connection.Close();
            return entries;

        }


        public static List<object[]> GetData(string request)
        {
            List<object[]> entries = new List<object[]>();
            var connection = GetDBConnection();
            var command = connection.CreateCommand();
            command.CommandText = request;
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var entry = new object[reader.FieldCount];
                reader.GetValues(entry);
                entries.Add(entry);
            }
            reader.Close();
            connection.Close();
            return entries;
        }

        public static MySqlDataReader RawRequest(string request)
        {
            List<object[]> entries = new List<object[]>();
            var connection = GetDBConnection();
            var command = connection.CreateCommand();
            command.CommandText = request;
            var reader = command.ExecuteReader();
            connection.Close();
            return reader;
        }

        public static async Task<MySqlDataReader> RawRequestAsync(string request)
        {
            return await Task.Run(() => RawRequest(request));
        }
    }
}
