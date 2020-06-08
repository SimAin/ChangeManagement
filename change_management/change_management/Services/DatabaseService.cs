using System;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

using change_management.Models;

namespace change_management.Controllers
{
    public class DatabaseService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DatabaseService(IConfiguration config){
            _configuration = config;
            _connectionString = config.GetConnectionString("db");
        } 

        private SqlConnection DatabaseConnector() {
            SqlConnection connection = new SqlConnection(_connectionString);
            return connection;
        }

        public List<User> databaseSelect(string table){
            var users = new List<User>();
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
                                users.Add(new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
                                Console.WriteLine("{0} {1} {2} {3}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
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

        public List<Team> databaseSelectTeams(string table){
            var teams = new List<Team>();
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
                                teams.Add(new Team(reader.GetInt32(0), reader.GetString(1)));
                                Console.WriteLine("{0} {1}", reader.GetInt32(0), reader.GetString(1));
                            }
                        }
                    }
                }
                return teams;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return teams;
            }
        }

        public List<SystemEntity> databaseSelectSystems(string table){
            var systems = new List<SystemEntity>();
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();       
                    String sql = ("SELECT systemId, systems.name, code, description, techStack, " + 
                                    "users.userId, users.forename, users.surname, users.role, " + 
                                    "teams.teamId, teams.name FROM systems " +
                                    "JOIN users ON users.userId = systems.pointOfContact " + 
                                    "JOIN teams ON teams.teamId = systems.owningTeam");

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                systems.Add(new SystemEntity(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), 
                                            new User(reader.GetInt32(5), reader.GetString(6), reader.GetString(7), reader.GetString(8)), 
                                            new Team(reader.GetInt32(9), reader.GetString(10))));
                                
                                
                                
                                
                                //Console.WriteLine("{0} {1} {2} {3} {4} {5} {6}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetInt32(5), reader.GetInt32(6));
                            }
                        }
                    }
                }
                return systems;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return systems;
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

        public void databaseTeamInsert(string name){
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();       
                    string sql2 = "INSERT INTO teams(name)   VALUES(@param2)";
                            
                    using(SqlCommand cmd = new SqlCommand(sql2,connection)) 
                    {
                        cmd.Parameters.Add("@param2", SqlDbType.NVarChar, 50).Value = name;
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

        public void databaseSystemInsert(SystemEntity system){
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();       
                    string sql2 = "INSERT INTO systems(name, code, description, techStack, pointOfContact, owningTeam)   VALUES(@param1, @param2, @param3, @param4, @param5, @param6)";
                            
                    using(SqlCommand cmd = new SqlCommand(sql2,connection)) 
                    {
                        cmd.Parameters.Add("@param1", SqlDbType.NVarChar, 50).Value = system.name;
                        cmd.Parameters.Add("@param2", SqlDbType.NVarChar, 50).Value = system.code;
                        cmd.Parameters.Add("@param3", SqlDbType.NVarChar, 50).Value = system.description;
                        cmd.Parameters.Add("@param4", SqlDbType.NVarChar, 50).Value = system.techStack;
                        cmd.Parameters.Add("@param5", SqlDbType.Int).Value = system.pointOfContact;
                        cmd.Parameters.Add("@param6", SqlDbType.Int).Value = system.owningTeam;
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