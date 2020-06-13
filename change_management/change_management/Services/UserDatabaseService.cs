using System;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

using change_management.Models;

namespace change_management.Controllers
{
    public class UserDatabaseService : IDatabaseService<User>
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public UserDatabaseService(IConfiguration config){
            _configuration = config;
            _connectionString = config.GetConnectionString("db");
        } 

        private SqlConnection DatabaseConnector() {
            SqlConnection connection = new SqlConnection(_connectionString);
            return connection;
        }

        public IEnumerable<User> SelectAll(){
            var users = new List<User>();
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();       
                    String sql = ("SELECT * FROM users");
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
                            }
                        }
                    }
                }
                return users;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return users;
            }
        }

        public User Select(int userID){
            User user = new User();
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();       
                    String sql = ("SELECT * FROM users WHERE userId = " + userID);
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                user = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
                            }
                        }
                    }
                }
                return user;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return user;
            }
        }

        public Team SelectUserTeam(int userID){
            var users = new List<User>();
            var team = new Team();
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();       
                    String sql = ("SELECT users.userId, users.forename, users.surname, users.role, " +
                                        "teams.teamId, teams.name " +
                                    "FROM USERS  " +
                                    "JOIN teamMembers ON teamMembers.userId = users.userId " +
                                    "JOIN teams ON teams.teamId = teamMembers.teamId  " +
                                    "WHERE teamMembers.teamId IN ( " +
                                        "SELECT teamMembers.teamId " +
                                        "FROM users  " +
                                        "JOIN teamMembers ON teamMembers.userId = users.userId " +
                                        "WHERE users.userId =" + userID + ")");

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                 users.Add(new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
                            }
                        }
                    }

                    String sql2 = ("SELECT teamMembers.teamId, teams.name " +
                                    "FROM users " +
                                    "JOIN teamMembers ON teamMembers.userId = users.userId " +
                                    "JOIN teams ON teams.teamId = teamMembers.teamId " +
                                    "WHERE users.userId =" + userID );

                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                team = new Team(reader.GetInt32(0), reader.GetString(1), users);
                            }
                        }
                    }
                }
                return team;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return team;
            }
        }

        public void Insert(User u){
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();       
                    string sql2 = "INSERT INTO users(forename, surname, role)   VALUES(@param2,@param3, @param4)";
                            
                    using(SqlCommand cmd = new SqlCommand(sql2,connection)) 
                    {
                        cmd.Parameters.Add("@param2", SqlDbType.NVarChar, 50).Value = u.forename;
                        cmd.Parameters.Add("@param3", SqlDbType.NVarChar, 50).Value = u.surname;
                        cmd.Parameters.Add("@param4", SqlDbType.NVarChar, 50).Value = u.role;
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