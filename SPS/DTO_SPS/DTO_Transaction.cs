using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_SPS
{
    public class DTO_Transaction
    {
        private int transID;
        private string timeIN;
        private string timeOut;
        private string license;
        private int ticketType;
        private double price;
        private int cardID;
        private int parkingID;
        private int vehicleType;
        private int userIID;
        private int userOID;

        public int MyUserOID
        {
            get { return userOID; }
            set { userOID = value; }
        }


        public int MyUserIID
        {
            get { return userIID; }
            set { userIID = value; }
        }


        /* ======== GETTER/SETTER ======== */
        public int MyVehicleType
        {
            get { return vehicleType; }
            set { vehicleType = value; }
        }


        public int MyParkingID
        {
            get { return parkingID; }
            set { parkingID = value; }
        }


        public int MyCardID
        {
            get { return cardID; }
            set { cardID = value; }
        }


        public double Myprice
        {
            get { return price; }
            set { price = value; }
        }


        public int MyTicketType
        {
            get { return ticketType; }
            set { ticketType = value; }
        }


        public string MyLicense
        {
            get { return license; }
            set { license = value; }
        }


        public string MyTimeOut
        {
            get { return timeOut; }
            set { timeOut = value; }
        }


        public string MyTimeIN
        {
            get { return timeIN; }
            set { timeIN = value; }
        }


        public int MyTransID
        {
            get { return transID; }
            set { transID = value; }
        }

        /* === Constructor === */
        public DTO_Transaction()
        {
        }

        public DTO_Transaction(int transID, string timeIN, string timeOut, string license, int ticketType, double price, int cardID, int parkingID, int vehicleType, int userIID, int userOID)
        {
            this.transID = transID;
            this.timeIN = timeIN;
            this.timeOut = timeOut;
            this.license = license;
            this.ticketType = ticketType;
            this.price = price;
            this.cardID = cardID;
            this.parkingID = parkingID;
            this.vehicleType = vehicleType;
            this.userIID = userIID;
            this.userOID = userOID;
        }

        public DTO_Transaction(int transID, string timeIN, string license, int ticketType, int cardID, int parkingID, int vehicleType, int userIID)
        {
            this.transID = transID;
            this.timeIN = timeIN;
            this.license = license;
            this.ticketType = ticketType;
            this.cardID = cardID;
            this.parkingID = parkingID;
            this.vehicleType = vehicleType;
            this.userIID = userIID;
        }

        public DTO_Transaction(int transID, string timeOut, double price, int userOID)
        {
            this.transID = transID;
            this.timeOut = timeOut;
            this.price = price;
            this.userOID = userOID;
        }

        public DTO_Transaction(string timeIN, string license, int ticketType, int cardID, int parkingID, int vehicleType, int userIID)
        {
            this.timeIN = timeIN;
            this.license = license;
            this.ticketType = ticketType;
            this.cardID = cardID;
            this.parkingID = parkingID;
            this.vehicleType = vehicleType;
            this.userIID = userIID;
        }
    }
}
