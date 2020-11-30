using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.ML;
using Emgu.CV.ML.Structure;
using Emgu.CV.UI;
using Emgu.Util;
using System.Diagnostics;
using Emgu.CV.CvEnum;
using System.IO;
using System.IO.Ports;
using tesseract;
using System.Collections;
using System.Threading;
using System.Media;
using System.Runtime.InteropServices;
using SpeechLib;

namespace SPS
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public void LoadData()
        {
            CSDL dl = new CSDL();
            dataGridView1.DataSource = dl.GetAll();
        }



        #region định nghĩa
        List<Image<Bgr, Byte>> PlateImagesList = new List<Image<Bgr, byte>>();
        List<string> PlateTextList = new List<string>();
        List<Rectangle> listRect = new List<Rectangle>();
        PictureBox[] box = new PictureBox[12];

        public TesseractProcessor full_tesseract = null;
        public TesseractProcessor ch_tesseract = null;
        public TesseractProcessor num_tesseract = null;
        private string m_path = Application.StartupPath + @"\data\";
        private List<string> lstimages = new List<string>();
        private const string m_lang = "eng";
        Capture capture = null;
       
       
       
        int current = 0;
        #endregion

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'bIENSOXEDataSet1.BienSo' table. You can move, or remove it, as needed.
          //  this.bienSoTableAdapter1.Fill(this.bIENSOXEDataSet1.BienSo);
           
            // TODO: This line of code loads data into the 'bIENSOXEDataSet.BienSo' table. You can move, or remove it, as needed.
         
            this.LoadData();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox5.Text = "";
            textBox3.Text = "";

         
            full_tesseract = new TesseractProcessor();
            bool succeed = full_tesseract.Init(m_path, m_lang, 3);
            if (!succeed)
            {
                MessageBox.Show("Tesseract initialization failed. The application will exit.");
                Application.Exit();
            }
            full_tesseract.SetVariable("tessedit_char_whitelist", "ACDFHKLMNPRSTVXY1234567890").ToString();

            ch_tesseract = new TesseractProcessor();
            succeed = ch_tesseract.Init(m_path, m_lang, 3);
            if (!succeed)
            {
                MessageBox.Show("Tesseract initialization failed. The application will exit.");
                Application.Exit();
            }
            ch_tesseract.SetVariable("tessedit_char_whitelist", "ACDEFHKLMNPRSTUVXY").ToString();

            num_tesseract = new TesseractProcessor();
            succeed = num_tesseract.Init(m_path, m_lang, 3);
            if (!succeed)
            {
                MessageBox.Show("Tesseract initialization failed. The application will exit.");
                Application.Exit();
            }
            num_tesseract.SetVariable("tessedit_char_whitelist", "1234567890").ToString();

            System.Environment.CurrentDirectory = System.IO.Path.GetFullPath(m_path);
            for (int i = 0; i < box.Length; i++)
            {
                box[i] = new PictureBox();
            }
            string folder = Application.StartupPath + "\\ImageTest";
            foreach (string fileName in Directory.GetFiles(folder, "*.bmp", SearchOption.TopDirectoryOnly))
            {
                lstimages.Add(Path.GetFullPath(fileName));
            }
            foreach (string fileName in Directory.GetFiles(folder, "*.jpg", SearchOption.TopDirectoryOnly))
            {
                lstimages.Add(Path.GetFullPath(fileName));
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            reset();
            bool checkxe = true;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image (*.bmp; *.jpg; *.jpeg; *.png) |*.bmp; *.jpg; *.jpeg; *.png|All files (*.*)|*.*||";
            dlg.InitialDirectory = Application.StartupPath + "\\ImageTest";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string startupPath = dlg.FileName;


            for (int i = 0; i < box.Length; i++)
            {
                this.Controls.Remove(box[i]);
            }

            ProcessImage(startupPath);
            if (PlateImagesList.Count != 0)
            {
                Image<Bgr, byte> src = new Image<Bgr, byte>(PlateImagesList[0].ToBitmap());
                Bitmap grayframe;
                NhanDang con = new NhanDang();
                Bitmap color;
                MessageBox.Show("1");
                int c = con.IdentifyContours(src.ToBitmap(), 50, false, out grayframe, out color, out listRect);
                MessageBox.Show("2");
                //int z = con.count;
                pictureBox6.Image = color;
                pictureBox3.Image = grayframe;
                MessageBox.Show("3");
               // textBox2.Text = c.ToString();
                Image<Gray, byte> dst = new Image<Gray, byte>(grayframe);
                //dst = dst.Dilate(2);
                //dst = dst.Erode(3);
                grayframe = dst.ToBitmap();
                //pictureBox2.Image = grayframe.Clone(listRect[2], grayframe.PixelFormat);
                string zz = "";
                string bienso = "";

                // lọc và sắp xếp số
                List<Bitmap> bmp = new List<Bitmap>();
                List<int> erode = new List<int>();
                List<Rectangle> up = new List<Rectangle>();
                List<Rectangle> dow = new List<Rectangle>();
                int up_y = 0, dow_y = 0;
                bool flag_up = false;

                int di = 0;

                if (listRect == null) return;

                for (int i = 0; i < listRect.Count; i++)
                {
                    Bitmap ch = grayframe.Clone(listRect[i], grayframe.PixelFormat);
                    int cou = 0;
                    full_tesseract.Clear();
                    full_tesseract.ClearAdaptiveClassifier();
                    string temp = full_tesseract.Apply(ch);
                    while (temp.Length > 3)
                    {
                        Image<Gray, byte> temp2 = new Image<Gray, byte>(ch);
                        temp2 = temp2.Erode(2);
                        ch = temp2.ToBitmap();
                        full_tesseract.Clear();
                        full_tesseract.ClearAdaptiveClassifier();
                        temp = full_tesseract.Apply(ch);
                        cou++;
                        if (cou > 10)
                        {
                            listRect.RemoveAt(i);
                            i--;
                            di = 0;
                            break;
                        }
                        di = cou;
                    }
                }

                for (int i = 0; i < listRect.Count; i++)
                {
                    for (int j = i; j < listRect.Count; j++)
                    {
                        if (listRect[i].Y > listRect[j].Y + 100)
                        {
                            flag_up = true;
                            up_y = listRect[j].Y;
                            dow_y = listRect[i].Y;
                            break;
                        }
                        else if (listRect[j].Y > listRect[i].Y + 100)
                        {
                            flag_up = true;
                            up_y = listRect[i].Y;
                            dow_y = listRect[j].Y;
                            break;
                        }
                        if (flag_up == true) break;
                    }
                }

                for (int i = 0; i < listRect.Count; i++)
                {
                    if (listRect[i].Y < up_y + 50 && listRect[i].Y > up_y - 50)
                    {
                        up.Add(listRect[i]);
                    }
                    else if (listRect[i].Y < dow_y + 50 && listRect[i].Y > dow_y - 50)
                    {
                        dow.Add(listRect[i]);
                    }
                }

                if (flag_up == false) dow = listRect;

                for (int i = 0; i < up.Count; i++)
                {
                    for (int j = i; j < up.Count; j++)
                    {
                        if (up[i].X > up[j].X)
                        {
                            Rectangle w = up[i];
                            up[i] = up[j];
                            up[j] = w;
                        }
                    }
                }
                for (int i = 0; i < dow.Count; i++)
                {
                    for (int j = i; j < dow.Count; j++)
                    {
                        if (dow[i].X > dow[j].X)
                        {
                            Rectangle w = dow[i];
                            dow[i] = dow[j];
                            dow[j] = w;
                        }
                    }
                }

                int x = 12;
                int c_x = 0;

                for (int i = 0; i < up.Count; i++)
                {
                    Bitmap ch = grayframe.Clone(up[i], grayframe.PixelFormat);
                    Bitmap o = ch;
                    //ch = con.Erodetion(ch);
                    string temp="";
                    if (i < 2)
                    {
                        temp = Ocr(ch, false, true); // nhan dien so
                    }
                    if (i == 2)
                    {
                        temp = Ocr(ch, false, false);// nhan dien chu
                    }
                    if (i == 3)
                    {
                        checkxe = false;
                        temp = Ocr(ch, false, true); // nhan dien so
                    }
         

                    zz += temp;
                    bienso += temp;
                    box[i].Location = new Point(x + i * 50, 0);
                    box[i].Size = new Size(50, 100);
                    box[i].SizeMode = PictureBoxSizeMode.StretchImage;
                    box[i].Image = ch;
                    panel1.Controls.Add(box[i]);
                    c_x++;
                }
                zz += "\r\n";
              
                for (int i = 0; i < dow.Count; i++)
                {
                    Bitmap ch = grayframe.Clone(dow[i], grayframe.PixelFormat);
                    //ch = con.Erodetion(ch);
                    string temp = Ocr(ch, false, true); // nhan dien so


                    zz += temp;
                    bienso += temp;
                    box[i + c_x].Location = new Point(x + i * 50, 100);
                    box[i + c_x].Size = new Size(50, 100);
                    box[i + c_x].SizeMode = PictureBoxSizeMode.StretchImage;
                    box[i + c_x].Image = ch;

                    panel1.Controls.Add(box[i + c_x]);
                }
                #region hienthi
                textBox1.Text = zz;
                textBox2.Text = DateTime.Now.ToString("dd/MM/yyyy ");
                textBox5.Text = DateTime.Now.ToString("hh:mm:ss tt");
                if (checkxe == true)
                {
                    textBox10.Text = "Xe Ôtô";

                }
                else
                {
                    textBox10.Text = "Xe Máy";
                }
                #endregion

                bienso.Trim();
                do
                {
                    bienso = bienso.Replace("\n", "");
                } while (bienso.IndexOf("\n") != -1);

            
                string Str1 = bienso.Substring(0,2);
                int n=int.Parse(Str1);
                #region switch
                switch (n)
                {
                    case 11:
                           textBox3.Text="Cao Bằng";
                           break;
                    case 12:
                           textBox3.Text = "Lạng Sơn";
                           break;
                    case 14:
                           textBox3.Text = "Quảng Ninh";
                           break;

                    case 15:
                           textBox3.Text = "Hải Phòng";
                           break;
                    case 16:
                           textBox3.Text = "Hải Phòng";
                           break;
                    case 17:
                           textBox3.Text = "Thái Bình";
                           break;
                    case 18:
                           textBox3.Text = "Nam Định";
                           break;
                    case 19:
                           textBox3.Text = "Phú Thọ";
                           break;
                    case 20:
                           textBox3.Text = "Thái Nguyên";
                           break;
                    case 21:
                           textBox3.Text = "Yên Bái";
                           break;
                    case 22:
                           textBox3.Text = "Tuyên Quang";
                           break;
                    case 23:
                           textBox3.Text = "Hà Giang";
                           break;
                    case 24:
                           textBox3.Text = "Lao Cai";
                           break;
                    case 25:
                           textBox3.Text = "Lai Châu";
                           break;
                    case 26:
                           textBox3.Text = "Sơn La";
                           break;
                    case 27:
                           textBox3.Text = "Điện Biên";
                           break;
                    case 28:
                           textBox3.Text = "Hòa Bình";
                           break;
                    case 29:
                           textBox3.Text = "Hà Nội";
                           break;
                    case 30:
                           textBox3.Text = "Hà Nội";
                           break;
                    case 31:
                           textBox3.Text = "Hà Nội";
                           break;
                    case 32:
                           textBox3.Text = "Hà Nội";
                           break;
                    case 33:
                           textBox3.Text = "Hà Nội";
                           break;
                    case 34:
                           textBox3.Text = "Hải Dương";
                           break;
                    case 35:
                           textBox3.Text = "Ninh Bình";
                           break;
                    case 36:
                           textBox3.Text = "Thanh Hóa";
                           break;
                    case 37:
                           textBox3.Text = "Nghệ An";
                           break;
                    case 38:
                           textBox3.Text = "Hà Tĩnh";
                           break;
                    case 43:
                           textBox3.Text = "Đà Nẵng";
                           break;
                    case 47:
                           textBox3.Text = "Đắc Lắc";
                           break;
                    case 48:
                           textBox3.Text = "Đắc Nông";
                           break;
                    case 49:
                           textBox3.Text = "Lâm Đồng";
                           break;
                    case 50:
                           textBox3.Text = "TP.Hồ Chí Minh";
                           break;
                    case 51:
                           textBox3.Text = "TP.Hồ Chí Minh";
                           break;
                    case 52:
                           textBox3.Text = "TP.Hồ Chí Minh";
                           break;
                    case 53:
                           textBox3.Text = "TP.Hồ Chí Minh";
                           break;
                    case 54:
                           textBox3.Text = "TP.Hồ Chí Minh";
                           break;
                    case 55:
                           textBox3.Text = "TP.Hồ Chí Minh";
                           break;
                    case 56:
                           textBox3.Text = "TP.Hồ Chí Minh";
                           break;
                    case 57:
                           textBox3.Text = "TP.Hồ Chí Minh";
                           break;
                    case 58:
                           textBox3.Text = "TP.Hồ Chí Minh";
                           break;
                    case 59:
                           textBox3.Text = "TP.Hồ Chí Minh";
                           break;

                    case 60:
                           textBox3.Text = "Đồng Nai";
                           break;

                    case 61:
                           textBox3.Text = "Bình Dương";
                           break;
                    case 62:
                           textBox3.Text = "Long An";
                           break;
                    case 63:
                           textBox3.Text = "Tiền Giang";
                           break;
                    case 64:
                           textBox3.Text = "Vĩnh Long";
                           break;
                    case 65:
                           textBox3.Text = "Cần Thơ";
                           break;
                    case 66:
                           textBox3.Text = "Đồng Tháp";
                           break;
                    case 67:
                           textBox3.Text = "An Giang";
                           break;
                    case 68:
                           textBox3.Text = "Kiên Giang";
                           break;
                    case 69:
                           textBox3.Text = "Cà Mau";
                           break;
                    case 70:
                           textBox3.Text = "Tây Ninh";
                           break;
                    case 71:
                           textBox3.Text = "Bến Tre";
                           break;
                    case 72:
                           textBox3.Text = "Bà Rịa-Vũng Tàu";
                           break;
                    case 73:
                           textBox3.Text = "Quảng Bình";
                           break;
                    case 74:
                           textBox3.Text = "Quảng Trị";
                           break;
                    case 75:
                           textBox3.Text = "Huế";
                           break;
                    case 76:
                           textBox3.Text = "Quảng Ngãi";
                           break;
                    case 77:
                           textBox3.Text = "Bình Định";
                           break;
                    case 78:
                           textBox3.Text = "Phú Yên";
                           break;
                    case 79:
                           textBox3.Text = "Khánh Hòa";
                           break;
                    case 81:
                           textBox3.Text = "Gia Lai";
                           break;
                    case 82:
                           textBox3.Text = "Kom Tum";
                           break;
                    case 83:
                           textBox3.Text = "Sóc Trăng";
                           break;
                    case 84:
                           textBox3.Text = "Trà Vinh";
                           break;
                    case 85:
                           textBox3.Text = "Ninh Thuận";
                           break;
                    case 86:
                           textBox3.Text = "Bình Thuận";
                           break;
                    case 88:
                           textBox3.Text = "Vĩnh Phúc";
                           break;
                    case 89:
                           textBox3.Text = "Hưng Yên";
                           break;
                    case 90:
                           textBox3.Text = "Hà Nam";
                           break;

                    case 92:
                           textBox3.Text = "Quảng Nam";
                           break;

                    case 93:
                           textBox3.Text = "Bình Phước";
                           break;
                    case 94:
                           textBox3.Text = "Bạc Liêu";
                           break;
                    case 95:
                           textBox3.Text = "Hậu Giang";
                           break;
                    case 97:
                           textBox3.Text = "Bắc Kạn";
                           break;
                    case 98:
                           textBox3.Text = "Bắc Giang";
                           break;
                    case 99:
                           textBox3.Text = "Bắc Ninh";
                           break;
                }
                #endregion

                #region them
                CSDL dl = new CSDL();
                string bienso1, ngay, gio, tinh, loaixe;
                bienso1 = textBox1.Text.Trim();
                ngay = textBox2.Text.Trim();
                gio = textBox5.Text.Trim();
                tinh = textBox3.Text.Trim();
                loaixe = textBox10.Text.Trim();
                if (bienso1 != "" && ngay != "" && gio != "" && tinh != ""&&loaixe!="")
                {
                    DataTable dt = dl.Check(bienso1);
                    if (dt.Rows.Count == 0)
                    {
                        bool ok = dl.InsertBienSo(bienso1, ngay, gio, tinh,loaixe);
                        if (ok)
                        {
                            this.LoadData();

                        }
                        else MessageBox.Show("Có lỗi khi thêm vào cơ sở dữ liệu !");
                    }
                    else MessageBox.Show("Biển số đã tồn tại trong quá trình thêm", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("Bạn chưa nhập đầy đủ", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                #endregion


            }
        }

        public void ProcessImage(string urlImage)
        {
            PlateImagesList.Clear();
            PlateTextList.Clear();

            Bitmap img = new Bitmap(urlImage);
            pictureBox1.Image = img;
            pictureBox1.Update();

            FindLicensePlate(img);

        }
        private string Ocr(Bitmap image_s, bool isFull, bool isNum = false)
        {
            string temp = "";
            Image<Gray, byte> src = new Image<Gray, byte>(image_s);
            double ratio = 1;
            while (true)
            {
                ratio = (double)CvInvoke.cvCountNonZero(src) / (src.Width * src.Height);
                if (ratio > 0.5) break;
                src = src.Dilate(2);
            }
            Bitmap image = src.ToBitmap();

            TesseractProcessor ocr;
            if (isFull)
                ocr = full_tesseract;
            else if (isNum)
                ocr = num_tesseract;
            else
                ocr = ch_tesseract;

            int cou = 0;
            ocr.Clear();
            ocr.ClearAdaptiveClassifier();
            temp = ocr.Apply(image);
            while (temp.Length > 3)
            {
                Image<Gray, byte> temp2 = new Image<Gray, byte>(image);
                temp2 = temp2.Erode(2);
                image = temp2.ToBitmap();
                ocr.Clear();
                ocr.ClearAdaptiveClassifier();
                temp = ocr.Apply(image);
                cou++;
                if (cou > 10)
                {
                    temp = "";
                    break;
                }
            }
            return temp;

        }

        public void FindLicensePlate(Bitmap image)
        {
          

            Image<Bgr, byte> frame = new Image<Bgr, byte>(image);




            using (Image<Gray, byte> grayframe = new Image<Gray, byte>(image))
            {


                var faces =
                       grayframe.DetectHaarCascade(
                               new HaarCascade(Application.StartupPath + "\\output-hv-33-x25.xml"), 1.1, 8,
                               HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                               new Size(0, 0)
                               )[0];

                #region giaithich
                //Thấy khu vực hình chữ nhật trong hình ảnh cho rằng rất có thể chứa các đối tượng thác đã được huấn luyện và trả lại những 
                //vùng như là một chuỗi các hình chữ nhật. Các chức năng quét hình ảnh nhiều lần ở các quy mô khác nhau 
               // (xem cvSetImagesForHaarClassifierCascade). Mỗi lần nó xem xét các khu vực chồng chéo trong hình ảnh và áp dụng 
               //     các phân loại cho các khu vực sử dụng cvRunHaarClassifierCascade. Nó cũng có thể áp dụng một số chẩn đoán để 
              //          giảm số lượng các khu vực phân tích, chẳng hạn như prunning Canny. Sau khi đã tiến hành và thu thập các hình
               // chữ nhật ứng cử viên (khu vực mà thông qua các đợt phân loại), nó nhóm chúng và trả về một chuỗi các hình chữ nhật trung 
               //     bình cho mỗi nhóm đủ lớn. Các thông số mặc định (scale_factor = 1.1, min_neighbors = 3, cờ = 0) được điều chỉnh cho chính 
              //          xác phát hiện đối tượng chưa chậm. Đối với một hoạt động nhanh hơn trên thực video hình ảnh các thiết lập là: scale_factor = 1.2,
                //   min_neighbors = 2, cờ = CV_HAAR_DO_CANNY_PRUNING, MIN_SIZE = <có thể tối thiểu kích thước mặt> (ví dụ, ~ 1/4 đến 1/16 của khu vực hình ảnh trong trường hợp hội nghị truyền hình).
              #endregion

                foreach (var face in faces)
                    {
                        Image<Bgr, Byte> tmp = frame.Copy();
                        tmp.ROI = face.rect;
                     
                        frame.Draw(face.rect, new Bgr(Color.Red), 2);

                        PlateImagesList.Add(tmp.Resize(500, 500, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC, true));

                        //Image<Gray, byte> tmp2 = new Image<Gray, byte>(tmp.ToBitmap());
                        //tmp2 = tmp2.ThresholdBinary(new Gray(50), new Gray(255));




                        pictureBox6.Image = tmp.ToBitmap();
                        pictureBox6.Update();




                        //string pl = this.Ocr(tmp2.ToBitmap());

                        //PlateTextList.Add(pl);
                    }

                    Image<Bgr, Byte> showimg = new Image<Bgr, Byte>(image.Size);
                    showimg = frame.Resize(imageBox1.Width, imageBox1.Height, 0);
                    imageBox1.Image = showimg;

                }
            
        }

        private void dataGridView1_SelectionChanged_1(object sender, EventArgs e)
        {
            try
            {
                textBox6.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                textBox9.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                textBox7.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                textBox8.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            }
            catch (Exception)
            {
                textBox6.Text = "";
                textBox9.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";

            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            CSDL dl = new CSDL();
            string bienso1;
            bienso1 = textBox6.Text.Trim();
     
            if (bienso1 != "")
            {
                DataTable dt = dl.Check(bienso1);
                if (dt.Rows.Count != 0)
                {
                    if (MessageBox.Show("Bạn có chắc chắn muốn xóa không ?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        bool ok = dl.DeleteBienSo(bienso1);
                        if (ok)
                        {
                            this.LoadData();

                        }
                        else MessageBox.Show("Có lỗi khi xóa cơ sở dữ liệu !");
                    }
                }
                else MessageBox.Show("Biển số không tồn tại !", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                MessageBox.Show("Bạn chưa nhập biển cần xóa !", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
             SpVoice voice =new SpVoice();
             voice.Speak(textBox6.Text, SpeechVoiceSpeakFlags.SVSFDefault);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                CSDL dl = new CSDL();
                dataGridView1.DataSource = dl.TimKiem1(textBox4.Text.Trim());
            }

            if (radioButton2.Checked == true) {

                CSDL dl = new CSDL();
                dataGridView1.DataSource = dl.TimKiem2(textBox4.Text.Trim());
            }
         

        
        }

        public void reset()
        {
            pictureBox1.Image = null;
            pictureBox3.Image = null;
            pictureBox6.Image = null;
            imageBox1.Image = null;
            panel1.Controls.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox5.Text = "";
            textBox3.Text = "";
            textBox10.Text = "";
          
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            reset();
           
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn chắc chắn muốn thoát không?", "Hỏi Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mọi thắc mắc xin các bạn gửi về địa chỉ gmail: Dinhhuuvi250494@gmail.com để được giải đáp.Xin Chân thành cảm ơn!");
        }
      
        bool chup = true;
        private void buttonX2_Click(object sender, EventArgs e)
        {

            if (chup == true)
            {
                capture = new Emgu.CV.Capture();
                timer1.Enabled = true;
                chup = false;

            }
            else
            {

                timer1.Enabled = false;
                capture.QueryFrame().Save("aa.bmp");
                FileStream fs = new FileStream(m_path + "aa.bmp", FileMode.Open, FileAccess.Read);
                string link = m_path + "aa.bmp";
             
                ProcessImage(link.Trim());
                Image temp = Image.FromStream(fs);
                fs.Close();
                pictureBox1.Image = temp;
                pictureBox1.Update();
                chup = true;

            }
                 
               
        }

        bool kt = true;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (kt == true)
            {
               kt = false;
                new Thread(() =>
                {
                    try
                    {
                        capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 640);
                        capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 480);
                        Image<Bgr, byte> cap = capture.QueryFrame();
                        if (cap != null)
                        {
                            MethodInvoker mi = delegate
                            {
                                try
                                {
                                    Bitmap bmp = cap.ToBitmap();
                                    pictureBox1.Image = bmp;
                                    pictureBox1.Update();
                                 

                                }
                                catch (Exception ex)
                                { }
                            };
                            if (InvokeRequired)
                                Invoke(mi);
                        }
                    }
                    catch (Exception) { }
                    kt = true;
                }).Start();

            }
        }

        

    

 
      

       


    }
}
