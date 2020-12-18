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
    public class DAL_User : DBConnect
    {
        public DataTable getUser()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from [User]", conn);
            DataTable dtUser = new DataTable();
            da.Fill(dtUser);
            return dtUser;
        }

        public DataTable getUserbyParkingID(int parkingID) //status 0-dang lam viec, 1-khong lam viec
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from [User] where StatusOfWork = 1 and ParkingPlaceID = "+ parkingID + "", conn);
            DataTable dtUser = new DataTable();
            da.Fill(dtUser);
            return dtUser;
        } 
        public DataTable getUserbyUserID(int userID)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from [User] where UserID = " + userID + "", conn);
            DataTable dtUser = new DataTable();
            da.Fill(dtUser);
            return dtUser;
        }
        public DataTable getUserbyAccountID(int accountID)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from [User] where AccountID = " + accountID + "", conn);
            DataTable dtUser = new DataTable();
            da.Fill(dtUser);
            return dtUser;
        }
        public bool updateUser(DTO_User user) //update bang Card
        {
            try
            {
                // Ket noi
                conn.Open();
                // Query string
                string SQL = string.Format("UPDATE [User] SET StatusOfwork = '{0}' WHERE AccountID = {1}", user.MyStatus, user.MyAccountID);
                SqlCommand cmd = new SqlCommand(SQL, conn);
                // Query và kiểm tra
                if (cmd.ExecuteNonQuery() > 0)
                    return true;
            }
            catch (Exception e)
            {
            }
            finally
            {
                // Dong ket noi
                conn.Close();
            }
            return false;
        }
    }
}
