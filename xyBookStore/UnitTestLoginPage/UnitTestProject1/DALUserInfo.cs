using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Data;

namespace UnitTestProject1
{
    internal class DALUserInfo
    {
        public int LogIN(string userName, string password)
        {
            var conn = new SqlConnection(Properties.Settings.Default.xyConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select UserID from UserData where "
                    + " UserName = @UserName and Password = @Password ";
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
    }
}