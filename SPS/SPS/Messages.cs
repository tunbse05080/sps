﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPS
{
    public class Messages
    {
        private string m01 = "Bạn có chắc chắn muốn thoát không?";
        private string m02 = "Thẻ không hợp lệ";
        private string m03 = "Thẻ đang sử dụng";
        private string m04 = "Thẻ bị khóa/hỏng";
        private string m05 = "Tính tiền như vé ngày/block";
        private string m06 = "Cho xe đi qua";
        private string m07 = "Bãi đỗ xe đã đầy";
        private string m08 = "Biển số không đúng";
        private string m09 = "Xe đang trong bãi";
        private string m10 = "Xe không có trong bãi";
        private string m11 = "Vé không đúng xe";
        private string m12= "Tài khoản/mật khẩu không đúng, hoặc không hợp lệ";
        private string m13 ="Bảo vệ chưa đăng nhập";
        private string m14 = "Thoát tài khoản";
        private string m15 = "Xe chưa đăng ký vé tháng";
        private string m16 = "Nhập địa chỉ CameraIP";

        public string M16
        {
            get { return m16; }
            set { m16 = value; }
        }


        public string M15
        {
            get { return m15; }
            set { m15 = value; }
        }


        public string M14
        {
            get { return m14; }
            set { m14 = value; }
        }


        public string M13
        {
            get { return m13; }
            set { m13= value; }
        }


        public string M12
        {
            get { return m12; }
            set { m12 = value; }
        }


        public string M11
        {
            get { return m11; }
            set { m11 = value; }
        }


        public string M10
        {
            get { return m10; }
            set { m10 = value; }
        }


        public string M09
        {
            get { return m09; }
            set { m09 = value; }
        }


        public string M08
        {
            get { return m08; }
            set { m08 = value; }
        }


        public string M07
        {
            get { return m07; }
            set { m07 = value; }
        }


        public string M06
        {
            get { return m06; }
            set { m06 = value; }
        }


        public string M05
        {
            get { return m05; }
            set { m05 = value; }
        }


        public string M04
        {
            get { return m04; }
            set { m04 = value; }
        }


        public string M03
        {
            get { return m03; }
            set { m03 = value; }
        }


        public string mes(int a)
        {
            switch (a)
            {
                case 1:
                    return m01;
                   // break;
                case 2:
                    return m02;
                //  break;
                case 3:
                    return m03;
                case 4:
                    return m04;
                case 5:
                    return m05;
                case 6:
                    return m06;
                case 7:
                    return m07;
                case 8:
                    return m08;
                case 9:
                    return m09;
                case 10:
                    return m10;
                case 11:
                    return m11;
                case 12:
                    return m12;
                case 13:
                    return m13;
                case 14:
                    return m14;
                case 15:
                    return m15;
                case 16:
                    return m16;
                default:
                    return "NONE";
                   // break;
            }
            //if (a == 1)
            //{
            //    return m01;
            //}
            //if (a == 2)
            //{
            //    return m02;
            //}
            //if (a == 3)
            //{
            //    return m03;
            //}
            //if (a == 4)
            //{
            //    return m04;
            //}
            //if (a == 5)
            //{
            //    return m05;
            //}
            //if (a == 6)
            //{
            //    return m06;
            //}
            //if (a == 7)
            //{
            //    return m07;
            //}
            //if (a == 8)
            //{
            //    return m08;
            //}
            //if (a == 9)
            //{
            //    return m09;
            //}
            //if (a == 10)
            //{
            //    return m10;
            //}
            //if (a == 11)
            //{
            //    return m11;
            //}
            //if (a == 12)
            //{
            //    return m12;
            //}
            //if (a == 13)
            //{
            //    return m13;
            //}
            //if (a == 14)
            //{
            //    return m14;
            //}
            //if (a == 15)
            //{
            //    return m15;
            //}
            //if (a == 16)
            //{
            //    return m16;
            //}
            //else return "NONE";
        }
        public string M02
        {
            get { return m02; }
            set { m02 = value; }
        }

        public string M01
        {
            get { return m01; }
            set { m01 = value; }
        }

    }
}
