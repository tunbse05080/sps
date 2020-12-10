using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_SPS
{
    public class DTO_Image
    {
        private int imageID;
        private string linkIn;
        private string linkOut;
        private int transID;

        /* ======== GETTER/SETTER ======== */
        public int MyTransID
        {
            get { return transID; }
            set { transID = value; }
        }


        public string MyLinkOut
        {
            get { return linkOut; }
            set { linkOut = value; }
        }


        public string MyLinkIn
        {
            get { return linkIn; }
            set { linkIn = value; }
        }


        public int MyImageID
        {
            get { return imageID; }
            set { imageID = value; }
        }

        /* === Constructor === */
        public DTO_Image()
        {
        }

        public DTO_Image(int imageID, string linkIn, string linkOut, int transID)
        {
            this.imageID = imageID;
            this.linkIn = linkIn;
            this.linkOut = linkOut;
            this.transID = transID;
        }
    }
}
