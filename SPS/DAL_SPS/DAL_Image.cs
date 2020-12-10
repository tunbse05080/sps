using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO_SPS;

namespace DAL_SPS
{
    public class DAL_Image : DBConnect
    {
        public DataTable getImage()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from Image", conn);
            DataTable dtImage = new DataTable();
            da.Fill(dtImage);
            return dtImage;
        }
        public DataTable getImagebyTransID(int transID)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from Image where TransactionID = "+ transID, conn);
            DataTable dtImage = new DataTable();
            da.Fill(dtImage);
            return dtImage;
        }
        public bool insertImage(DTO_Image image) //them vao bang Image
        {
            try
            {
                // Ket noi
                conn.Open();
                // Query string - vì mình để Image ID là identity (giá trị tự tăng dần) nên ko cần fải insert ID
                string SQL = string.Format("INSERT INTO Image(LinkImageIN, TransactionID) VALUES ('{0}', '{1}')", image.MyLinkIn,image.MyTransID);
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
        public bool updateImage(DTO_Image image) //update bang Image
        {
            try
            {
                // Ket noi
                conn.Open();
                // Query string
                string SQL = string.Format("UPDATE Image SET LinkImageOut = '{0}' WHERE ImageID = {1}", image.MyLinkOut,image.MyImageID);
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
