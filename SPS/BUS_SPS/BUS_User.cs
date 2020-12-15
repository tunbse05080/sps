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
    public class BUS_User
    {
        DAL_User dalUser = new DAL_User();
        public DataTable GetData()
        {
            return dalUser.getUser();
        }
        public int getUserID(string username)
        {
            return Convert.ToInt32(dalUser.getUserbyUsername(username).Rows[0][0].ToString());
        }
        public string getPassword(string username)
        {
            return dalUser.getUserbyUsername(username).Rows[0][3].ToString();
        }
        public bool checkUser(string username,int parkingID) //kiem tra user co duoc phep dang nhap khong
        {
            return dalUser.getUserbyParkingID(parkingID).AsEnumerable().Any(row => username == row.Field<String>("UserName"));
        }
    }
}
