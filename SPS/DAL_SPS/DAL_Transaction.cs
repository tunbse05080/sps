using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO_SPS;

namespace DAL_SPS
{
    public class DAL_Transaction : DBConnect
    {
        public DataTable getTransaction()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from [Transaction]", conn);
            DataTable dtTrans = new DataTable();
            da.Fill(dtTrans);
            return dtTrans;
        }

        public DataTable getTransactionOrdebyTimeIn(string license)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from [Transaction] where LicensePlates = '" + license + "' ORDER BY TimeIn DESC", conn);
            DataTable dtTrans = new DataTable();
            da.Fill(dtTrans);
            return dtTrans;
        }
        public DataTable getTransactionbyLicense(string license)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from [Transaction] where LicensePlates = '" + license + "'", conn);
            DataTable dtTrans = new DataTable();
            da.Fill(dtTrans);
            return dtTrans;
        }
    }
}
