using System;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

using change_management.Models;

namespace change_management.Controllers
{
    public class TeamDatabaseService : IDatabaseService<Team>
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public TeamDatabaseService(IConfiguration config){
            _configuration = config;
            _connectionString = config.GetConnectionString("db");
        } 

        private SqlConnection DatabaseConnector() {
            SqlConnection connection = new SqlConnection(_connectionString);
            return connection;
        }

        public IEnumerable<Team> SelectAll(){
            var teams = new List<Team>();
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();       
                    String sql = ("SELECT * FROM teams");

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                teams.Add(new Team(reader.GetInt32(0), reader.GetString(1)));
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

        public string SelectTeam(int id){
            string team = "";
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();       
                    String sql = ("SELECT name FROM teams WHERE teamId = " + id);

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                team = (reader.GetString(0));
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

        public IEnumerable<User> SelectAllMembers(int teamId){
            var users = new List<User>();
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();       
                    String sql = ("SELECT users.userId, users.forename, users.surname, users.role " +
                                    "FROM teams " + 
                                    "JOIN teamMembers ON teamMembers.teamId = teams.teamId " + 
                                    "JOIN users ON users.userId = teamMembers.userId " +
                                    "WHERE teams.teamId = " + teamId);
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

        public IEnumerable<Team> SelectAllMembers(){
            var teams = SelectAll();   
            foreach (var team in teams)
            {
                team.teamMembers = SelectAllMembers(team.teamID);
            }
            return teams;
        }

        public void Insert(Team t){
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();       
                    string sql2 = "INSERT INTO teams(name)   VALUES(@param2)";
                            
                    using(SqlCommand cmd = new SqlCommand(sql2,connection)) 
                    {
                        cmd.Parameters.Add("@param2", SqlDbType.NVarChar, 50).Value = t.name;
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

        public void InsertUser(int t, int u){
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();       
                    string sql2 = "INSERT INTO teamMembers(userId, teamId)   VALUES(@param1, @param2)";
                            
                    using(SqlCommand cmd = new SqlCommand(sql2,connection)) 
                    {
                        cmd.Parameters.Add("@param1", SqlDbType.Int).Value = u;
                        cmd.Parameters.Add("@param2", SqlDbType.Int).Value = t;
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