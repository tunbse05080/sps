using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPS
{
    class Messages
    {
        private string m01 = "Bạn có muốn chắc chắn muốn thoát không?";
        private string m02 = "Thẻ không hợp lệ";
        private string m03 = "Thẻ đang sử dụng";
        private string m04 = "Thẻ bị khóa";

        public string M04
        {
            get { return m04; }
            set { m04 = value; }
        }


        public string M03
        {
            get { return m03 ; }
            set { m03 = value; }
        }


        public string mes(int a)
        {
            if (a == 1)
            {
                return m01;
            }
            if (a == 2)
            {
                return m02;
            }
            if (a == 3)
            {
                return m03;
            }
            if (a == 4)
            {
                return m04;
            }
            else return "";
        }
        public string M02
        {
            get { return m02 ; }
            set { m02  = value; }
        }

        public string M01
        {
            get { return m01 ; }
            set { m01  = value; }
        }

    }
}
