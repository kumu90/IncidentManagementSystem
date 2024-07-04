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
        public List<UserInfo> UserDetail(string search)
        {
            List<UserInfo> list = new List<UserInfo>();
            try
            {
                string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = @"UserList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.Add(new SqlParameter("@Search", search ?? ""));
                        //cmd.Parameters.AddWithValue("@search", search);
                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                list.Add(new UserInfo()
                                {
                                    InstId = sqlDataReader["InstId"].ToString(),
                                    Username = sqlDataReader["Username"].ToString(),                                    
                                    Email = sqlDataReader["Email"].ToString(),
                                    Roles = sqlDataReader["UserRoleId"].ToString(),
                                    
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
                Console.WriteLine(ex.Message);
            }
            return new List<UserInfo>();
        }
    }

    public interface IUserDataAccess
    {
        List<UserInfo> UserDetail(string search);
    }
}
