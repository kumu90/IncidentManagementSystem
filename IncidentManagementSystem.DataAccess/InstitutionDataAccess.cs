using IncidentManagementSystem.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mime;

namespace IncidentManagementSystem.DataAccess
{
    public class InstitutionDataAccess : IInstitutionDataAccess
    {
        private readonly string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public SQLStatusDto InstitutionCreate(InstNameDto _instNameDto)
        {
            var serviceList = string.Join(",", _instNameDto.ServiceIdList);
            SQLStatusDto _SQLStatus = new SQLStatusDto();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = "InstitutionClientCreate";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@institutionName", _instNameDto.InstitutionName);
                        cmd.Parameters.AddWithValue("@country", _instNameDto.Country);
                        cmd.Parameters.AddWithValue("@state", _instNameDto.State);
                        cmd.Parameters.AddWithValue("@address", _instNameDto.Address);
                        cmd.Parameters.AddWithValue("@zipCode", _instNameDto.ZipCode);
                        cmd.Parameters.AddWithValue("@contactPersonAdmin", _instNameDto.ContactPersonAdmin);
                        cmd.Parameters.AddWithValue("@contactPersonTechnical", _instNameDto.ContactPersonTechnical);
                        cmd.Parameters.AddWithValue("@contactNumber", _instNameDto.ContactNumber);
                        cmd.Parameters.AddWithValue("@email", _instNameDto.Email);
                        cmd.Parameters.AddWithValue("@imageUrl", _instNameDto.ImageUrl);
                        cmd.Parameters.AddWithValue("@userId", _instNameDto.CreatedBy ?? "");
                        cmd.Parameters.AddWithValue("@serviceIds", serviceList ?? "");
                        cmd.Parameters.AddWithValue("@ImageData", _instNameDto.ImageData);
                        cmd.Parameters.AddWithValue("@ContentType", _instNameDto.contentType);



                        conn.Open();
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

        public InstListDto InstitutionList(string search, int page = 1, int offset = 10)
        {
            //List<InstNameDto> list = new List<InstNameDto>();
            //try
            //{
            //    using (SqlConnection conn = new SqlConnection(conStr))
            //    {
            //        using (SqlCommand cmd = new SqlCommand(conStr, conn))
            //        {

            //            cmd.CommandText = @"InstList";
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Connection = conn;
            //            cmd.Parameters.Add(new SqlParameter("@Search", search ?? ""));
            //            conn.Open();

            //            using (var sqlDataReader = cmd.ExecuteReader())
            //            {
            //                while (sqlDataReader.Read())
            //                {
            //                    list.Add(new InstNameDto()
            //                    {
            //                        InstId = sqlDataReader["InstId"].ToString(),
            //                        InstitutionName = sqlDataReader["InstitutionName"].ToString(),
            //                        Country = sqlDataReader["Country"].ToString(),
            //                        State = sqlDataReader["State"].ToString(),
            //                        Address = sqlDataReader["Address"].ToString(),
            //                        ZipCode = sqlDataReader["ZipCode"].ToString(),
            //                        ContactNumber = sqlDataReader["ContactNumber"].ToString(),
            //                        ContactPersonTechnical = sqlDataReader["ContactPersonTechnical"].ToString(),
            //                        Email = sqlDataReader["ZipCode"].ToString(),
            //                        CreatedBy = sqlDataReader["ContactNumber"].ToString(),
            //                        Flag = sqlDataReader["Flag"].ToString(),
            //                        ContactPersonAdmin = sqlDataReader["ContactPersonAdmin"].ToString(),
            //                        CreatedDate = sqlDataReader["CreatedDate"].ToString()
            //                    });
            //                }
            //            }

            //            conn.Close();

            //        }
            //    }

            //    return list;

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //return new List<InstNameDto>();

            InstListDto data = new InstListDto();
            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "GetInstitutionListDetails";
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
                            InstNameDto dto = new InstNameDto();
                            dto.InstId = row["InstId"].ToString();
                            dto.InstitutionName = row["InstitutionName"].ToString();
                            dto.Country = row["Country"].ToString();
                            dto.State = row["State"].ToString();
                            dto.Address = row["Address"].ToString();
                            dto.ZipCode = row["ZipCode"].ToString();
                            dto.ContactNumber = row["ContactNumber"].ToString();
                            dto.ContactPersonTechnical = row["ContactPersonTechnical"].ToString();
                            dto.Email = row["Email"].ToString();
                            //dto.CreatedBy = row["CreatedBy"].ToString();
                            dto.Flag = row["Flag"].ToString();
                            dto.ContactPersonAdmin = row["ContactPersonAdmin"].ToString();
                            dto.CreatedDate = row["CreatedDate"].ToString();
                            
                            data.InstList.Add(dto);
                        }
                        return data;
                    }
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
            return new InstListDto();
        }

        public List<InstNameDto> GetInstName(string userId)
        {
            List<InstNameDto> Instlist = new List<InstNameDto>();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = "GetInstNameDropDown";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@userId", userId);

                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                Instlist.Add(new InstNameDto()
                                {
                                    InstId = sqlDataReader["InstId"].ToString(),
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
                Console.WriteLine(ex.Message);
            }
            return new List<InstNameDto>();
        }


        public List<Roles> RoleList()
        {
            List<Roles> Roleslist = new List<Roles>();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = "GetRoleListDropDown";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                Roleslist.Add(new Roles()
                                {
                                    Id = sqlDataReader["Id"].ToString(),
                                    Name = sqlDataReader["Name"].ToString()
                                });
                            }
                        }

                        conn.Close();

                    }
                }

                return Roleslist;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<Roles>();
        }

    }
    public interface IInstitutionDataAccess
    {
        SQLStatusDto InstitutionCreate(InstNameDto _instNameDto);
        InstListDto InstitutionList(string search, int page = 1, int offset = 10);
        List<InstNameDto> GetInstName(string userId);
        List<Roles> RoleList();
    }
    
}
