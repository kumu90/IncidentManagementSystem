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

    public class TicketDataAccess : ITicketDataAccess
    {
        private string conStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public SQLStatusDto TicketCreate(TicketDto _ticketDto)
        {
            SQLStatusDto _sQLStatus = new SQLStatusDto();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = @"InstitutionTicketCreate";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@ServiceId", _ticketDto.ServiceId);
                        cmd.Parameters.AddWithValue("@IssueId", _ticketDto.IssueId);
                        cmd.Parameters.AddWithValue("@InstId", _ticketDto.InstId);
                        cmd.Parameters.AddWithValue("@Description", _ticketDto.Description);
                        cmd.Parameters.AddWithValue("@CellNumber", _ticketDto.CellNumber);
                        cmd.Parameters.AddWithValue("@Email", _ticketDto.Email);
                        cmd.Parameters.AddWithValue("@ImageUrl", _ticketDto.ImageUrl ?? "");
                        cmd.Parameters.AddWithValue("@ImageData", _ticketDto.ImageData);
                        cmd.Parameters.AddWithValue("@ContentType", _ticketDto.contentType);

                        conn.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            {
                                _sQLStatus.Status = rdr["Status"].ToString();
                                _sQLStatus.Message = rdr["Message"].ToString();
                            }
                        }
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
        public List<TicketDto> TicketInfo(string search = "",string InstId="",string status="", int page = 1, int offset = 10,string userId = "")
        {
            //List<TicketDto> Ticketlist = new List<TicketDto>();
            //try
            //{
            //    using (SqlConnection conn = new SqlConnection(conStr))
            //    {
            //        using (SqlCommand cmd = new SqlCommand(conStr, conn))
            //        {

            //            cmd.CommandText = @"GetTicketListDetails";
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Parameters.Add(new SqlParameter("@Search", search ?? ""));
            //            cmd.Parameters.Add(new SqlParameter("@instId", InstId ?? ""));
            //            cmd.Parameters.Add(new SqlParameter("@status", status ?? ""));
            //            cmd.Parameters.Add(new SqlParameter("@page", page));
            //            cmd.Parameters.Add(new SqlParameter("@offset", offset));
            //            cmd.Connection = conn;

            //            conn.Open();

            //            using (var sqlDataReader = cmd.ExecuteReader())
            //            {
            //                while (sqlDataReader.Read())
            //                {
            //                    Ticketlist.Add(new TicketDto()
            //                    {
            //                        TicketId = sqlDataReader["TicketId"].ToString(),
            //                        //Description = sqlDataReader["Description"].ToString(),
            //                        status = sqlDataReader["Status"].ToString(),
            //                        InstId = sqlDataReader["InstitutionName"].ToString(),
            //                        ServiceId = sqlDataReader["ServiceName"].ToString(),
            //                        IssueId = sqlDataReader["Issue"].ToString(),
            //                        CellNumber = sqlDataReader["CellNumber"].ToString(),
            //                        Email = sqlDataReader["Email"].ToString()

            //                    });
            //                }
            //            }

            //            conn.Close();

            //        }
            //    }

            //    return Ticketlist;

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            //return new List<TicketDto>();
            List<TicketDto> data = new List<TicketDto>();
            using (SqlConnection con = new SqlConnection(conStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = @"GetTicketListDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Search", search ?? ""));
                    cmd.Parameters.Add(new SqlParameter("@instId", InstId ?? ""));
                    cmd.Parameters.Add(new SqlParameter("@status", status ?? ""));
                    cmd.Parameters.Add(new SqlParameter("@userId", ""));
                    cmd.Parameters.Add(new SqlParameter("@page", page));
                    cmd.Parameters.Add(new SqlParameter("@offset", offset));
                    

                    cmd.Connection = con;
                    DataSet ds = new DataSet();
                    SqlDataAdapter ads = new SqlDataAdapter(cmd);
                    con.Open();
                    ads.Fill(ds);
                    foreach (DataRow row in ds.Tables[1].Rows)
                    {
                        TicketDto dto = new TicketDto();
                        dto.TicketId = row["TicketId"].ToString();
                        dto.status = row["Status"].ToString();
                        dto.InstId = row["InstitutionName"].ToString();
                        dto.ServiceId = row["ServiceName"].ToString();
                        dto.IssueId = row["Issue"].ToString();
                        dto.CellNumber =row["CellNumber"].ToString();
                        dto.Email =row["Email"].ToString();                       
                        foreach (DataRow row1 in ds.Tables[0].Rows)
                        {
                            dto.TotalCount = Convert.ToInt32(row1["TotalCount"]);
                        }
                        data.Add(dto);
                    }
                    return data;
                }
            }
        }

        public TicketDto GetTicketDetails(string TicketId)
        {
            TicketDto Ticketlist = new TicketDto();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = "GetTicketDetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", TicketId));
                        cmd.Connection = conn;

                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            if (sqlDataReader.Read())
                            {
                                Ticketlist = new TicketDto()
                                {
                                    TicketId = sqlDataReader["TicketId"].ToString(),
                                    date = Convert.ToDateTime(sqlDataReader["Date"].ToString()),
                                    status = sqlDataReader["status"].ToString(),
                                    InstId = sqlDataReader["InstitutionName"].ToString(),
                                    ServiceId = sqlDataReader["ServiceName"].ToString(),
                                    IssueId = sqlDataReader["Issue"].ToString(),
                                    CellNumber = sqlDataReader["CellNumber"].ToString(),
                                    Email = sqlDataReader["Email"].ToString(),
                                    Description = sqlDataReader["Description"].ToString(),
                                    ImageUrl = sqlDataReader["ImageData"].ToString(),
                                    ImageData = (byte[])sqlDataReader["ImageData"],
                                    contentType = sqlDataReader["ContentType"].ToString()


                                };

                                // Convert byte[] ImageData to base64 string for ImageUrl
                                //if (ticketDetail.ImageData != null && ticketDetail.ImageData.Length > 0)
                                //{
                                //    string base64String = Convert.ToBase64String(ticketDetail.ImageData);
                                //    ticketDetail.ImageUrl = $"data:{ticketDetail.ContentType};base64,{base64String}";
                                //}



                            }
                        }
                    }

                    conn.Close();

                }
                return Ticketlist;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ticketlist;
        }

        public List<IssueDto> GetIssuesList(string ServiceId = "")
        {
            List<IssueDto> Issuelist = new List<IssueDto>();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = "IssueList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ServiceId", ServiceId ?? "");
                        cmd.Connection = conn;

                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            while(sqlDataReader.Read())
                            {
                                Issuelist.Add(new IssueDto()
                                {
                                    IssueId = Convert.ToInt32(sqlDataReader["IssueId"].ToString()),
                                    IssueName = sqlDataReader["Issue"].ToString()
                                });
                            }
                        }

                        conn.Close();

                    }
                }

                return Issuelist;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new List<IssueDto>();
        }

        public TicketAssignDto TicketAssign(string TicketId = "")
        {
            TicketAssignDto ticketAssignDto = new TicketAssignDto();
            try
         {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = "TicketAssign";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@TicketId", TicketId));
                        cmd.Connection = conn;

                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            if (sqlDataReader.Read())
                            {
                                ticketAssignDto = new TicketAssignDto()
                                {
                                    TicketId = sqlDataReader["TicketId"].ToString(),
                                    Date = Convert.ToDateTime(sqlDataReader["Date"].ToString()),
                                    ServiceId = sqlDataReader["ServiceName"].ToString(),
                                    IssueId = sqlDataReader["Issue"].ToString()


                                };


                            }
                        }
                    }

                }
                return ticketAssignDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ticketAssignDto;
        }


        public TicketDto GetInstDetail(string UserName = "")
        {
            TicketDto instNameDto = new TicketDto();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = "GetInstutionTicketCreate";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", UserName));
                        cmd.Connection = conn;

                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            if (sqlDataReader.Read())
                            {
                                instNameDto = new TicketDto()
                                {
                                    InstId = sqlDataReader["InstId"].ToString(),
                                    InstitutionName = sqlDataReader["InstitutionName"].ToString(),
                                    Email = sqlDataReader["Email"].ToString(),
                                    CellNumber = sqlDataReader["ContactNumber"].ToString()
                                };


                            }
                        }
                    }

                }
                return instNameDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return instNameDto;
        }

        //public TicketDto GetInstDetailSearch(string UserName = "")
        //{
        //    TicketDto instNameDto = new TicketDto();
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(conStr))
        //        {
        //            using (SqlCommand cmd = new SqlCommand(conStr, conn))
        //            {

        //                cmd.CommandText = "GetInstutionTicketSearch";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("@Id", UserName));
        //                cmd.Connection = conn;

        //                conn.Open();

        //                using (var sqlDataReader = cmd.ExecuteReader())
        //                {
        //                    if (sqlDataReader.Read())
        //                    {
        //                        instNameDto = new TicketDto()
        //                        {
        //                            InstId = sqlDataReader["InstId"].ToString(),
        //                            InstitutionName = sqlDataReader["InstitutionName"].ToString()
        //                        };


        //                    }
        //                }
        //            }

        //        }
        //        return instNameDto;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return instNameDto;
        //}
        public SQLStatusDto TicketAssignTo(TicketAssignDto AssignDto)
        {
            SQLStatusDto _sQLStatus = new SQLStatusDto();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = "InstitutionTicketAssignUpdate";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@TicketId", AssignDto.TicketId);
                        cmd.Parameters.AddWithValue("@AssignTo", AssignDto.AssignTo);


                        conn.Open();                        
                        using (var rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            {
                                _sQLStatus.Status = rdr["Status"].ToString();
                                _sQLStatus.Message = rdr["Message"].ToString();
                            }
                        }
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

        public SQLStatusDto TicketReject(string TicketId)
        {
            SQLStatusDto _sQLStatus = new SQLStatusDto();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {

                        cmd.CommandText = "InstitutionTicketRejectUpdate";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@TicketId",TicketId);
                       
                        conn.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            {
                                _sQLStatus.Status = rdr["Status"].ToString();
                                _sQLStatus.Message = rdr["Message"].ToString();
                            }
                        }
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

        public ResolvedByDto GetResolveDetails(string TicketId)
        {
            ResolvedByDto resolvedBylist = new ResolvedByDto();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {
                        cmd.CommandText = "GetInstiturionTicketResolve";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Id", TicketId));
                        cmd.Connection = conn;

                        conn.Open();

                        using (var sqlDataReader = cmd.ExecuteReader())
                        {
                            if (sqlDataReader.Read())
                            {
                                resolvedBylist = new ResolvedByDto()
                                {
                                    TranDateTime = Convert.ToDateTime(sqlDataReader["Date"].ToString()),
                                    Username = sqlDataReader["UserName"].ToString(),
                                    TicketId = sqlDataReader["TicketId"].ToString(),                                   
                                    InstId = sqlDataReader["InstitutionName"].ToString(),                                    
                                    IssueId = sqlDataReader["Issue"].ToString()                                   
                                };

                            }
                        }
                    }

                    conn.Close();

                }
                return resolvedBylist;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return resolvedBylist;

        }

        public SQLStatusDto TicketResolveBy(ResolvedByDto resolvedByDto)
        {
            SQLStatusDto _sQLStatus = new SQLStatusDto();
            try
            {
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    using (SqlCommand cmd = new SqlCommand(conStr, conn))
                    {
                        cmd.CommandText = "InstitutionTicketResolve";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Parameters.AddWithValue("@UserId", resolvedByDto.Username);
                        cmd.Parameters.AddWithValue("@TicketId", resolvedByDto.TicketId);
                        cmd.Parameters.AddWithValue("@InstId", resolvedByDto.InstId);
                        cmd.Parameters.AddWithValue("@IssueId", resolvedByDto.IssueId);
                        cmd.Parameters.AddWithValue("@TranDateTime", resolvedByDto.TranDateTime);
                        cmd.Parameters.AddWithValue("@ResolveId", resolvedByDto.Resolve);
                        cmd.Parameters.AddWithValue("@Description", resolvedByDto.Description);
                        conn.Open();
                        using (var rdr = cmd.ExecuteReader())
                        {
                            rdr.Read();
                            {
                                _sQLStatus.Status = rdr["Status"].ToString();
                                _sQLStatus.Message = rdr["Message"].ToString();
                            }
                        }
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
}
public interface ITicketDataAccess
{
    SQLStatusDto TicketCreate(TicketDto ticketDto);
    List<TicketDto> TicketInfo(string search = "", string InstId = "", string status="", int page = 1, int offset = 10, string userId = "");

    TicketDto GetTicketDetails(string TicketId);

    List<IssueDto> GetIssuesList(string ServiceId = "");
    TicketAssignDto TicketAssign(string TicketId = "");

    TicketDto GetInstDetail(string UserName = "");
    //TicketDto GetInstDetailSearch(string UserName = "");

    SQLStatusDto TicketAssignTo(TicketAssignDto AssignDto);

    SQLStatusDto TicketReject(string TicketId);

    ResolvedByDto GetResolveDetails(string TicketId);

    SQLStatusDto TicketResolveBy(ResolvedByDto resolvedByDto);
}
