using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using change_management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace change_management.Controllers {
    public class SystemDatabaseService : IDatabaseService<SystemEntity> {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public SystemDatabaseService (IConfiguration config) {
            _configuration = config;
            _connectionString = config.GetConnectionString ("db");
        }

        private SqlConnection DatabaseConnector () {
            SqlConnection connection = new SqlConnection (_connectionString);
            return connection;
        }

        public IEnumerable<SystemEntity> SelectAll () {
            var systems = new List<SystemEntity> ();
            try {
                var connection = DatabaseConnector ();
                using (connection) {
                    connection.Open ();
                    String sql = ("SELECT systemId, systems.name, code, description, techStack, " +
                        "users.userId, users.forename, users.surname, users.role, " +
                        "teams.teamId, teams.name " +
                        "FROM systems " +
                        "JOIN users ON users.userId = systems.pointOfContact " +
                        "JOIN teams ON teams.teamId = systems.owningTeam");

                    using (SqlCommand command = new SqlCommand (sql, connection)) {
                        using (SqlDataReader reader = command.ExecuteReader ()) {
                            while (reader.Read ()) {
                                systems.Add (new SystemEntity (reader.GetInt32 (0), reader.GetString (1), reader.GetString (2), reader.GetString (3), reader.GetString (4),
                                    new User (reader.GetInt32 (5), reader.GetString (6), reader.GetString (7), reader.GetString (8)),
                                    new Team (reader.GetInt32 (9), reader.GetString (10))));
                            }
                        }
                    }
                }
                return systems;
            } catch (SqlException e) {
                Console.WriteLine (e.ToString ());
                return systems;
            }
        }

        public IEnumerable<SystemEntity> SelectAll (int id) {
            var systems = new List<SystemEntity> ();
            try {
                var connection = DatabaseConnector ();
                using (connection) {
                    connection.Open ();
                    String sql = ("SELECT systemId, systems.name, code, description, techStack, " +
                        "users.userId, users.forename, users.surname, users.role, " +
                        "teams.teamId, teams.name FROM systems " +
                        "JOIN users ON users.userId = systems.pointOfContact " +
                        "JOIN teams ON teams.teamId = systems.owningTeam " +
                        "WHERE systems.owningTeam = " + id);

                    using (SqlCommand command = new SqlCommand (sql, connection)) {
                        using (SqlDataReader reader = command.ExecuteReader ()) {
                            while (reader.Read ()) {
                                systems.Add (new SystemEntity (reader.GetInt32 (0), reader.GetString (1), reader.GetString (2), reader.GetString (3), reader.GetString (4),
                                    new User (reader.GetInt32 (5), reader.GetString (6), reader.GetString (7), reader.GetString (8)),
                                    new Team (reader.GetInt32 (9), reader.GetString (10))));
                            }
                        }
                    }
                }
                return systems;
            } catch (SqlException e) {
                Console.WriteLine (e.ToString ());
                return systems;
            }
        }

        public SystemEntity Select (int id) {
            var system = new SystemEntity ();
            try {
                var connection = DatabaseConnector ();
                using (connection) {
                    connection.Open ();
                    String sql = ("SELECT systemId, systems.name, code, description, techStack, " +
                        "users.userId, users.forename, users.surname, users.role, " +
                        "teams.teamId, teams.name FROM systems " +
                        "JOIN users ON users.userId = systems.pointOfContact " +
                        "JOIN teams ON teams.teamId = systems.owningTeam " +
                        "WHERE systems.systemId = " + id);

                    using (SqlCommand command = new SqlCommand (sql, connection)) {
                        using (SqlDataReader reader = command.ExecuteReader ()) {
                            while (reader.Read ()) {
                                system = new SystemEntity (reader.GetInt32 (0), reader.GetString (1), reader.GetString (2), reader.GetString (3), reader.GetString (4),
                                    new User (reader.GetInt32 (5), reader.GetString (6), reader.GetString (7), reader.GetString (8)),
                                    new Team (reader.GetInt32 (9), reader.GetString (10)));
                            }
                        }
                    }
                }
                return system;
            } catch (SqlException e) {
                Console.WriteLine (e.ToString ());
                return system;
            }
        }

        public void Insert (SystemEntity s) {
            try {
                var connection = DatabaseConnector ();
                using (connection) {
                    connection.Open ();
                    string sql2 = "INSERT INTO systems(name, code, description, techStack, pointOfContact, owningTeam)   VALUES(@param1, @param2, @param3, @param4, @param5, @param6)";

                    using (SqlCommand cmd = new SqlCommand (sql2, connection)) {
                        cmd.Parameters.Add ("@param1", SqlDbType.NVarChar, 50).Value = s.name;
                        cmd.Parameters.Add ("@param2", SqlDbType.NVarChar, 50).Value = s.code;
                        cmd.Parameters.Add ("@param3", SqlDbType.NVarChar, 50).Value = s.description;
                        cmd.Parameters.Add ("@param4", SqlDbType.NVarChar, 50).Value = s.techStack;
                        cmd.Parameters.Add ("@param5", SqlDbType.Int).Value = s.pointOfContactID;
                        cmd.Parameters.Add ("@param6", SqlDbType.Int).Value = s.owningTeamID;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery ();
                    }
                }
            } catch (SqlException e) {
                Console.WriteLine (e.ToString ());
            }
        }

        public void Update (SystemEntity s) {
            try {
                var connection = DatabaseConnector ();
                using (connection) {
                    connection.Open ();

                    string sql = "UPDATE systems " +
                        "SET name=@param1, code=@param2, " +
                        "description=@param3, techStack=@param4, pointOfContact=@param5, " +
                        "owningTeam=@param6 " +
                        "WHERE systemId = " + s.systemID;

                    using (SqlCommand cmd = new SqlCommand (sql, connection)) {
                        cmd.Parameters.Add ("@param1", SqlDbType.NVarChar, 50).Value = s.name;
                        cmd.Parameters.Add ("@param2", SqlDbType.NVarChar, 50).Value = s.code;
                        cmd.Parameters.Add ("@param3", SqlDbType.NVarChar, 50).Value = s.description;
                        cmd.Parameters.Add ("@param4", SqlDbType.NVarChar, 50).Value = s.techStack;
                        cmd.Parameters.Add ("@param5", SqlDbType.Int).Value = s.pointOfContactID;
                        cmd.Parameters.Add ("@param6", SqlDbType.Int).Value = s.owningTeamID;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery ();
                    }
                }
            } catch (SqlException e) {
                Console.WriteLine (e.ToString ());
            }
        }
    }
}