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
    public class BUS_Image
    {
        DAL_Image dalImage = new DAL_Image();
        public DataTable getImage()
        {
            return dalImage.getImage();
        }
        public int getImageID(int transID)
        {
            return Convert.ToInt32(dalImage.getImagebyTransID(transID).Rows[0][0].ToString());
        }
        public bool insertImage(DTO_Image image)
        {
            return dalImage.insertImage(image);
        }
        public bool updateImage(DTO_Image image)
        {
            return dalImage.updateImage(image);
        }
    }
}
