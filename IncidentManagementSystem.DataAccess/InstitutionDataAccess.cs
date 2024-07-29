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

                        cmd.CommandText = @"InstitutionClientCreate";
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

        public List<InstNameDto> InstitutionList(string search)
        {
            List<InstNameDto> list = new List<InstNameDto>();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = @"InstList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.Add(new SqlParameter("@Search", search ?? ""));
                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                list.Add(new InstNameDto()
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
                Console.WriteLine(ex.Message);
            }
            return new List<InstNameDto>();
        }

        public List<InstNameDto> GetInstName()
        {
            List<InstNameDto> Instlist = new List<InstNameDto>();
            try
            {
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

                        cmd.CommandText = @"RoleList";
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
        List<InstNameDto> InstitutionList(string search);
        List<InstNameDto> GetInstName();
        List<Roles> RoleList();
    }
    
}
