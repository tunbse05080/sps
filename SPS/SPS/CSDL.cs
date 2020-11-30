using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SPS
{
    class CSDL
    {
        private SqlConnection _Connection;
        public SqlConnection Connection
        {
            get { return _Connection; }
            set { _Connection = value; }
        }

        // Khoi tao 
        public CSDL() { }

        private SqlConnection ReturnAnOpenConnection()
        {
            SqlConnection conn = new SqlConnection(@"Data Source=spsdata.cmuuiywkgmhz.ap-southeast-1.rds.amazonaws.com,1433;Initial Catalog=BIENSOXE;User ID=sps;Password=12345678");
            conn.Open();
            return conn;
        }

        public DataTable GetAll() 
        {
            _Connection = this.ReturnAnOpenConnection();
            SqlCommand cmd = new SqlCommand("Select * from BienSo", _Connection);
            DataTable dt = new DataTable();
            SqlDataAdapter adt = new SqlDataAdapter(cmd);
            adt.Fill(dt);
            return dt;
        
        }

        public DataTable Check(string bienso)
        {
            _Connection = this.ReturnAnOpenConnection();
            SqlCommand cmd = new SqlCommand("select * from BienSo where bienso='" + bienso + "'", _Connection);
            DataTable dt = new DataTable();
            SqlDataAdapter adt = new SqlDataAdapter(cmd);
            adt.Fill(dt);
            return dt;
        }

        public DataTable TimKiem1(string bienso)
        {
            _Connection = this.ReturnAnOpenConnection();
            SqlCommand cmd = new SqlCommand("Select * from BienSo where bienso LIKE N'%"+bienso+ "%'", _Connection);
            DataTable dt = new DataTable();
            SqlDataAdapter adt = new SqlDataAdapter(cmd);
            adt.Fill(dt);
            return dt;
        }

        public DataTable TimKiem2(string thanhpho)
        {
            _Connection = this.ReturnAnOpenConnection();
            SqlCommand cmd = new SqlCommand("Select * from BienSo where tinh LIKE N'%" + thanhpho + "%'", _Connection);
            DataTable dt = new DataTable();
            SqlDataAdapter adt = new SqlDataAdapter(cmd);
            adt.Fill(dt);
            return dt;
        }

        public bool InsertBienSo(string bienso, string ngay, string gio, string tinh,string loaixe)
        {
            try
            {
                _Connection = this.ReturnAnOpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "insert into BienSo values('" + bienso + "','" + ngay + "','" + gio + "',N'"+tinh+"',N'"+loaixe+"')";
                cmd.Connection = _Connection;
                cmd.ExecuteNonQuery();
                return true;

            }
            catch (SqlException)
            {
                return false;
            }
        }

        public bool DeleteBienSo(string bienso)
        {
            try
            {
                _Connection = this.ReturnAnOpenConnection();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Delete from BienSo where bienso='" + bienso + "'";
                cmd.Connection = _Connection;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException)
            {
                return false;

            }
        }

    }

}
