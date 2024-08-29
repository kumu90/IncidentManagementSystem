using IncidentManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace IncidentManagementSystem.Common
{
    public static class ExceptionLogs
    {
        ///private readonly string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public  static void LogError(this Exception exception, ErrorLogDto errorLog)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var command = new SqlCommand("InsertErrorLog", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ExceptionMessage", errorLog.ExceptionMessage);
                    command.Parameters.AddWithValue("@StackTrace", errorLog.StackTrace);
                    command.Parameters.AddWithValue("@ControllerName", errorLog.ControllerName);
                    command.Parameters.AddWithValue("@ActionName", errorLog.ActionName);
                    command.Parameters.AddWithValue("@UserId", (object)errorLog.userId ?? DBNull.Value);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
