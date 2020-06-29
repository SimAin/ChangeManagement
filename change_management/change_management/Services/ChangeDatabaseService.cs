using System;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

using change_management.Models;

namespace change_management.Controllers
{
    public class ChangeDatabaseService : IDatabaseService<Change>
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ChangeDatabaseService(IConfiguration config){
            _configuration = config;
            _connectionString = config.GetConnectionString("db");
        } 

        private SqlConnection DatabaseConnector() {
            SqlConnection connection = new SqlConnection(_connectionString);
            return connection;
        }

        private string GetCompleteChangeSql() {
            return ("SELECT changes.changeId, " +
                                        "systems.systemId, systems.name, systems.code, systems.description, systems.techStack, " +
                                        "changes.type, changes.description, changes.criticality, changes.deadline, changes.priority, " + 
                                        "changes.processingTimeDays, changes.dateCreated," + 
                                        "status.status, " +
                                        "approver.userId, approver.forename, approver.surname, approver.role, " +
                                        "stakeholder.userId, stakeholder.forename, stakeholder.surname, stakeholder.role, " +
                                        "teams.teamId, teams.name, " +
                                        "responsible.userId, responsible.forename, responsible.surname, responsible.role " +
                                    "FROM changes " + 
                                    "JOIN systems ON systems.systemId = changes.systemId " +
                                    "JOIN status ON status.statusId = changes.statusId " +
                                    "JOIN users AS approver ON approver.userId = changes.approverId " +
                                    "JOIN users AS stakeholder ON stakeholder.userId = changes.stakeholderId " +
                                    "JOIN users AS responsible ON responsible.userId = changes.userResponsibleId " + 
                                    "JOIN teams ON teamId = changes.teamResponsibleId");
        }

