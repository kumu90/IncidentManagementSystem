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
    public class ServiceInstutionDataAccess: IServiceInstutionDataAccess
    {
        public SQLStatusDto AddService(ServiceDto service)
        {
            SQLStatusDto _SQLStatus = new SQLStatusDto();
            try
            {
                string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;                
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = @"InstitutionServiceRegister";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@ServiceId", service.ServiceId);
                        cmd.Parameters.AddWithValue("@InstId", service.InstId);

                        conn.Open();
                        //cmd.ExecuteNonQuery();
                        //conn.Close();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                _SQLStatus.Status = rdr["Status"].ToString();
                                _SQLStatus.Message = rdr["Message"].ToString();
                            }
                        }

                    }
                }

                return _SQLStatus;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return _SQLStatus;
        }

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


                        cmd.Parameters.AddWithValue("@ServiceId", _ticketDto.ServiceId);
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


        public List<ServiceDto> GetServices(string InstId="")
        {
            List<ServiceDto> Servicelist = new List<ServiceDto>();
            try
            {
                string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = "GetService";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@instId", InstId ?? "");
                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                Servicelist.Add(new ServiceDto()
                                {
                                    ServiceId = Convert.ToInt32(sqlDataReader["ServiceId"].ToString()),
                                    ServiceName = sqlDataReader["ServiceName"].ToString(),
                                });
                            }
                        }

                        conn.Close();

                    }
                }

                return Servicelist;

            }
            catch (Exception ex)
            {

            }
            return new List<ServiceDto>();
        }

        public List<TicketDto> ticketInfo(string search = "")
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
                                    Description = sqlDataReader["Description"].ToString(),
                                    InstId = sqlDataReader["InstId"].ToString(),
                                    ServiceId = sqlDataReader["ServiceId"].ToString(),
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

            }
            return new List<TicketDto>();
        }
    }

    public interface IServiceInstutionDataAccess
    {
        SQLStatusDto AddService(ServiceDto service);

        SQLStatusDto TicketCreate(TicketDto _ticketDto);

        List<ServiceDto> GetServices(string InstId = "");

        List<TicketDto> ticketInfo(string search = "");
    }
}
