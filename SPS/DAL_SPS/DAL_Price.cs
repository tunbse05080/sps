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
    public class DAL_Price : DBConnect
    {
        public DataTable getPrice()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from Price", conn);
            DataTable dtPrice = new DataTable();
            da.Fill(dtPrice);
            return dtPrice;
        }
        public DataTable getPrice(int parkingID, int vehicleType)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from Price where ParkingPlaceID = " + parkingID + " AND TypeOfvehicle = "+ vehicleType + " AND TimeOfApply < GETDATE()", conn);
            DataTable dtPrice = new DataTable();
            da.Fill(dtPrice);
            return dtPrice;
        }

    }
}
