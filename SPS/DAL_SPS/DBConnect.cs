using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace DAL_SPS
{
    public class DBConnect
    {
        protected SqlConnection conn = new SqlConnection("Data Source=45.76.154.211,1433;Initial Catalog=SmartParkings;Persist Security Info=True;User ID=sa;Password=Sps1234");
    }
}
