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
            SqlDataAdapter da = new SqlDataAdapter("select * from ParkingPlace", conn);
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


    }
}
