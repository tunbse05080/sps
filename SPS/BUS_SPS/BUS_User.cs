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
        public int getUserID(int accountID)
        {
            return Convert.ToInt32(dalUser.getUserbyAccountID(accountID).Rows[0][0].ToString());
        }
        public string getNamebyUserID(int userID)
        {
            return dalUser.getUserbyUserID(userID).Rows[0][1].ToString();
        }
        public string getNamebyAccountID(int accountID)
        {
            return dalUser.getUserbyAccountID(accountID).Rows[0][1].ToString();
        }
        public bool checkUser(int accountID,int parkingID) //kiem tra user co dung bai do xe khong
        {
            return dalUser.getUserbyParkingID(parkingID).AsEnumerable().Any(row => accountID == row.Field<int>("AccountID"));
        }
        public bool updateUser(DTO_User user)
        {
            return dalUser.updateUser(user);
        }
    }
}
