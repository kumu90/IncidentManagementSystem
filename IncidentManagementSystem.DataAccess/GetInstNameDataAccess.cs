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
    public class GetInstNameDataAccess: IGetInstNameDataAccess
    {
        public List<InstNameDto> GetInstName()
        {

            List<InstNameDto> Instlist = new List<InstNameDto>();
            try
            {
                string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = @"InstName";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                Instlist.Add(new InstNameDto()
                                {
                                    Id = sqlDataReader["Id"].ToString(),
                                    InstitutionName = sqlDataReader["InstitutionName"].ToString()
                                });
                            }
                        }

                        conn.Close();

                    }
                }

                return Instlist;

            }
            catch (Exception ex)
            {
                return new List<InstNameDto>();
            }
        }
    }
    public interface IGetInstNameDataAccess
    {
        List<InstNameDto> GetInstName(InstNameDto _instNameDto);
    }
}
