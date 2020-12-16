using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL_SPS;
using System.Data;
using DTO_SPS;

namespace BUS_SPS
{
    public class BUS_ParkingPlace
    {
        DAL_ParkingPlace dalParkingPlace = new DAL_ParkingPlace();
        public DataTable getParkingPlace()
        {
            return dalParkingPlace.getParkingPlace();
        }
        public int getParkingStatus(int parkingID)
        {
            return Convert.ToInt32(dalParkingPlace.getParkingPlacebyID(parkingID).Rows[0][7].ToString());
        }
        public int getCarFree(int ParkingID)
        {
            return Convert.ToInt32(dalParkingPlace.getParkingPlacebyID(ParkingID).Rows[0][5].ToString());
        }
        public int getMotorFree(int ParkingID)
        {
            return Convert.ToInt32(dalParkingPlace.getParkingPlacebyID(ParkingID).Rows[0][6].ToString());
        }
        public bool checkParking(int parkingID) //kiem tra parkingID co status = 1 co ton tai khong
        {
            return dalParkingPlace.getParkingPlace().AsEnumerable().Any(row => parkingID == row.Field<Int32>("ParkingPlaceID"));
        }
        public bool updateCarParking(DTO_ParkingPlace parking)
        {
            return dalParkingPlace.updateCarParking(parking);
        }
        public bool updateMotoParking(DTO_ParkingPlace parking)
        {
            return dalParkingPlace.updateMotoParking(parking);
        }
    }
}
