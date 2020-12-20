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
            if (dalParkingPlace.getParkingPlacebyID(parkingID) != null && dalParkingPlace.getParkingPlacebyID(parkingID).Rows.Count > 0)
            {
                return Convert.ToInt32(dalParkingPlace.getParkingPlacebyID(parkingID).Rows[0][7].ToString());
            }
            return 0;  
        }
        public int getCarFree(int ParkingID)
        {
            if (dalParkingPlace.getParkingPlacebyID(ParkingID) != null && dalParkingPlace.getParkingPlacebyID(ParkingID).Rows.Count > 0)
            {
                return Convert.ToInt32(dalParkingPlace.getParkingPlacebyID(ParkingID).Rows[0][5].ToString());
            }
            return 0; 
        }
        public int getMotorFree(int ParkingID)
        {
            if (dalParkingPlace.getParkingPlacebyID(ParkingID) != null && dalParkingPlace.getParkingPlacebyID(ParkingID).Rows.Count > 0)
            {
                return Convert.ToInt32(dalParkingPlace.getParkingPlacebyID(ParkingID).Rows[0][6].ToString());
            }
            return 0; 
        }
        public string getParkingName(int parkingID)
        {
            if (dalParkingPlace.getParkingPlacebyID(parkingID) != null && dalParkingPlace.getParkingPlacebyID(parkingID).Rows.Count > 0)
            {
                return dalParkingPlace.getParkingPlacebyID(parkingID).Rows[0][1].ToString();
            }
            return "";  
        }

        public bool checkParking(int parkingID) //kiem tra parkingID co status = 1 co ton tai khong
        {
            if (dalParkingPlace.getParkingPlace() != null && dalParkingPlace.getParkingPlace().Rows.Count > 0)
            {
                return dalParkingPlace.getParkingPlace().AsEnumerable().Any(row => parkingID == row.Field<Int32>("ParkingPlaceID"));
            }
            return false;
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
