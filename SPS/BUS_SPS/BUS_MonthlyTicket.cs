using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using DAL_SPS;
using DTO_SPS;

namespace BUS_SPS
{
    public class BUS_MonthlyTicket
    {
        DAL_MonthlyTicket dalTicket = new DAL_MonthlyTicket();
        public DataTable getMonthlyTicket()
        {
            return dalTicket.getMonthlyTicket();
        }
        public bool checkLicense(string license) //kiem tra bien so xe co dang ky ve thang khong
        {
            if (dalTicket.getMonthlyTicket() != null && dalTicket.getMonthlyTicket().Rows.Count > 0)
            {
                return dalTicket.getMonthlyTicket().AsEnumerable().Any(row => license == row.Field<String>("LicensePlates"));
            }
            return false;
        }
        public DataTable getMonthlyTicket(string license)
        {
            return dalTicket.getMonthlyTicketbyLicense(license);
        }
        public string getName(string license) //thong tin ten chu xe
        {
            if (dalTicket.getMonthlyTicketbyLicense(license) != null && dalTicket.getMonthlyTicketbyLicense(license).Rows.Count > 0)
            {
                return dalTicket.getMonthlyTicketbyLicense(license).Rows[0][1].ToString();
            }
            return ""; 
        }
        public int getParkingIDbyLicense(string license) //thong tin bai do xe ung voi bien so
        {
            if (dalTicket.getMonthlyTicketbyLicense(license) != null && dalTicket.getMonthlyTicketbyLicense(license).Rows.Count > 0)
            {
                return Convert.ToInt32(dalTicket.getMonthlyTicketbyLicense(license).Rows[0][10].ToString());
            }
            return 0;
        }
        public int getCardIDbyLicense(string license) //thong tin the
        {
            if (dalTicket.getMonthlyTicketbyLicense(license) != null && dalTicket.getMonthlyTicketbyLicense(license).Rows.Count > 0)
            {
                return Convert.ToInt32(dalTicket.getMonthlyTicketbyLicense(license).Rows[0][9].ToString());
            }
            return 0;
        }
        public string getExpiryDate(string license) //thong tin ngay het han cua ve thang
        {
            if (dalTicket.getMonthlyTicketbyLicense(license) != null && dalTicket.getMonthlyTicketbyLicense(license).Rows.Count > 0)
            {
                return dalTicket.getMonthlyTicketbyLicense(license).Rows[0][8].ToString();
            }
            return ""; 
          //  return DateTime.ParseExact(dalTicket.getMonthlyTicketbyLicense(license).Rows[0][8].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
        public string getStartDate(string license) //thong tin ngay bat dau cua ve thang
        {
            if (dalTicket.getMonthlyTicketbyLicense(license) != null && dalTicket.getMonthlyTicketbyLicense(license).Rows.Count > 0)
            {
                return dalTicket.getMonthlyTicketbyLicense(license).Rows[0][7].ToString();
            }
            return "";
            //  return DateTime.ParseExact(dalTicket.getMonthlyTicketbyLicense(license).Rows[0][8].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
