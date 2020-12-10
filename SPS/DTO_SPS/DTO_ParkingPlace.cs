using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTO_SPS
{
    public class DTO_ParkingPlace
    {
        private int PlaceID;
        private string PlaceName;
        private string PlaceLocation;       
        private int NumberOfCar;
        private int NumberOfMotor;
        private int NumberCarFree;
        private int NumberMotorFree;
        private int Status;

        /* ======== GETTER/SETTER ======== */
        public int MyNumberMotorFree
        {
            get { return NumberMotorFree; }
            set { NumberMotorFree = value; }
        }


        public int MyNumberCarFree
        {
            get { return NumberCarFree; }
            set { NumberCarFree = value; }
        }


        public int MyNumberOfMotor
        {
            get { return NumberOfMotor; }
            set { NumberOfMotor = value; }
        }


        public int MyNumberOfCar
        {
            get { return NumberOfCar; }
            set { NumberOfCar = value; }
        }


        public int MyStatus
        {
            get { return Status; }
            set { Status = value; }
        }


        public string MyPlaceLocation
        {
            get { return PlaceLocation; }
            set { PlaceLocation = value; }
        }


        public string MyPlaceName
        {
            get { return PlaceName; }
            set { PlaceName = value; }
        }

        public int MyPlaceID
        {
            get { return PlaceID; }
            set { PlaceID = value; }
        }

        /* === Constructor === */
        public DTO_ParkingPlace()
        {
        }

        public DTO_ParkingPlace(int placeID, string placeName, string placeLocation, int numberOfCar, int numberOfMotor, int numberCarFree, int numberMotorFree, int status)
        {
            this.PlaceID = placeID;
            this.PlaceName = placeName;
            this.PlaceLocation = placeLocation;
            this.NumberOfCar = numberOfCar;
            this.NumberOfMotor = numberOfMotor;
            this.NumberCarFree = numberCarFree;
            this.NumberMotorFree = numberMotorFree;
            this.Status = status;
        }

        public DTO_ParkingPlace(int placeID, int numberCarFree)
        {
            PlaceID = placeID;
            NumberCarFree = numberCarFree;
        }

    }
}
