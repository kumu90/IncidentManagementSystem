﻿using IncidentManagementSystem.Model;
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

    public class TicketDataAccess : ITicketDataAccess
    {
        private string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public SQLStatusDto TicketCreate(TicketDto _ticketDto)
        {
            SQLStatusDto _sQLStatus = new SQLStatusDto();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = @"InstitutionTicketCreate";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@TicketId", _ticketDto.TicketId);
                        cmd.Parameters.AddWithValue("@ServiceId", _ticketDto.ServiceId);
                        cmd.Parameters.AddWithValue("@IssueId", _ticketDto.IssueId);
                        cmd.Parameters.AddWithValue("@InstId", _ticketDto.InstId);
                        cmd.Parameters.AddWithValue("@Description", _ticketDto.Description);
                        cmd.Parameters.AddWithValue("@CellNumber", _ticketDto.CellNumber);
                        cmd.Parameters.AddWithValue("@Email", _ticketDto.Email);
                        cmd.Parameters.AddWithValue("@ImageUrl", _ticketDto.ImageUrl ?? "");

                        conn.Open();
                        //cmd.ExecuteNonQuery();
                        //conn.Close();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                _sQLStatus.Status = rdr["Status"].ToString();
                                _sQLStatus.Message = rdr["Message"].ToString();
                            }
                        }


                    }
                }

                return _sQLStatus;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return _sQLStatus;
        }
        public List<TicketDto> TicketInfo(string search = "")
        {
            List<TicketDto> Ticketlist = new List<TicketDto>();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = @"TicketName";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Search", search ?? ""));
                        cmd.Connection = conn;

                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                Ticketlist.Add(new TicketDto()
                                {
                                    TicketId = sqlDataReader["TicketId"].ToString(),
                                    //Description = sqlDataReader["Description"].ToString(),
                                    //status = Convert.ToBoolean(sqlDataReader["InstId"].ToString()),
                                    InstId = sqlDataReader["InstitutionName"].ToString(),
                                    ServiceId = sqlDataReader["ServiceName"].ToString(),
                                    IssueId = sqlDataReader["Issue"].ToString(),
                                    CellNumber = sqlDataReader["CellNumber"].ToString(),
                                    Email = sqlDataReader["Email"].ToString()

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
            return new List<TicketDto>();
        }

        public TicketDto GetTicketDetails(string TicketId)
        {
            TicketDto Ticketlist = new TicketDto();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = "GetTicketDetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", TicketId));
                        cmd.Connection = conn;

                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            if (sqlDataReader.Read())
                            {
                                Ticketlist = new TicketDto()
                                {
                                    TicketId = sqlDataReader["TicketId"].ToString(),
                                    date = Convert.ToDateTime(sqlDataReader["Date"].ToString()),
                                    status = sqlDataReader["status"].ToString(),
                                    InstId = sqlDataReader["InstitutionName"].ToString(),
                                    ServiceId = sqlDataReader["ServiceName"].ToString(),
                                    IssueId = sqlDataReader["Issue"].ToString(),
                                    CellNumber = sqlDataReader["CellNumber"].ToString(),
                                    Email = sqlDataReader["Email"].ToString(),
                                    Description = sqlDataReader["Description"].ToString(),
                                    ImageUrl = sqlDataReader["ImageUrl"].ToString()

                                };


                            }
                        }
                    }

                    conn.Close();

                }
                return Ticketlist;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ticketlist;
        }

        public List<IssueDto> GetIssuesList(string ServiceId = "")
        {
            List<IssueDto> Issuelist = new List<IssueDto>();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = @"IssueList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                Issuelist.Add(new IssueDto()
                                {
                                    IssueId = Convert.ToInt32(sqlDataReader["IssueId"].ToString()),
                                    IssueName = sqlDataReader["Issue"].ToString()
                                });
                            }
                        }

                        conn.Close();

                    }
                }

                return Issuelist;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<IssueDto>();
        }

        public TicketAssignDto TicketAssign(string TicketId="")
        {
            TicketAssignDto ticketAssignDto = new TicketAssignDto();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = "TicketAssign";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@TicketId", TicketId));
                        cmd.Connection = conn;

                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            if (sqlDataReader.Read())
                            {
                                ticketAssignDto = new TicketAssignDto()
                                {
                                    TicketId = sqlDataReader["TicketId"].ToString(),
                                    Date = Convert.ToDateTime(sqlDataReader["Date"].ToString()),
                                    Status = sqlDataReader["status"].ToString(),
                                    IssueId= sqlDataReader["IssueId"].ToString()
                                    

                                };


                            }
                        }
                    }

                }
                return ticketAssignDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ticketAssignDto;
        }
    }
}
public interface ITicketDataAccess
{
    SQLStatusDto TicketCreate(TicketDto ticketDto);
    List<TicketDto> TicketInfo(string search = "");

    TicketDto GetTicketDetails(string TicketId);

    List<IssueDto> GetIssuesList(string ServiceId = "");
    TicketAssignDto TicketAssign(string TicketId="");
}
