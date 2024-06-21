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
                /*ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;*/
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = @"InsertServiceRegistration";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                       
                        cmd.Parameters.AddWithValue("@service_name", service.ServiceName);
                        cmd.Parameters.AddWithValue("@InstId", service.Institution);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();


                    }
                }

                return _SQLStatus;
            }
            catch (Exception ex)
            {

            }
            return _SQLStatus;
        }
    }

    public interface IServiceInstutionDataAccess
    {
        SQLStatusDto AddService(ServiceDto service);
    }
}
