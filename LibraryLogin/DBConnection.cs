using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace LibraryLogin
{
    public class DBConnection
    {
        public string ConnectionString { get; set; }
        public DBConnection(string server, string db, string user, string pass)
        {
            this.ConnectionString = $"Server = '{server}'; database = '{db}'; User Id = {user}; password = '{pass}'";
        }
        public string CreateQuery(string query)
        {
            string answer = "";
            using (MySqlConnection myConnection =  new MySqlConnection(ConnectionString))
            {
                myConnection.Open();
                MySqlCommand command = new MySqlCommand(query, myConnection);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        answer = reader.GetString(0);
                    }
                }
            }
            return answer;
                
        }
    }
}