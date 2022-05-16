using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data;
using System.Collections.Generic;

namespace BookStoreLIB
{
    public class DALUserInfo
    {
        public int LogIn(string userName, string password)
        {
            var conn = new SqlConnection(BookStoreDATA.Properties.Settings.Default.KPConnections);

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select UserID from UserData where "
                    + " UserName  = @UserName and Password = @Password ";
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Password", password);
                conn.Open();
                int userId = (int)cmd.ExecuteScalar();
                if (userId > 0) return userId;
                else return -1;
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return -1;
            }

            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }

        }

        public int Register(string userName, string password, string firstName, string lastName, string email)
        {
            var conn = new SqlConnection(BookStoreDATA.Properties.Settings.Default.KPConnections);

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO UserData (UserName, Password, FirstName, LastName, Email)"
                    + " VALUES (@UserName, @Password, @FirstName, @LastName, @Email)";
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Email", email);
                conn.Open();
                int userId = (int)cmd.ExecuteScalar();
                if (userId > 0) return userId;
                else return -1;
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return -1;
            }

            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }

        }
        public Dictionary<string, string> AccountInfo(int userId)
        {
            Dictionary<string, string> account = new Dictionary<string, string>();
            account.Add("fname", "");
            account.Add("lname", "");
            account.Add("email", "");
            account.Add("phone", "");
            account.Add("bdate", "");

            var conn = new SqlConnection(BookStoreDATA.Properties.Settings.Default.KPConnections);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Select FirstName, LastName, Email, Phone, BirthDate from UserData " +
                "where UserID = @userId";
            cmd.Parameters.AddWithValue("@userId", userId);
            conn.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        account["fname"] = (reader["FirstName"] != DBNull.Value) ? (string)reader["FirstName"] : "";
                        account["lname"] = (reader["LastName"] != DBNull.Value) ? (string)reader["LastName"] : "";
                        account["email"] = (reader["Email"] != DBNull.Value) ? (string)reader["Email"] : "";
                        account["phone"] = (reader["Phone"] != DBNull.Value) ? (string)reader["Phone"] : "";
                        account["bdate"] = (reader["BirthDate"] != DBNull.Value) ? Convert.ToDateTime(reader["BirthDate"]).ToShortDateString() : "";
                    }
                }
            }
            conn.Close();

            return account;
        }
    }
}