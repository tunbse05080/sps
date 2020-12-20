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
            if (dalCard.getCard() != null && dalCard.getCard().Rows.Count > 0)
            {
                return dalCard.getCard().AsEnumerable().Any(row => cardnumber == row.Field<String>("CardNumber"));
            }
            return false;
        }
        public DataTable getCardNumber(string cardnumber)
        {
            return dalCard.getCardbyCardNumber(cardnumber);
        }
        public int getCardStatus(string cardnumber)
        {
            if (dalCard.getCardbyCardNumber(cardnumber) != null && dalCard.getCardbyCardNumber(cardnumber).Rows.Count > 0)
            {
                return Convert.ToInt32(dalCard.getCardbyCardNumber(cardnumber).Rows[0][3].ToString());
            }
            return 0; 
        }
        public int getCardStatusbyID(int cardID)
        {
            if (dalCard.getCardbyCardID(cardID) != null && dalCard.getCardbyCardID(cardID).Rows.Count > 0)
            {
                return Convert.ToInt32(dalCard.getCardbyCardID(cardID).Rows[0][3].ToString());
            }
            return 0;
        }
        public int getCardID(string cardnumber)
        {
            if (dalCard.getCardbyCardNumber(cardnumber) != null && dalCard.getCardbyCardNumber(cardnumber).Rows.Count > 0)
            {
                return Convert.ToInt32(dalCard.getCardbyCardNumber(cardnumber).Rows[0][0].ToString());
            }
            return 0; 
        }
        public bool updateCard(DTO_Card card)
        {
            return dalCard.updateCard(card);
        }
    }
}
