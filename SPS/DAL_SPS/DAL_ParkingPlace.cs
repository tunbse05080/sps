using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DTO_SPS;

namespace DAL_SPS
{
    public class DAL_ParkingPlace : DBConnect
    {
        public DataTable getParkingPlace()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from ParkingPlace where StatusOfParkingPlace = 1", conn);
            DataTable dtParkingPlace = new DataTable();
            da.Fill(dtParkingPlace);
            return dtParkingPlace;
        }
        public DataTable getParkingPlacebyID(int ParkingID)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from ParkingPlace where ParkingPlaceID = " + ParkingID + "", conn);
            DataTable dtParkingPlace = new DataTable();
            da.Fill(dtParkingPlace);
            return dtParkingPlace;
        }
        public bool updateCarParking(DTO_ParkingPlace parking) //update so cho o to
        {
            try
            {
                // Ket noi
                conn.Open();
                // Query string
                string SQL = string.Format("UPDATE ParkingPlace SET NumberCarBlank = '{0}' WHERE ParkingPlaceID = {1}", parking.MyNumberCarFree, parking.MyPlaceID);
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
        public bool updateMotoParking(DTO_ParkingPlace parking) //update so cho  xe may
        {
            try
            {
                // Ket noi
                conn.Open();
                // Query string
                string SQL = string.Format("UPDATE ParkingPlace SET NumberMotoBikeBlank = '{0}' WHERE ParkingPlaceID = {1}", parking.MyNumberMotorFree, parking.MyPlaceID);
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
