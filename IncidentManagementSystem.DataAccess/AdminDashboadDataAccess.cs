using IncidentManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentManagementSystem.DataAccess
{
    public class AdminDashboadDataAccess : IAdminDashboadDataAccess
    {
        private string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public List<TicketDto> GetTicketList(string userId)
        {
            List<TicketDto> Ticketlist = new List<TicketDto>();
            
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = @"GetTotalTicketCount";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@userId", userId);

                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                Ticketlist.Add(new TicketDto()
                                {
                                    InstId = sqlDataReader["InstId"].ToString(),
                                    TicketId = sqlDataReader["TicketId"].ToString()
                                   
                                });
                            }
                        }

                        conn.Close();

                    }
                }

                return Ticketlist;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ticketlist;
        }
    }
}

public interface IAdminDashboadDataAccess 
{
    List<TicketDto> GetTicketList(string userId);
}
