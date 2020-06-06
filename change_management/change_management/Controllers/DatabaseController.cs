using System;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace change_management.Controllers
{
 public class DatabaseController : Controller
    {
        private SqlConnection DatabaseConnector() {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = ""; 
            builder.UserID = "";            
            builder.Password = "";     
            builder.InitialCatalog = "";
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            return connection;
        }

        public void databaseSelect(string table){
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();       
                    String sql = ("SELECT * FROM " + table);

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("{0} {1} {2} {3}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void databaseUserInsert(string forename, string surname, string role){
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();       
                    string sql2 = "INSERT INTO users(forename, surname, role)   VALUES(@param2,@param3, @param4)";
                            
                    using(SqlCommand cmd = new SqlCommand(sql2,connection)) 
                    {
                        cmd.Parameters.Add("@param2", SqlDbType.NVarChar, 50).Value = forename;
                        cmd.Parameters.Add("@param3", SqlDbType.NVarChar, 50).Value = surname;
                        cmd.Parameters.Add("@param4", SqlDbType.NVarChar, 50).Value = role;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery(); 
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}