using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IncidentManagementSystem.Model;
using System.Configuration;
using System.Net;

namespace IncidentManagementSystem.DataAccess
{
    public class InstNameDataAccess : IInstNameDataAccess
    {
        public SQLStatusDto InstNameRegister(InstNameDto _instNameDto)
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

                        cmd.CommandText = @"InstitutionClientCreate";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        cmd.Parameters.AddWithValue("@instId", _instNameDto.InstId);
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
                        cmd.Parameters.AddWithValue("@userId", _instNameDto.CreatedBy);

                        conn.Open();
                        //status = cmd.ExecuteScalar().ToString();
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
                return _SQLStatus;
            }
        }
    }
    public interface IInstNameDataAccess
    {
        SQLStatusDto InstNameRegister(InstNameDto _instNameDto);
    }
}
