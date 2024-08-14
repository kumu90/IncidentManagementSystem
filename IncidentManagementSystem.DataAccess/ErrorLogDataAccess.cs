using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncidentManagementSystem.Model;

namespace IncidentManagementSystem.DataAccess
{
    public class ErrorLogDataAccess : IErrorLogDataAccess
    {
        private readonly string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public void LogError(ErrorLogDto errorLog)
        {
            using (var connection = new SqlConnection(conStr))
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

public interface IErrorLogDataAccess
{
    void LogError(ErrorLogDto errorLog);
}