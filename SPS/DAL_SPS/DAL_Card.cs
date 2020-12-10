using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DTO_SPS;

namespace DAL_SPS
{
    public class DAL_Card : DBConnect
    {
        public DataTable getCard()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from Card", conn);
            DataTable dtCard = new DataTable();
            da.Fill(dtCard);
            return dtCard;
        }

        public DataTable getCardbyCardNumber(string cardnumber)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from Card where CardNumber = " + cardnumber + "", conn);
            DataTable dtCard = new DataTable();
            da.Fill(dtCard);
            return dtCard;
        }
        public DataTable getCardbyCardID(int cardID)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from Card where CardID = " + cardID + "", conn);
            DataTable dtCard = new DataTable();
            da.Fill(dtCard);
            return dtCard;
        }
        public bool updateCard(DTO_Card card) //update bang Card
        {
            try
            {
                // Ket noi
                conn.Open();
                // Query string
                string SQL = string.Format("UPDATE Card SET Status = '{0}' WHERE CardID = {1}", card.MyStatus,card.MyCardID);
                SqlCommand cmd = new SqlCommand(SQL, conn);
                // Query và kiểm tra
                if (cmd.ExecuteNonQuery() > 0)
                    return true;
            }
            catch (Exception e)
            {
            }
            finally
            {
                // Dong ket noi
                conn.Close();
            }
            return false;
        }
    }
}
