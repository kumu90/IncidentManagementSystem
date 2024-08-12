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
        private readonly string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public UserListDto UserDetail(string search, int page = 1, int offset = 10, string userId = "")
        {
            //List<UserInfo> list = new List<UserInfo>();
            //try
            //{
            //    using (SqlConnection conn = new SqlConnection(conStr))
            //    {
            //        using (SqlCommand cmd = new SqlCommand(conStr, conn))
            //        {

            //            cmd.CommandText = @"UserList";
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Connection = conn;
            //            cmd.Parameters.Add(new SqlParameter("@Search", search ?? ""));                        
            //            conn.Open();
            //            using (var sqlDataReader = cmd.ExecuteReader())
            //            {
            //                while (sqlDataReader.Read())
            //                {
            //                    list.Add(new UserInfo()
            //                    {
            //                        InstId = sqlDataReader["InstitutionName"].ToString(),
            //                        Username = sqlDataReader["Username"].ToString(),                                    
            //                        Email = sqlDataReader["Email"].ToString(),
            //                        Roles = sqlDataReader["Name"].ToString(),

            //                    });
            //                }
            //            }
            //            conn.Close();
            //        }
            //    }
            //    return list;

            //}
            //catch (Exception ex)
            // {
            //    Console.WriteLine(ex.Message);
            //}
            //return new List<UserInfo>();
            UserListDto data = new UserListDto();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "GetUserListDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Search", search ?? ""));
                    cmd.Parameters.Add(new SqlParameter("@page", page));
                    cmd.Parameters.Add(new SqlParameter("@offset", offset));
                    cmd.Connection = con;
                    DataSet ds = new DataSet();
                    SqlDataAdapter ads = new SqlDataAdapter(cmd);
                    con.Open();
                    ads.Fill(ds);
                    foreach (DataRow row1 in ds.Tables[0].Rows)
                    {
                        data.TotalCount = Convert.ToInt32(row1["TotalCount"]);
                    }
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        UserInfo dto = new UserInfo();
                        dto.InstId = row["InstitutionName"].ToString();
                        dto.Username = row["Username"].ToString();                        
                        dto.Email = row["Email"].ToString();
                        dto.Roles = row["RoleName"].ToString();

                        data.UserList.Add(dto);
                    }
                    return data;
                }
            }
        }
        public List<UserInfo> UserList(string userId)
        {
            List<UserInfo> Instlist = new List<UserInfo>();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = @"UserList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@userId", userId);

                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                Instlist.Add(new UserInfo()
                                {
                                    Id = sqlDataReader["Id"].ToString(),
                                    Username = sqlDataReader["UserName"].ToString()
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
                Console.WriteLine(ex.Message);
            }
            return new List<UserInfo>();
        }
    }

    public interface IUserDataAccess
    {
        UserListDto UserDetail(string search, int page = 1, int offset = 10, string userId = "");
        List<UserInfo> UserList(string userId);
    }
}
