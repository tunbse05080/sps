using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_SPS;
using DTO_SPS;

namespace BUS_SPS
{
    public class BUS_Price
    {
        DAL_Price dalPrice = new DAL_Price();
        public DataTable getPrice()
        {
            return dalPrice.getPrice();
        }
        public double getDailyPrice(int parkingID, int vehicleType, DateTime Timein)
        {
            if (dalPrice.getCurrentPrice(parkingID, vehicleType, Timein) != null && dalPrice.getCurrentPrice(parkingID, vehicleType, Timein).Rows.Count > 0)
            {
                return Convert.ToInt32(dalPrice.getCurrentPrice(parkingID, vehicleType, Timein).Rows[0][2].ToString());
            }
            return 0;
        }

        public double getFirstBlockPrice(int parkingID, int vehicleType, DateTime Timein)
        {
            if (dalPrice.getCurrentPrice(parkingID, vehicleType, Timein) != null && dalPrice.getCurrentPrice(parkingID, vehicleType, Timein).Rows.Count > 0)
            {
                return Convert.ToDouble(dalPrice.getCurrentPrice(parkingID, vehicleType, Timein).Rows[0][3].ToString());
            }
            return 0;
        }
        public double getNextBlockPrice(int parkingID, int vehicleType, DateTime Timein)
        {
            if (dalPrice.getCurrentPrice(parkingID, vehicleType, Timein) != null && dalPrice.getCurrentPrice(parkingID, vehicleType, Timein).Rows.Count > 0)
            {
                return Convert.ToDouble(dalPrice.getCurrentPrice(parkingID, vehicleType, Timein).Rows[0][4].ToString());
            }
            return 0;
        }
        public int gettimeFirstBlock(int parkingID, int vehicleType, DateTime Timein)
        {
            if (dalPrice.getCurrentPrice(parkingID, vehicleType, Timein) != null && dalPrice.getCurrentPrice(parkingID, vehicleType, Timein).Rows.Count > 0)
            {
                return Convert.ToInt32(dalPrice.getCurrentPrice(parkingID, vehicleType, Timein).Rows[0][6].ToString());
            }
            return 0;
        }
        public int gettimeNextBlock(int parkingID, int vehicleType, DateTime Timein)
        {
            if (dalPrice.getCurrentPrice(parkingID, vehicleType, Timein) != null && dalPrice.getCurrentPrice(parkingID, vehicleType, Timein).Rows.Count > 0)
            {
                return Convert.ToInt32(dalPrice.getCurrentPrice(parkingID, vehicleType, Timein).Rows[0][7].ToString());
            }
            return 0;
        }
    }
}
