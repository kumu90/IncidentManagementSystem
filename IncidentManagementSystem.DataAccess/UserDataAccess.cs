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
    public class UserDataAccess:IUserDataAccess
    {
        public List<UserDto> UserDetail(string search)
        {
            List<UserDto> list = new List<UserDto>();
            try
            {
                string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = @"InstList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.Add(new SqlParameter("@Search", search ?? ""));
                        //cmd.Parameters.AddWithValue("@search", search);
                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                list.Add(new UserDto()
                                {
                                    InstId = sqlDataReader["InstId"].ToString(),
                                    InstitutionName = sqlDataReader["InstitutionName"].ToString(),
                                    Country = sqlDataReader["Country"].ToString(),
                                    State = sqlDataReader["State"].ToString(),
                                    Address = sqlDataReader["Address"].ToString(),
                                    ZipCode = sqlDataReader["ZipCode"].ToString(),
                                    ContactNumber = sqlDataReader["ContactNumber"].ToString(),
                                    ContactPersonTechnical = sqlDataReader["ContactPersonTechnical"].ToString(),
                                    Email = sqlDataReader["ZipCode"].ToString(),
                                    CreatedBy = sqlDataReader["ContactNumber"].ToString(),
                                    Flag = sqlDataReader["Flag"].ToString(),
                                    ContactPersonAdmin = sqlDataReader["ContactPersonAdmin"].ToString(),
                                    CreatedDate = sqlDataReader["CreatedDate"].ToString()
                                });
                            }
                        }

                        conn.Close();

                    }
                }

                return list;

            }
            catch (Exception ex)
             {

            }
            return new List<UserDto>();
        }
    }

    public interface IUserDataAccess
    {
        List<UserDto> UserDetail(string search);
    }
}
