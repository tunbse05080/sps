using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_SPS
{
    public class DAL_Image : DBConnect
    {
        public DataTable getImage()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from Image", conn);
            DataTable dtImage = new DataTable();
            da.Fill(dtImage);
            return dtImage;
        }

    }
}
