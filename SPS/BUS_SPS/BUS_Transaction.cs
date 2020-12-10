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
            return dalTrans.getTransaction().AsEnumerable().Any(row => license == row.Field<String>("LicensePlates"));
        }
        public string checkLicenseTimeOut(string license) //thoi gian ra cua xe (neu = NULL : xe van con trong bai xe)
        {
            return dalTrans.getTransactionOrdebyTimeIn(license).Rows[0][2].ToString();
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
            return Convert.ToInt32(dalTrans.getTransactionOrdebyTimeIn(license).Rows[0][0].ToString());
        }
        public int getCardID(int trans)
        {
            return Convert.ToInt32(dalTrans.getTransactionbyID(trans).Rows[0][6].ToString());
        }
        public string getTimeIn(int transID) //thong tin thoi gian vao
        {
            return dalTrans.getTransactionbyID(transID).Rows[0][1].ToString();
            //  return DateTime.ParseExact(dalTicket.getMonthlyTicketbyLicense(license).Rows[0][8].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
