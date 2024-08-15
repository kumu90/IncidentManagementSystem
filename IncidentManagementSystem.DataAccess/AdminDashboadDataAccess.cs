using IncidentManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

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

        public List<TicketDto> GetTicketPandingStatusList(string userId)
        {
            List<TicketDto> Pandinglist = new List<TicketDto>();

            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = @"GetTicketStatusPanding";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@userId", userId);

                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                Pandinglist.Add(new TicketDto()
                                {
                                    TicketId =  sqlDataReader["TicketId"].ToString()

                                });
                            }
                        }

                        conn.Close();
                    }
                }

                return Pandinglist;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Pandinglist;
        }

        public List<ResolvedByDto> GetResolveList(string userId)
        {
            List<ResolvedByDto> resolvedlist = new List<ResolvedByDto>();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {
                        cmd.CommandText = "GetTicketStatusResolve";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@userId", userId));
                        cmd.Connection = conn;

                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                resolvedlist.Add(new ResolvedByDto()
                                {
                                   
                                    TicketId = sqlDataReader["TicketId"].ToString()
                                    
                                });

                            }
                        }
                    }

                    conn.Close();

                }
                return resolvedlist;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return resolvedlist;

        }

        public List<TicketDto> GetTicketRejectStatusList(string userId)
        {
            List<TicketDto> Rejectlist = new List<TicketDto>();

            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = @"GetTicketStatusReject";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@userId", userId);

                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                Rejectlist.Add(new TicketDto()
                                {
                                    TicketId = sqlDataReader["TicketId"].ToString()

                                });
                            }
                        }

                        conn.Close();
                    }
                }

                return Rejectlist;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Rejectlist;
        }
    }
}

public interface IAdminDashboadDataAccess 
{
    List<TicketDto> GetTicketList(string userId);
    List<TicketDto> GetTicketPandingStatusList(string userId);
    List<ResolvedByDto> GetResolveList(string userId);
    List<TicketDto> GetTicketRejectStatusList(string userId);
}
