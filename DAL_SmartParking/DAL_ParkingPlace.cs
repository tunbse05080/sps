using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO__SmartParking;

namespace DAL_SmartParking
{
    public class DAL_ParkingPlace : DBConnect
    {
        public DataTable getParkingPlace()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from ParkingPlace",conn);
            DataTable dtParkingPlace = new DataTable();
            da.Fill(dtParkingPlace);
            return dtParkingPlace;
        }
    }
}
