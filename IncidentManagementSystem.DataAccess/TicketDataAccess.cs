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
    public class TicketDataAccess : ITicketDataAccess
    {
        public SQLStatusDto TicketCreate(TicketDto _ticketDto)
        {
            SQLStatusDto _sQLStatus = new SQLStatusDto();
            try
            {
                string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
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
                string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
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

        public TicketDto GetTicketDetails(int TicketId)
        {
            TicketDto Ticketlist = new TicketDto();
            try
            {
                string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
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
                                    status = Convert.ToBoolean(sqlDataReader["status"].ToString()),
                                    InstId = sqlDataReader["InstitutionName"].ToString(),
                                    ServiceId = sqlDataReader["ServiceName"].ToString(),
                                    IssueId = sqlDataReader["ServiceName"].ToString(),
                                    CellNumber = sqlDataReader["CellNumber"].ToString(),
                                    Email = sqlDataReader["Email"].ToString(),
                                    Description = sqlDataReader["Description"].ToString(),
                                    //status = Convert.ToBoolean(sqlDataReader["InstId"].ToString()),

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
            return null;
        }

        public List<IssueDto> GetIssuesList() 
        {
            List<IssueDto> Issuelist = new List<IssueDto>();
            try
            {
                string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
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
    }
}
public interface ITicketDataAccess
{
    SQLStatusDto TicketCreate(TicketDto ticketDto);
    List<TicketDto> TicketInfo(string search = "");

    TicketDto GetTicketDetails(int TicketId);

    List<IssueDto> GetIssuesList();
}
