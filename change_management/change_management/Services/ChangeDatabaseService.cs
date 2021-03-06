using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using change_management.Models;
using change_management.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace change_management.Controllers {
    public class ChangeDatabaseService : IDatabaseService<Change> {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public ChangeDatabaseService (IConfiguration config) {
            _configuration = config;
            _connectionString = config.GetConnectionString ("db");
        }

        private SqlConnection DatabaseConnector () {
            SqlConnection connection = new SqlConnection (_connectionString);
            return connection;
        }

        private string GetCompleteChangeSql () {
            return ("SELECT changes.changeId, " +
                "systems.systemId, systems.name, systems.code, systems.description, systems.techStack, " +
                "changes.type, changes.description, changes.criticality, changes.deadline, changes.priority, " +
                "changes.processingTimeDays, changes.dateCreated, changes.dateStarted, " +
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
                "LEFT JOIN users AS responsible ON responsible.userId = changes.userResponsibleId " +
                "JOIN teams ON teamId = changes.teamResponsibleId");
        }

        private Change CreateChange (SqlDataReader reader) {
            return new Change (reader.GetInt32 (0),
                new SystemEntity (reader.GetInt32 (1), reader.GetString (2), reader.GetString (3), reader.GetString (4), reader.GetString (5)),
                reader.GetString (6), reader.GetString (7), reader.GetBoolean (8), reader.GetDateTime (9), reader.GetInt32 (10),
                reader.GetInt32 (11), reader.GetDateTime (12), reader.IsDBNull (13) ? (DateTime?) null : (DateTime?) reader.GetDateTime (13),
                reader.GetString (14),
                new User (reader.GetInt32 (15), reader.GetString (16), reader.GetString (17), reader.GetString (18)),
                new User (reader.GetInt32 (19), reader.GetString (20), reader.GetString (21), reader.GetString (22)),
                new Team (reader.GetInt32 (23), reader.GetString (24)),
                new User (reader.IsDBNull (25) ? (int) 0 : (int) reader.GetInt32 (25), reader.IsDBNull (26) ? "" : (string) reader.GetString (26), reader.IsDBNull (27) ? "" : (string) reader.GetString (27), reader.IsDBNull (28) ? "" : (string) reader.GetString (28)));
        }

        public IEnumerable<Change> SelectAll () {
            var changes = new List<Change> ();
            try {
                var connection = DatabaseConnector ();
                using (connection) {
                    connection.Open ();
                    String sql = GetCompleteChangeSql ();

                    using (SqlCommand command = new SqlCommand (sql, connection)) {
                        using (SqlDataReader reader = command.ExecuteReader ()) {
                            while (reader.Read ()) {
                                changes.Add (CreateChange (reader));
                            }
                        }
                    }
                }
                return changes;
            } catch (SqlException e) {
                Console.WriteLine (e.ToString ());
                return changes;
            }
        }

        public Change Select (int changeId) {
            var change = new Change ();
            try {
                var connection = DatabaseConnector ();
                using (connection) {
                    connection.Open ();
                    String sql = GetCompleteChangeSql () + " WHERE changes.changeId = " + changeId;

                    using (SqlCommand command = new SqlCommand (sql, connection)) {
                        using (SqlDataReader reader = command.ExecuteReader ()) {
                            while (reader.Read ()) {
                                change = CreateChange (reader);
                            }
                        }
                    }
                }
                return change;
            } catch (SqlException e) {
                Console.WriteLine (e.ToString ());
                return change;
            }
        }

        public IEnumerable<ChangeAudit> SelectChangeAudit (int changeId) {
            var audit = new List<ChangeAudit> ();
            try {
                var connection = DatabaseConnector ();
                using (connection) {
                    connection.Open ();
                    String sql = GetCompleteChangeSql () +
                        " JOIN users AS changer ON changer.userId = changeAudit.updateUserId " +
                        "WHERE changeAudit.changeId = " + changeId;

                    sql = sql.Replace ("FROM changes", ", changeAudit.changeAuditId,  changeAudit.auditType, changeAudit.comment, " +
                        "changer.userId, changer.forename, changer.surname, changer.role,  changeAudit.auditDate FROM changeAudit");

                    sql = sql.Replace ("changes.", "changeAudit.");

                    using (SqlCommand command = new SqlCommand (sql, connection)) {
                        using (SqlDataReader reader = command.ExecuteReader ()) {
                            while (reader.Read ()) {
                                audit.Add (new ChangeAudit (CreateChange (reader), reader.GetInt32 (29), reader.GetString (30), reader.GetString (31),
                                    new User (reader.GetInt32 (32), reader.GetString (33), reader.GetString (34), reader.GetString (35)), reader.GetDateTime (36)));
                            }
                        }
                    }
                }
                return audit;
            } catch (SqlException e) {
                Console.WriteLine (e.ToString ());
                return audit;
            }
        }

        public IEnumerable<Change> SelectTeamPendingChanges (int id) {
            var changes = new List<Change> ();
            try {
                var connection = DatabaseConnector ();
                using (connection) {
                    connection.Open ();
                    String sql = ("SELECT changes.changeId, " +
                        "systems.systemId, systems.name, systems.code, systems.description, systems.techStack, " +
                        "changes.type, changes.description, changes.criticality, changes.deadline, changes.priority, " +
                        "changes.processingTimeDays, changes.dateCreated, changes.dateStarted, " +
                        "status.status, " +
                        "responsible.userId, responsible.forename, responsible.surname, responsible.role " +
                        "FROM changes " +
                        "JOIN systems ON systems.systemId = changes.systemId " +
                        "JOIN status ON status.statusId = changes.statusId " +
                        "LEFT JOIN users AS responsible ON responsible.userId = changes.userResponsibleId " +
                        "JOIN teams ON teamId = changes.teamResponsibleId " +
                        "WHERE changes.teamResponsibleId = " + id + " AND status.status != 'Complete'");

                    using (SqlCommand command = new SqlCommand (sql, connection)) {
                        using (SqlDataReader reader = command.ExecuteReader ()) {
                            while (reader.Read ()) {
                                changes.Add (new Change (reader.GetInt32 (0),
                                    new SystemEntity (reader.GetInt32 (1), reader.GetString (2), reader.GetString (3), reader.GetString (4), reader.GetString (5)),
                                    reader.GetString (6), reader.GetString (7), reader.GetBoolean (8), reader.GetDateTime (9), reader.GetInt32 (10),
                                    reader.GetInt32 (11), reader.GetDateTime (12), reader.IsDBNull (13) ? (DateTime?) null : (DateTime?) reader.GetDateTime (13),
                                    reader.GetString (14),
                                    new User (reader.IsDBNull (15) ? (int) 0 : (int) reader.GetInt32 (15), reader.IsDBNull (16) ? "" : (string) reader.GetString (16), reader.IsDBNull (17) ? "" : (string) reader.GetString (17), reader.IsDBNull (18) ? "" : (string) reader.GetString (18))));
                            }
                        }
                    }
                }
                return changes;
            } catch (SqlException e) {
                Console.WriteLine (e.ToString ());
                return changes;
            }
        }

        public IEnumerable<Change> SelectSystemChanges (int systemID) {
            var changes = new List<Change> ();
            try {
                var connection = DatabaseConnector ();
                using (connection) {
                    connection.Open ();
                    String sql = GetCompleteChangeSql () + " WHERE systems.systemId = " + systemID;

                    using (SqlCommand command = new SqlCommand (sql, connection)) {
                        using (SqlDataReader reader = command.ExecuteReader ()) {
                            while (reader.Read ()) {
                                changes.Add (CreateChange (reader));
                            }
                        }
                    }
                }
                return changes;
            } catch (SqlException e) {
                Console.WriteLine (e.ToString ());
                return changes;
            }
        }

        public void Insert (Change c) {
            string userResponsible = "";
            string userResponsibleText = "";
            if (c.userResponsibleId != 0) {
                userResponsibleText = "userResponsibleId,";
                userResponsible = "@param10,";
            }
            c.statusId = 1;
            c.createdDate = DateTime.Now;
            try {
                var connection = DatabaseConnector ();
                using (connection) {
                    connection.Open ();
                    string sql = "INSERT INTO changes(systemId, type, description, criticality, deadline, priority, " +
                        "approverId, stakeholderId, teamResponsibleId, " + userResponsibleText + " processingTimeDays, statusId, dateCreated)" +
                        "VALUES(@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8, @param9, " + userResponsible + " @param11, @param12, @param13) " +
                        "SELECT IDENT_CURRENT('changes') AS newId";

                    using (SqlCommand cmd = new SqlCommand (sql, connection)) {
                        cmd.Parameters.Add ("@param1", SqlDbType.Int).Value = c.systemId;
                        cmd.Parameters.Add ("@param2", SqlDbType.NVarChar, 50).Value = c.type;
                        cmd.Parameters.Add ("@param3", SqlDbType.NVarChar, 50).Value = c.description;
                        cmd.Parameters.Add ("@param4", SqlDbType.Bit).Value = c.criticality;
                        cmd.Parameters.Add ("@param5", SqlDbType.DateTime).Value = c.deadline;
                        cmd.Parameters.Add ("@param6", SqlDbType.Int).Value = c.priority;
                        cmd.Parameters.Add ("@param7", SqlDbType.Int).Value = c.approverId;
                        cmd.Parameters.Add ("@param8", SqlDbType.Int).Value = c.stakeholderId;
                        cmd.Parameters.Add ("@param9", SqlDbType.Int).Value = c.teamResponsibleId;
                        if (c.userResponsibleId != 0) {
                            cmd.Parameters.Add ("@param10", SqlDbType.Int).Value = c.userResponsibleId;
                        }
                        cmd.Parameters.Add ("@param11", SqlDbType.Int).Value = c.processingTime;
                        cmd.Parameters.Add ("@param12", SqlDbType.Int).Value = c.statusId;
                        cmd.Parameters.Add ("@param13", SqlDbType.DateTime).Value = c.createdDate;
                        cmd.CommandType = CommandType.Text;

                        c.changeId = Convert.ToInt32 (cmd.ExecuteScalar ());
                    }
                }

                Audit (c, "Insert");
            } catch (SqlException e) {
                Console.WriteLine (e.ToString ());
            }
        }

        public void Update (Change c, string comments = "n/a") {

            var startedDate = "";
            if (c.startedDate.HasValue) {
                startedDate = ", dateStarted=@param11 ";
            }
            string userResponsibleText = "";
            if (c.userResponsibleId != 0) {
                userResponsibleText = "userResponsibleId=@param8, ";
            }

            try {
                var connection = DatabaseConnector ();
                using (connection) {
                    connection.Open ();

                    string sql = "UPDATE changes " +
                        "SET description=@param1, criticality=@param2, " +
                        "deadline=@param3, priority=@param4, approverId=@param5, " +
                        "stakeholderId=@param6, teamResponsibleId=@param7, " + userResponsibleText +
                        "processingTimeDays=@param9, statusId=@param10 " + startedDate +
                        "WHERE changeId = " + c.changeId;

                    using (SqlCommand cmd = new SqlCommand (sql, connection)) {
                        cmd.Parameters.Add ("@param1", SqlDbType.NVarChar, 50).Value = c.description;
                        cmd.Parameters.Add ("@param2", SqlDbType.Bit).Value = c.criticality;
                        cmd.Parameters.Add ("@param3", SqlDbType.DateTime).Value = c.deadline;
                        cmd.Parameters.Add ("@param4", SqlDbType.Int).Value = c.priority;
                        cmd.Parameters.Add ("@param5", SqlDbType.Int).Value = c.approverId;
                        cmd.Parameters.Add ("@param6", SqlDbType.Int).Value = c.stakeholderId;
                        cmd.Parameters.Add ("@param7", SqlDbType.Int).Value = c.teamResponsibleId;
                        if (c.userResponsibleId != 0) {
                            cmd.Parameters.Add ("@param8", SqlDbType.Int).Value = c.userResponsibleId;
                        }
                        cmd.Parameters.Add ("@param9", SqlDbType.Int).Value = c.processingTime;
                        cmd.Parameters.Add ("@param10", SqlDbType.Int).Value = c.statusId;
                        if (c.startedDate.HasValue) {
                            cmd.Parameters.Add ("@param11", SqlDbType.DateTime).Value = c.startedDate;
                        }

                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery ();
                    }
                }
                Audit (c, "Update", comments);
            } catch (SqlException e) {
                Console.WriteLine (e.ToString ());
            }
        }

        public void Audit (Change c, string type = "n/a", string comment = "n/a") {

            if (type == "Update") {
                var originalChange = Select (c.changeId);
                c.systemId = originalChange.system.systemID;
                c.type = originalChange.type;
                c.createdDate = originalChange.createdDate;
            }

            string userResponsible = "";
            string userResponsibleText = "";
            if (c.userResponsibleId != 0) {
                userResponsibleText = "userResponsibleId,";
                userResponsible = "@param13,";
            }
            try {
                var connection = DatabaseConnector ();
                using (connection) {
                    connection.Open ();
                    string sql = "INSERT INTO changeAudit(auditType, changeId, systemId, updateUserId, type, description, criticality, deadline, priority, " +
                        "approverId, stakeholderId, teamResponsibleId, " + userResponsibleText + " processingTimeDays, statusId, comment, dateCreated, auditDate)" +
                        "VALUES(@param1, @param2, @param3, @param4, @param5, @param6, @param7, @param8, " +
                        "@param9, @param10, @param11, @param12, " + userResponsible + " @param14, @param15, @param16, @param17, @param18)";

                    using (SqlCommand cmd = new SqlCommand (sql, connection)) {
                        cmd.Parameters.Add ("@param1", SqlDbType.NVarChar, 50).Value = type;
                        cmd.Parameters.Add ("@param2", SqlDbType.Int).Value = c.changeId;
                        cmd.Parameters.Add ("@param3", SqlDbType.Int).Value = c.systemId;
                        cmd.Parameters.Add ("@param4", SqlDbType.Int).Value = SessionService.loggedInUser.userID;
                        cmd.Parameters.Add ("@param5", SqlDbType.NVarChar, 50).Value = c.type;
                        cmd.Parameters.Add ("@param6", SqlDbType.NVarChar, 50).Value = c.description;
                        cmd.Parameters.Add ("@param7", SqlDbType.Bit).Value = c.criticality;
                        cmd.Parameters.Add ("@param8", SqlDbType.DateTime).Value = c.deadline;
                        cmd.Parameters.Add ("@param9", SqlDbType.Int).Value = c.priority;
                        cmd.Parameters.Add ("@param10", SqlDbType.Int).Value = c.approverId;
                        cmd.Parameters.Add ("@param11", SqlDbType.Int).Value = c.stakeholderId;
                        cmd.Parameters.Add ("@param12", SqlDbType.Int).Value = c.teamResponsibleId;
                        if (c.userResponsibleId != 0) {
                            cmd.Parameters.Add ("@param13", SqlDbType.Int).Value = c.userResponsibleId;
                        }
                        cmd.Parameters.Add ("@param14", SqlDbType.Int).Value = c.processingTime;
                        cmd.Parameters.Add ("@param15", SqlDbType.Int).Value = c.statusId;
                        cmd.Parameters.Add ("@param16", SqlDbType.NVarChar, 50).Value = comment;
                        cmd.Parameters.Add ("@param17", SqlDbType.DateTime).Value = c.createdDate;
                        cmd.Parameters.Add ("@param18", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery ();
                    }
                }
            } catch (SqlException e) {
                Console.WriteLine (e.ToString ());
            }
        }

        public IEnumerable<Status> SelectStatuss () {
            var statuss = new List<Status> ();
            try {
                var connection = DatabaseConnector ();
                using (connection) {
                    connection.Open ();
                    String sql = "SELECT * FROM status";

                    using (SqlCommand command = new SqlCommand (sql, connection)) {
                        using (SqlDataReader reader = command.ExecuteReader ()) {
                            while (reader.Read ()) {
                                statuss.Add (new Status (reader.GetInt32 (0), reader.GetString (1)));
                            }
                        }
                    }
                }
                return statuss;
            } catch (SqlException e) {
                Console.WriteLine (e.ToString ());
                return statuss;
            }
        }

        public int SelectChangeStatus (int changeId) {
            var status = 0;
            try {
                var connection = DatabaseConnector ();
                using (connection) {
                    connection.Open ();
                    String sql = "SELECT statusId " +
                        "FROM changes " +
                        " WHERE changes.changeId = " + changeId;

                    using (SqlCommand command = new SqlCommand (sql, connection)) {
                        using (SqlDataReader reader = command.ExecuteReader ()) {
                            while (reader.Read ()) {
                                status = (reader.GetInt32 (0));
                            }
                        }
                    }
                }
                return status;
            } catch (SqlException e) {
                Console.WriteLine (e.ToString ());
                return status;
            }
        }
    }
}