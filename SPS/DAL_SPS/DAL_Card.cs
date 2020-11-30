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
    }
}
