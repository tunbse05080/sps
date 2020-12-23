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
    public class BUS_Transaction
    {
        DAL_Transaction dalTrans = new DAL_Transaction();
        public DataTable getTransaction()
        {
            return dalTrans.getTransaction();
        }
        public bool checkLicense(string license) //kiem tra bien so xe co trong danh sach transaction khong
        {
            if (dalTrans.getTransaction() != null && dalTrans.getTransaction().Rows.Count > 0)
            {
                return dalTrans.getTransaction().AsEnumerable().Any(row => license == row.Field<String>("LicensePlates"));
            }
            return false;
        }
        public bool checkLicenseinPlace(string license) //kiem tra bien so xe co trong bai khong (TimeOuv != NULL)
        {
            if (dalTrans.getTransactionTimeOutisNull(license) != null && dalTrans.getTransactionTimeOutisNull(license).Rows.Count > 0)
            {
                return true; //xe trong bai
            }
            return false;
        }
        public string checkLicenseTimeOut(string license) //thoi gian ra cua xe (neu = NULL : xe van con trong bai xe)
        {
            if (dalTrans.getTransactionOrdebyTimeIn(license) != null && dalTrans.getTransactionOrdebyTimeIn(license).Rows.Count > 0)
            {
                return dalTrans.getTransactionOrdebyTimeIn(license).Rows[0][2].ToString();
            }
            return "0";
        }
        public bool insertTransaction(DTO_Transaction trans)
        {
            return dalTrans.insertTransaction(trans);
        }
        public bool updateTransaction(DTO_Transaction trans)
        {
            return dalTrans.updateTransaction(trans);
        }
        public int getTransactionID(string license)
        {
            if (dalTrans.getTransactionOrdebyTimeIn(license) != null && dalTrans.getTransactionOrdebyTimeIn(license).Rows.Count > 0)
            {
                return Convert.ToInt32(dalTrans.getTransactionOrdebyTimeIn(license).Rows[0][0].ToString());
            }
            return 0; 
        }
        public int getCardID(int trans)
        {
            if (dalTrans.getTransactionbyID(trans) != null && dalTrans.getTransactionbyID(trans).Rows.Count > 0)
            {
                return Convert.ToInt32(dalTrans.getTransactionbyID(trans).Rows[0][6].ToString());
            }
            return 0;
        }
        public int getParkingID(int trans)
        {
            if (dalTrans.getTransactionbyID(trans) != null && dalTrans.getTransactionbyID(trans).Rows.Count > 0)
            {
                return Convert.ToInt32(dalTrans.getTransactionbyID(trans).Rows[0][7].ToString());
            }
            return 0;
        }

        public string getTimeInbyTransID(int transID) //thong tin thoi gian vao
        {
            if (dalTrans.getTransactionbyID(transID) != null && dalTrans.getTransactionbyID(transID).Rows.Count > 0)
            {
                return dalTrans.getTransactionbyID(transID).Rows[0][1].ToString();
            }
            return "";
            //  return DateTime.ParseExact(dalTicket.getMonthlyTicketbyLicense(license).Rows[0][8].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
        public string getTimeInbyLicense(string license) //thong tin thoi gian vao
        {
            if (dalTrans.getTransactionOrdebyTimeIn(license) != null && dalTrans.getTransactionOrdebyTimeIn(license).Rows.Count > 0)
            {
                return dalTrans.getTransactionOrdebyTimeIn(license).Rows[0][1].ToString();
            }
            return ""; 
            //  return DateTime.ParseExact(dalTicket.getMonthlyTicketbyLicense(license).Rows[0][8].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
