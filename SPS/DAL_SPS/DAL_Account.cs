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
    public class DAL_Account: DBConnect
    {
        public DataTable getAccount()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from [Account] where RoleID = 2", conn);
            DataTable dtAccount = new DataTable();
            da.Fill(dtAccount);
            return dtAccount;
        }
        public DataTable getAccountbyUsername(string username)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from [Account] where RoleID = 2 and UserName = '" + username + "'", conn);
            DataTable dtAccount = new DataTable();
            da.Fill(dtAccount);
            return dtAccount;
        }
        public DataTable GetAccountbyAccountID(int accountID)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from [Account] where RoleID = 2 and AccountID = " + accountID + "", conn);
            DataTable dtAccount = new DataTable();
            da.Fill(dtAccount);
            return dtAccount;
        }
    }
}
