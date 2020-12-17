using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTO_SPS
{
    public class DTO_MonthlyTicket
    {
        private int ticketID;
        private string name;
        private int identity;
        private int phone;
        private string email;
        private int vehicle;
        private string license;
        private string regisdate;
        private string Exdate;
        private int cardID;
        private int parkingID;



        //private int parkingID;

        /* ======== GETTER/SETTER ======== */
        //public int MyParkingID
        //{
        //    get { return parkingID; }
        //    set { parkingID = value; }
        //}
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

        public string MyExdate
        {
            get { return Exdate; }
            set { Exdate = value; }
        }


        public string MyRegisdateDate
        {
            get { return regisdate; }
            set { regisdate = value; }
        }

        public string MyLicense
        {
            get { return license; }
            set { license = value; }
        }

        public int MyVehicle
        {
            get { return vehicle; }
            set { vehicle = value; }
        }

        public string MyEmail
        {
            get { return email; }
            set { email = value; }
        }

        public int MyPhone
        {
            get { return phone; }
            set { phone = value; }
        }

        public int MyIdentity
        {
            get { return identity; }
            set { identity = value; }
        }

        public string MyName
        {
            get { return name; }
            set { name = value; }
        }

        public int MyTicketID
        {
            get { return ticketID; }
            set { ticketID = value; }
        }

        /* === Constructor === */
        public DTO_MonthlyTicket()
        {
        }

        public DTO_MonthlyTicket(int ticketID, string name, int identity, int phone, string email, int vehicle, string license, string regisdate, string exdate, int cardID)
        {
            this.ticketID = ticketID;
            this.name = name;
            this.identity = identity;
            this.phone = phone;
            this.email = email;
            this.vehicle = vehicle;
            this.license = license;
            this.regisdate = regisdate;
            Exdate = exdate;
            this.cardID = cardID;
           // this.parkingID = parkingID;
        }
    }
}

