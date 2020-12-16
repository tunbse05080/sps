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
            return dalTicket.getMonthlyTicket().AsEnumerable().Any(row => license == row.Field<String>("LicensePlates"));
        }
        public DataTable getMonthlyTicket(string license)
        {
            return dalTicket.getMonthlyTicketbyLicense(license);
        }
        public string getName(string license) //thong tin ten chu xe
        {
            return dalTicket.getMonthlyTicketbyLicense(license).Rows[0][1].ToString();
        }
        public int getCardIDbyLicense(string license) //thong tin the
        {
            return Convert.ToInt32(dalTicket.getMonthlyTicketbyLicense(license).Rows[0][9].ToString());
        }
        public string getExpiryDate(string license) //thong tin ngay het han cua ve thang
        {
             return dalTicket.getMonthlyTicketbyLicense(license).Rows[0][8].ToString();
          //  return DateTime.ParseExact(dalTicket.getMonthlyTicketbyLicense(license).Rows[0][8].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
