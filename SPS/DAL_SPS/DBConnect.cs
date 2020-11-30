using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace DAL_SPS
{
    public class DBConnect
    {
        protected SqlConnection conn = new SqlConnection("Data Source=spsdata.cmuuiywkgmhz.ap-southeast-1.rds.amazonaws.com,1433;Initial Catalog=SmartParkings;Persist Security Info=True;User ID=sps;Password=12345678");
    }
}
