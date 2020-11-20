using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO__SmartParking;

namespace DAL_SmartParking
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
    }
}
