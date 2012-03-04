using System;
using System.Collections.Generic;
using System.Text;

using MySql.Data.MySqlClient;
using System.Collections;

namespace Nimboid
{
    public class DBAccess
    {

        MySqlConnectionStringBuilder myCSB;
        

        public uint Port  { set { myCSB.Port = value; } }
        public string Host { set { myCSB.Server = value; } }
        public string User { set { myCSB.UserID = value; } }
        public string Password { set { myCSB.Password = value; } }

        public DBAccess()
        {
                myCSB = new MySqlConnectionStringBuilder();
                
                myCSB.Database = "nexus";
                myCSB.ConnectionTimeout = 30;
                Port = 3306;
                Host = "localhost";
                User = "root";
                Password = "missymoo";
                
        }

        private string HashString(string Value)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(Value);
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2").ToLower();
            return ret;
        }

        
        public bool CheckPassword(string name, string pass)
        {
            bool returnval = false;
            using (MySqlConnection myConnection = new MySqlConnection(myCSB.ConnectionString))
            {
                myConnection.Open();
                MySqlCommand command = myConnection.CreateCommand();
                command.CommandText = "SELECT Password FROM Users WHERE Username = '" + name + "'";

                // Call the Close method when you are finished using the OracleDataReader 
                // to use the associated OracleConnection for any other purpose.
                // Or put the reader in the using block to call Close implicitly.
                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        // printing the table content
                        string passfromdb = reader.GetString(0);
                        returnval = (passfromdb == HashString(pass));
                    }
                }

                myConnection.Close();
            }
            return returnval;



        }

        public int GetUserID(string name)
        {
            int returnval = -1;
            using (MySqlConnection myConnection = new MySqlConnection(myCSB.ConnectionString))
            {
                myConnection.Open();
                MySqlCommand command = myConnection.CreateCommand();
                command.CommandText = "SELECT ID FROM Users WHERE Username = '" + name + "'";

                // Call the Close method when you are finished using the OracleDataReader 
                // to use the associated OracleConnection for any other purpose.
                // Or put the reader in the using block to call Close implicitly.
                using (MySqlDataReader reader = command.ExecuteReader())
                {
               
                    while (reader.Read())
                    {
                        // printing the table content
                        returnval = reader.GetInt32(0);
                        for (int i = 0; i < reader.FieldCount; i++)
                            Console.Write(reader.GetValue(i).ToString() + "\t");
                        Console.Write(Environment.NewLine);
                    }
                }

                myConnection.Close();
            }
            return returnval;
        }

        public string GetUsername(int ID)
        {
            string returnval = null;
            using (MySqlConnection myConnection = new MySqlConnection(myCSB.ConnectionString))
            {
                myConnection.Open();
                MySqlCommand command = myConnection.CreateCommand();
                command.CommandText = "SELECT Username FROM Users WHERE ID = '" + ID.ToString() + "'";

                // Call the Close method when you are finished using the OracleDataReader 
                // to use the associated OracleConnection for any other purpose.
                // Or put the reader in the using block to call Close implicitly.
                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        // printing the table content
                        returnval = reader.GetString(0);
                        for (int i = 0; i < reader.FieldCount; i++)
                            Console.Write(reader.GetValue(i).ToString() + "\t");
                        Console.Write(Environment.NewLine);
                    }
                }

                myConnection.Close();
            }
            return returnval;
        }

        public ArrayList GetFriends(int ID)
        {
            ArrayList returnval = new ArrayList();
            using (MySqlConnection myConnection = new MySqlConnection(myCSB.ConnectionString))
            {
                myConnection.Open();
                MySqlCommand command = myConnection.CreateCommand();
                command.CommandText = "SELECT FriendID FROM FriendsList WHERE UserID = " + ID + " AND Confirmed = 1";

                // Call the Close method when you are finished using the OracleDataReader 
                // to use the associated OracleConnection for any other purpose.
                // Or put the reader in the using block to call Close implicitly.
                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        returnval.Add(reader.GetInt32(0));
                        Console.WriteLine("Got Friend");
                    }
                }

                myConnection.Close();
            }
            Console.WriteLine("Found: {0}", returnval.Count);
            return returnval;

        }

        public ArrayList GetFriendsUsers(int ID)
        {
            ArrayList friendIDs = GetFriends(ID);

            ArrayList returnval = new ArrayList();
            using (MySqlConnection myConnection = new MySqlConnection(myCSB.ConnectionString))
            {
                myConnection.Open();
                MySqlCommand command = myConnection.CreateCommand();
                command.CommandText = "SELECT FriendID FROM FriendsList WHERE UserID = " + ID + " AND Confirmed = 1";

                // Call the Close method when you are finished using the OracleDataReader 
                // to use the associated OracleConnection for any other purpose.
                // Or put the reader in the using block to call Close implicitly.
                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        returnval.Add(reader.GetInt32(0));
                    }
                }

                myConnection.Close();
            }
            return returnval;


        }



    }
}
