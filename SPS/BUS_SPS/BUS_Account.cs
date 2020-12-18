using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_SPS;
using DTO_SPS;

namespace BUS_SPS
{
    public class BUS_Account
    {
        DAL_Account dalAccount = new DAL_Account();
        public DataTable GetData()
        {
            return dalAccount.getAccount();
        }
        public int getAccountID(string username) //lay accoutID tu username
        {
            return Convert.ToInt32(dalAccount.getAccountbyUsername(username).Rows[0][0].ToString());
        }
        public string getPassword(string username)
        {
            return dalAccount.getAccountbyUsername(username).Rows[0][2].ToString();
        }
        public bool checkAccount(string username) //kiem tra account co ton tai khong, co bi khoa khong va co phai bao ve khong
        {
            return dalAccount.getAccount().AsEnumerable().Any(row => username == row.Field<String>("UserName"));
        }
    }
}
