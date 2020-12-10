using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DAL_SPS;
using DTO_SPS;

namespace BUS_SPS
{
    public class BUS_Card
    {
        DAL_Card dalCard = new DAL_Card();
        public DataTable getCard()
        {
            return dalCard.getCard();
        }
        public bool checkCard(string cardnumber) //kiem tra Card co ton tai trong he thong khong
        {
            return dalCard.getCard().AsEnumerable().Any(row => cardnumber == row.Field<String>("CardNumber"));
        }
        public DataTable getCardNumber(string cardnumber)
        {
            return dalCard.getCardbyCardNumber(cardnumber);
        }
        public int getCardStatus(string cardnumber)
        {
            return Convert.ToInt32(dalCard.getCardbyCardNumber(cardnumber).Rows[0][3].ToString());
        }
        public int getCardStatusbyID(int cardID)
        {
            return Convert.ToInt32(dalCard.getCardbyCardID(cardID).Rows[0][3].ToString());
        }
        public int getCardID(string cardnumber)
        {
            return Convert.ToInt32(dalCard.getCardbyCardNumber(cardnumber).Rows[0][0].ToString());
        }
        public bool updateCard(DTO_Card card)
        {
            return dalCard.updateCard(card);
        }
    }
}
