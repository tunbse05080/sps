using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_SPS
{
    public class DTO_Price
    {
        private int priceID;
        private int vehicleType;
        private double daily;
        private double monthly;
        private double firstBlock;
        private double nextBlock;
        private int prakingID;
        private int timeFirstBlock;
        private string timeNextBlock;

        public string MyTimeNextBlock
        {
            get { return timeNextBlock; }
            set { timeNextBlock = value; }
        }


        public int MyTimeFirstBlock
        {
            get { return timeFirstBlock; }
            set { timeFirstBlock = value; }
        }


        public int MyParkingID
        {
            get { return prakingID; }
            set { prakingID = value; }
        }


        public double MyNextBlock
        {
            get { return nextBlock; }
            set { nextBlock = value; }
        }


        public double MyFirstBlock
        {
            get { return firstBlock; }
            set { firstBlock = value; }
        }


        public double MyMonthly
        {
            get { return monthly; }
            set { monthly = value; }
        }


        public double MyDaily
        {
            get { return daily; }
            set { daily = value; }
        }


        public int MyVehicleType
        {
            get { return vehicleType; }
            set { vehicleType = value; }
        }


        public int MyPriceID
        {
            get { return priceID; }
            set { priceID = value; }
        }

        public DTO_Price()
        {
        }

        public DTO_Price(int priceID, int vehicleType, double daily, double monthly, double firstBlock, double nextBlock, int prakingID)
        {
            this.priceID = priceID;
            this.vehicleType = vehicleType;
            this.daily = daily;
            this.monthly = monthly;
            this.firstBlock = firstBlock;
            this.nextBlock = nextBlock;
            this.prakingID = prakingID;
        }

        public DTO_Price(int priceID, int vehicleType, double daily, double monthly, double firstBlock, double nextBlock, int prakingID, int timeFirstBlock, int timeNextBlock) : this(priceID, vehicleType, daily, monthly, firstBlock, nextBlock, prakingID)
        {
            this.timeFirstBlock = timeFirstBlock;
            this.timeNextBlock = timeNextBlock;
        }
    }
}
