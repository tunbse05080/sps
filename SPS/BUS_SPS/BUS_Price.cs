﻿using System;
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
        public double getDailyPrice(int parkingID, int vehicleType)
        {
            return Convert.ToInt32(dalPrice.getPrice(parkingID,vehicleType).Rows[0][2].ToString());
        }
        public double getMonthlyPrice(int parkingID, int vehicleType)
        {
            return Convert.ToDouble(dalPrice.getPrice(parkingID, vehicleType).Rows[0][3].ToString());
        }
        public double getFirstBlockPrice(int parkingID, int vehicleType)
        {
            return Convert.ToDouble(dalPrice.getPrice(parkingID, vehicleType).Rows[0][4].ToString());
        }
        public double getNextBlockPrice(int parkingID, int vehicleType)
        {
            return Convert.ToDouble(dalPrice.getPrice(parkingID, vehicleType).Rows[0][5].ToString());
        }
    }
}