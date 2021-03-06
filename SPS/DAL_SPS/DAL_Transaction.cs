﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO_SPS;

namespace DAL_SPS
{
    public class DAL_Transaction : DBConnect
    {
        public DataTable getTransaction()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from [Transaction]", conn);
            DataTable dtTrans = new DataTable();
            da.Fill(dtTrans);
            return dtTrans;
        }

        public DataTable getTransactionOrdebyTimeIn(string license)
        {
            SqlDataAdapter da = new SqlDataAdapter("select TOP(1) * from [Transaction] where LicensePlates = '" + license + "' ORDER BY TimeIn DESC", conn);
            DataTable dtTrans = new DataTable();
            da.Fill(dtTrans);
            return dtTrans;
        }
        public DataTable getTransactionTimeOutisNull(string license)
        {
            SqlDataAdapter da = new SqlDataAdapter("select TOP(1) * from [Transaction] where LicensePlates = '" + license + "' AND TimeOutv is NULL  ORDER BY TimeIn DESC", conn);
            DataTable dtTrans = new DataTable();
            da.Fill(dtTrans);
            return dtTrans;
        }
        public DataTable getTransactionbyLicense(string license)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from [Transaction] where LicensePlates = '" + license + "'", conn);
            DataTable dtTrans = new DataTable();
            da.Fill(dtTrans);
            return dtTrans;
        }
        public DataTable getTransactionbyID(int transID)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from [Transaction] where TransactionID = '" + transID + "'", conn);
            DataTable dtTrans = new DataTable();
            da.Fill(dtTrans);
            return dtTrans;
        }
        public bool insertTransaction(DTO_Transaction trans) //them vao bang Transaction
        {
            try
            {
                // Ket noi
                conn.Open();

                // Query string - vì mình để Transaction ID là identity (giá trị tự tăng dần) nên ko cần phải insert ID
                string SQL = string.Format("INSERT INTO [Transaction](TimeIn, LicensePlates, TypeOfTicket, CardID, ParkingPlaceID, TypeOfVerhicleTran, UserIID) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','{6}')", trans.MyTimeIN, trans.MyLicense, trans.MyTicketType,trans.MyCardID,trans.MyParkingID,trans.MyVehicleType,trans.MyUserIID);

                // Command (mặc định command type = text nên chúng ta khỏi fải làm gì nhiều).
                SqlCommand cmd = new SqlCommand(SQL, conn);

                // Query và kiểm tra
                if (cmd.ExecuteNonQuery() > 0)
                    return true;

            }
            catch (Exception e)
            {

            }
            finally
            {
                // Dong ket noi
                conn.Close();
            }

            return false;
        }
        public bool updateTransaction(DTO_Transaction trans) //update bang Transaction
        {
            try
            {
                // Ket noi
                conn.Open();

                // Query string
                string SQL = string.Format("UPDATE [Transaction] SET TimeOutv = '{0}', TotalPrice = '{1}', UserOID = '{2}' WHERE TransactionID = {3}", trans.MyTimeOut, trans.Myprice,trans.MyUserOID,trans.MyTransID);

                // Command (mặc định command type = text nên chúng ta khỏi fải làm gì nhiều).
                SqlCommand cmd = new SqlCommand(SQL, conn);

                // Query và kiểm tra
                if (cmd.ExecuteNonQuery() > 0)
                    return true;

            }
            catch (Exception e)
            {

            }
            finally
            {
                // Dong ket noi
                conn.Close();
            }

            return false;
        }
    }
}
