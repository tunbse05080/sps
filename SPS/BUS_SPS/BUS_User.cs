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
            if (dalUser.getUserbyAccountID(accountID) != null && dalUser.getUserbyAccountID(accountID).Rows.Count > 0)
            {
                return Convert.ToInt32(dalUser.getUserbyAccountID(accountID).Rows[0][0].ToString());
            }
            return 0;
        }
        public string getNamebyUserID(int userID)
        {
            if (dalUser.getUserbyUserID(userID) != null && dalUser.getUserbyUserID(userID).Rows.Count > 0)
            {
                return dalUser.getUserbyUserID(userID).Rows[0][1].ToString();
            }
            return "";
        }
        public string getNamebyAccountID(int accountID)
        {
            if (dalUser.getUserbyAccountID(accountID) != null && dalUser.getUserbyAccountID(accountID).Rows.Count > 0)
            {
                return dalUser.getUserbyAccountID(accountID).Rows[0][1].ToString();
            }
            return "";
        }
        public bool checkUser(int accountID,int parkingID) //kiem tra user co dung bai do xe khong
        {
            if (dalUser.getUserbyParkingID(parkingID) != null && dalUser.getUserbyParkingID(parkingID).Rows.Count > 0)
            {
                return dalUser.getUserbyParkingID(parkingID).AsEnumerable().Any(row => accountID == row.Field<int>("AccountID"));
            }
            return false;
        }
        public bool updateUser(DTO_User user)
        {
            return dalUser.updateUser(user);
        }
    }
}
