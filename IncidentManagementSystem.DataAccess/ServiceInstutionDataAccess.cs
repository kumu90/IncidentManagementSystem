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
                        cmd.Parameters.AddWithValue("@Service", service.ServiceName);
                        cmd.Parameters.AddWithValue("@InstId", service.Institution);

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


                        cmd.Parameters.AddWithValue("@Service", _ticketDto.InstId);
                        cmd.Parameters.AddWithValue("@InstId", _ticketDto.ServiceId);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();


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
    }

    public interface IServiceInstutionDataAccess
    {
        SQLStatusDto AddService(ServiceDto service);

        SQLStatusDto TicketCreate(TicketDto _ticketDto);
    }
}
