using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_SPS
{
    public class DTO_User
    {
        private int userID;
        private string name;
        private string dateOfBirth;
        private int gender;
        private string address;
        private string identify;
        private string phone;
        private string email;
        private int parkingID;
        private int accoutID;
        private int status;

        public int MyAccountID
        {
            get { return accoutID; }
            set { accoutID = value; }
        }


        public int MyStatus
        {
            get { return status; }
            set { status = value; }
        }


        public int MyParkingID
        {
            get { return parkingID; }
            set { parkingID = value; }
        }

        public string MyEmail
        {
            get { return email; }
            set { email = value; }
        }


        public string MyPhone
        {
            get { return phone; }
            set { phone = value; }
        }


        public string MyIdentify
        {
            get { return identify; }
            set { identify = value; }
        }


        public string MyAddress
        {
            get { return address; }
            set { address = value; }
        }


        public int MyGender
        {
            get { return gender; }
            set { gender = value; }
        }


        public string MyDateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
        }


        public string MyName
        {
            get { return name; }
            set { name = value; }
        }


        public int MyUserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public DTO_User()
        {
        }

        public DTO_User(int userID, string name, string dateOfBirth, int gender, string address, string identify, string phone, string email, int parkingID, int accoutID, int status)
        {
            this.userID = userID;
            this.name = name;
            this.dateOfBirth = dateOfBirth;
            this.gender = gender;
            this.address = address;
            this.identify = identify;
            this.phone = phone;
            this.email = email;
            this.parkingID = parkingID;
            this.accoutID = accoutID;
            this.status = status;
        }

        public DTO_User(int accoutID, int status)
        {
            this.accoutID = accoutID;
            this.status = status;
        }
    }
}