        private Change CreateChange(SqlDataReader reader){
            return new Change(reader.GetInt32(0), 
                                new SystemEntity(reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5)),
                                reader.GetString(6), reader.GetString(7), reader.GetBoolean(8), reader.GetDateTime(9), reader.GetInt32(10), 
                                reader.GetInt32(11), reader.GetDateTime(12),
                                reader.GetString(13),
                                new User(reader.GetInt32(14), reader.GetString(15), reader.GetString(16), reader.GetString(17)),
                                new User(reader.GetInt32(18), reader.GetString(19), reader.GetString(20), reader.GetString(21)),  
                                new Team(reader.GetInt32(22), reader.GetString(23)),
                                new User(reader.GetInt32(24), reader.GetString(25), reader.GetString(26), reader.GetString(27)));
        }

        public IEnumerable<Change> SelectAll(){
            var changes = new List<Change>();
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();       
                    String sql = GetCompleteChangeSql();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                changes.Add(CreateChange(reader));
                            }
                        }
                    }
                }
                return changes;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return changes;
            }
        }

        public Change Select(int changeId){
            var change = new Change();
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();       
                    String sql = GetCompleteChangeSql() + " WHERE changes.changeId = " + changeId;

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                change = CreateChange(reader);
                            }
                        }
                    }
                }
                return change;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return change;
            }
        }

        public IEnumerable<Change> SelectTeamChanges(int id){
            var changes = new List<Change>();
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();       
                    String sql = ("SELECT changes.changeId, " +
                                        "systems.systemId, systems.name, systems.code, systems.description, systems.techStack, " +
                                        "changes.type, changes.description, changes.criticality, changes.deadline, changes.priority, changes.processingTimeDays, " +
                                        "responsible.userId, responsible.forename, responsible.surname, responsible.role " +
                                    "FROM changes " + 
                                    "JOIN systems ON systems.systemId = changes.systemId " +
                                    "JOIN users AS responsible ON responsible.userId = changes.userResponsibleId " + 
                                    "JOIN teams ON teamId = changes.teamResponsibleId " +
                                    "WHERE changes.teamResponsibleId = " + id);

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                changes.Add(new Change(reader.GetInt32(0), 
                                            new SystemEntity(reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5)),
                                            reader.GetString(6), reader.GetString(7), reader.GetBoolean(8), reader.GetDateTime(9), reader.GetInt32(10), reader.GetInt32(11),
                                            new User(reader.GetInt32(12), reader.GetString(13), reader.GetString(14), reader.GetString(15))));
                            }
                        }
                    }
                }
                return changes;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return changes;
            }
        }

        public IEnumerable<Change> SelectSystemChanges(int systemID){
            var changes = new List<Change>();
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();       
                    String sql = GetCompleteChangeSql() + " WHERE systems.systemId = " + systemID;

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                changes.Add(CreateChange(reader));
                            }
                        }
                    }
                }
                return changes;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                return changes;
            }
        }

        public void Insert(Change c){
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();
                    string sql = "INSERT INTO changes(systemId, type, description, criticality, deadline, priority, " +
                                    "approverId, stakeholderId, teamResponsibleId, userResponsibleId, processingTimeDays, statusId, dateCreated)" +
                                    "VALUES(@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8, @param9, @param10, @param11, @param12, @param13)";
                            
                    using(SqlCommand cmd = new SqlCommand(sql,connection)) 
                    {
                        cmd.Parameters.Add("@param1", SqlDbType.Int).Value = c.systemId;
                        cmd.Parameters.Add("@param2", SqlDbType.NVarChar, 50).Value = c.type;
                        cmd.Parameters.Add("@param3", SqlDbType.NVarChar, 50).Value = c.description;
                        cmd.Parameters.Add("@param4", SqlDbType.Bit).Value = c.criticality;
                        cmd.Parameters.Add("@param5", SqlDbType.DateTime).Value = c.deadline;
                        cmd.Parameters.Add("@param6", SqlDbType.Int).Value = c.priority;
                        cmd.Parameters.Add("@param7", SqlDbType.Int).Value = c.approverId;
                        cmd.Parameters.Add("@param8", SqlDbType.Int).Value = c.stakeholderId;
                        cmd.Parameters.Add("@param9", SqlDbType.Int).Value = c.teamResponsibleId;
                        cmd.Parameters.Add("@param10", SqlDbType.Int).Value = c.userResponsibleId;
                        cmd.Parameters.Add("@param11", SqlDbType.Int).Value = c.processingTime;
                        cmd.Parameters.Add("@param12", SqlDbType.Int).Value = 1;
                        cmd.Parameters.Add("@param13", SqlDbType.DateTime).Value = DateTime.Now;
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

        public void Update(Change c){
            try {
                var connection = DatabaseConnector();
                using (connection)
                {
                    connection.Open();

                    string sql ="UPDATE changes " +
                                "SET description=@param1, criticality=@param2, " +
                                    "deadline=@param3, priority=@param4, approverId=@param5, " + 
                                    "stakeholderId=@param6, teamResponsibleId=@param7, userResponsibleId=@param8, " +
                                    "processingTimeDays=@param9, statusId=@param10 " +
                                "WHERE changeId = " + c.changeId;

                    using(SqlCommand cmd = new SqlCommand(sql,connection)) 
                    {
                        cmd.Parameters.Add("@param1", SqlDbType.NVarChar, 50).Value = c.description;
                        cmd.Parameters.Add("@param2", SqlDbType.Bit).Value = c.criticality;
                        cmd.Parameters.Add("@param3", SqlDbType.DateTime).Value = c.deadline;
                        cmd.Parameters.Add("@param4", SqlDbType.Int).Value = c.priority;
                        cmd.Parameters.Add("@param5", SqlDbType.Int).Value = c.approverId;
                        cmd.Parameters.Add("@param6", SqlDbType.Int).Value = c.stakeholderId;
                        cmd.Parameters.Add("@param7", SqlDbType.Int).Value = c.teamResponsibleId;
                        cmd.Parameters.Add("@param8", SqlDbType.Int).Value = c.userResponsibleId;
                        cmd.Parameters.Add("@param9", SqlDbType.Int).Value = c.processingTime;
                        cmd.Parameters.Add("@param10", SqlDbType.Int).Value = 1;
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