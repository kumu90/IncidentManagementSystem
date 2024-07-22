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
    public class ProductDataAccess : IProductDataAccess
    {
        private readonly string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString; 
        public SQLStatusDto ServiceCreate(ServiceDto service)
        {
            SQLStatusDto _SQLStatus = new SQLStatusDto();
            try
            {               
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
                            rdr.Read();
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


        public List<ServiceDto> GetServices(string InstId = "")
        {
            List<ServiceDto> Servicelist = new List<ServiceDto>();
            try
            {             
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
                Console.WriteLine(ex.Message);
            }
            return new List<ServiceDto>();
        }

    }


}

public interface IProductDataAccess
{
    SQLStatusDto ServiceCreate(ServiceDto service);

 

    List<ServiceDto> GetServices(string InstId = "");


}

