using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO__SmartParking;
using DAL_SmartParking;
using System.Data;

namespace BUS_SmartParking
{
    public class BUS_ParkingPlace
    {
        DAL_ParkingPlace dalParkingPlace = new DAL_ParkingPlace();
        public DataTable getParkingPlace()
        {
            return dalParkingPlace.getParkingPlace();
        }
    }
}
