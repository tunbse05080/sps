using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTO_SPS
{
    public class DTO_Card
    {
        private int cardID;
        private string cardNumber;
        private string date;
        private int status;

        /* ======== GETTER/SETTER ======== */
        public int MyStatus
        {
            get { return status; }
            set { status = value; }
        }


        public string MyDate
        {
            get { return date; }
            set { date = value; }
        }


        public string MyCardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; }
        }


        public int MyCardID
        {
            get { return cardID; }
            set { cardID = value; }
        }

        /* === Constructor === */
        public DTO_Card()
        {
        }

        public DTO_Card(int cardID, string cardNumber, string date, int status)
        {
            this.cardID = cardID;
            this.cardNumber = cardNumber;
            this.date = date;
            this.status = status;
        }
    }
}
