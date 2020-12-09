using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DTO_SPS;

namespace DAL_SPS
{
    public class DAL_MonthlyTicket : DBConnect
    {
        public DataTable getMonthlyTicket()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from MonthlyTicket", conn);
            DataTable dtTicket = new DataTable();
            da.Fill(dtTicket);
            return dtTicket;
        }

        public DataTable getMonthlyTicketbyLicense(string license)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from MonthlyTicket where LicensePlates = '" + license + "'", conn);
            DataTable dtTicket = new DataTable();
            da.Fill(dtTicket);
            return dtTicket;
        }

    }
}
