using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO__SmartParking
{
    public class DTO_ParkingPlace
    {
        private int PlaceID;
        private string PlaceName;
        private string PlaceLocation;
        private int NumberOfSlot;
        private int NumberOfCar;
        private int NumberOfMotor;
        private int NumberCarFree;
        private int NumberMotorFree;

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


        public int MyNumberOfSlot
        {
            get { return NumberOfSlot; }
            set { NumberOfSlot = value; }
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

        public DTO_ParkingPlace(int placeID, string placeName, string placeLocation, int numberOfSlot, int numberOfCar, int numberOfMotor, int numberCarFree, int numberMotorFree)
        {
            PlaceID = placeID;
            PlaceName = placeName;
            PlaceLocation = placeLocation;
            NumberOfSlot = numberOfSlot;
            NumberOfCar = numberOfCar;
            NumberOfMotor = numberOfMotor;
            NumberCarFree = numberCarFree;
            NumberMotorFree = numberMotorFree;
        }
    }
}
