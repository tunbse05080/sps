using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.ML;
using Emgu.CV.ML.Structure;
using Emgu.CV.UI;
using Emgu.Util;
using Emgu.CV.CvEnum;
using DTO_SPS;
using BUS_SPS;
using tesseract;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace SPS
{
    public partial class MainForm : Form
    {
        #region
        private VideoCaptureDevice CAM;
        private Bitmap BMP;
        private FilterInfoCollection CAMS;
        System.Windows.Forms.Timer tmr = null;
        BUS_ParkingPlace busPK = new BUS_ParkingPlace();
        BUS_Card busCard = new BUS_Card();
        BUS_MonthlyTicket busTicket = new BUS_MonthlyTicket();
        Messages mes = new Messages();
        public int CarFree { get; set; }
        public int MotorFree { get; set; }
        private int ParkingID;
        private int GateID;
        private string m_path = Application.StartupPath + @"\data\"; //duong dan luu hinh anh
        List<Image<Bgr, Byte>> PlateImagesList = new List<Image<Bgr, byte>>();
        List<string> PlateTextList = new List<string>();
        List<Rectangle> listRect = new List<Rectangle>();
        PictureBox[] box = new PictureBox[12];
        public TesseractProcessor full_tesseract = null;
        public TesseractProcessor ch_tesseract = null;
        public TesseractProcessor num_tesseract = null;
        private List<string> lstimages = new List<string>();
        private const string m_lang = "eng";
        bool chkCard = true;
        bool chkLicense = true;
        int vehicleType; //loai xe 0-xemay, 1-oto
        string pictureLink;
        #endregion

        public MainForm()
        {
            InitializeComponent();
            CAMS = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (CAMS.Count > 0)
            {
                foreach (FilterInfo info in CAMS)
                {
                    toolStripComboBox1.Items.Add(info.Name);
                }
                toolStripComboBox1.SelectedIndex = 0;
            }
            else
            {
                toolStripComboBox1.Text = "No cameras found";
                string message = "No cameras found. Insert a camera please!";
                string caption = "No cameras found";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(this, message, caption, buttons);

                if (result == DialogResult.Yes)
                {

                    // Closes the parent form.
                    Application.Exit();

                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(mes.mes(1), "Hỏi Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CAM.Stop();
                Application.Exit();
            }    
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
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
            //string folder = Application.StartupPath + "\\ImageTest";
            //foreach (string fileName in Directory.GetFiles(folder, "*.bmp", SearchOption.TopDirectoryOnly))
            //{
            //    lstimages.Add(Path.GetFullPath(fileName));
            //}
            //foreach (string fileName in Directory.GetFiles(folder, "*.jpg", SearchOption.TopDirectoryOnly))
            //{
            //    lstimages.Add(Path.GetFullPath(fileName));
            //}
            CallSetting();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            CallSetting();
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (busPK.getMotorFree(ParkingID) == 0 && busPK.getCarFree(ParkingID)==0)
            {
                Error(7);
            }
            else if (txtCardNo.Text != "")
            {
                lblCardNo.Text = txtCardNo.Text;
                txtCardNo.Text = "";
                autoCapture();
               
            }else {
                autoCapture();
            }
            
        }


        //Get info from Setting
        private void CallSetting()
        {
            using (SettingForm form2 = new SettingForm())
            {
                if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    GateID = form2.SelectedGate;
                    ParkingID = form2.ParkingID;
                    if (GateID == 0)
                    {
                        lblGate.Text = "Cổng vào";
                    }
                    else
                    {
                        lblGate.Text = "Cổng ra";
                    }
                    groupPanel1.Text = "Bãi đỗ xe " + form2.ParkingName;
                    lblCar.Text = busPK.getCarFree(ParkingID).ToString();
                    lblMotor.Text = busPK.getMotorFree(ParkingID).ToString();
                }
            }
            StartTimer();
            if (CAMS.Count > 0)
            {
                startCamera();
            }
        }
        //start camera
        private void startCamera()
        {
            if (CAM != null && CAM.IsRunning)
            {
                CAM.Stop();
            }
            CAM = new VideoCaptureDevice(CAMS[toolStripComboBox1.SelectedIndex].MonikerString);
            CAM.NewFrame += CAM_NewFrame;
            CAM.Start();
        }
        private void CAM_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            BMP = (Bitmap)eventArgs.Frame.Clone();
            pictureBox_WC.Image = BMP;
            //   throw new NotImplementedException();
        }

        //Timenow
        private void StartTimer()
        {
            tmr = new System.Windows.Forms.Timer();
            tmr.Interval = 1000;
            tmr.Tick += new EventHandler(tmr_Tick);
            tmr.Enabled = true;
        }

        void tmr_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("hh:mm:ss tt - dd/MM/yyyy");
        }

        //capture photo
        private void CapturePhoto()
        {
            pictureBox1.Image = pictureBox_WC.Image;
            if (System.IO.File.Exists(m_path + "aa.bmp")) //xoa file aa.bmp neu file da ton tai
            {
                System.IO.File.Delete(m_path + "aa.bmp");
            }
            pictureBox1.Image.Save(m_path + "aa.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            

        }

        //kiem tra card
        private void checkCard()
        {
            
            if (busCard.checkCard(lblCardNo.Text) == false)
            {
                Error(2);
                chkCard = false;
            }
            else if (GateID == 0 && busCard.getCardStatus(lblCardNo.Text) == 1)
            {
                Error(3);
                chkCard = false;
            }
            else if (GateID == 0 && busCard.getCardStatus(lblCardNo.Text) == 2)
            {
                Error(4);
                chkCard = false;
            }
            else
            {
                chkCard = true;
            }
        }

        //kiem tra bien so xe
        private void checkLicense()
        {
            
            if (busTicket.checkLicense(txtLicense.Text) == true)
            {
                lblTicket.Text = "Vé tháng";
                lblName.Text = busTicket.getName(txtLicense.Text);
                if (DateTime.Parse(busTicket.getExpiryDate(txtLicense.Text)) < DateTime.Now)
                {
                    //lblTimeOut.Text = busTicket.getExpiryDate(txtLicense.Text);
                    chkLicense = false;
                    
                }
                
            }else 
            {
                lblTicket.Text = "Vé ngày/bloc";
                chkLicense = true;
            }
        }

        //su kien kiem tra hinh anh khi quet the hoac nhan nut Capture
        private void autoCapture()
        {
            checkCard();
            if (chkCard == true)
            {
                GetVehicleInfo();
                checkLicense();
                if (chkLicense == true)
                {
                    if (vehicleType == 0)
                    {
                        if (busPK.getMotorFree(ParkingID) > 0)
                        {
                            
                        }else
                        {
                            Error(7);
                        }
                    }
                    else
                    {
                        if (busPK.getCarFree(ParkingID) > 0)
                        {
                            pictureLink = UploadImageToImageShack(m_path + "aa.bmp");
                            lblTimeOut.Text = pictureLink;
                        }
                        else
                        {
                            Error(7);
                        }
                    }
                }
                else
                {
                    Error(5);
                }
            }
        }

        //upload anh len imageshack
        public string UploadImageToImageShack(string imageAddress)
        {
            string _baseUri = "https://api.imageshack.com/v2/images";
            string key = "JHUV8MSZf125d77eb0025b3991e46e175590215d";

            WebClient w = new WebClient();
            w.Headers.Add("Authorization", "Client-ID " + key);
            w.Headers.Add("Authorization", "key" + key);
            w.Headers.Set("Content-Type", "multipart/form-data");

            try
            {
                string kaka = string.Format("api_key={0}&file={1}&type=base64&public=yes", key, Convert.ToBase64String(File.ReadAllBytes(imageAddress)));

                string responseArray = w.UploadString(_baseUri, kaka);

                dynamic result = responseArray;// Encoding.ASCII.GetString(responseArray);
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("link\":\"(.*?)\"");
                Match match = reg.Match(result);
                string url = match.ToString().Replace("link\":\"", "").Replace("\"", "").Replace("\\/", "/");
                return "http://" + url;
            }
            catch (Exception s)
            {
                MessageBox.Show("Something went wrong. " + s.Message); return "Failed!";
            }



        }
        //phan tich hinh anh
        private void GetVehicleInfo()
        {
            reset();
            bool checkxe = true;
            CapturePhoto();
            string link = m_path + "aa.bmp";
            ProcessImage(link);
            if (PlateImagesList.Count != 0)
            {
                Image<Bgr, byte> src = new Image<Bgr, byte>(PlateImagesList[0].ToBitmap());
                Bitmap grayframe;
                Detect con = new Detect();
                Bitmap color;
                // MessageBox.Show("1");
                int c = con.IdentifyContours(src.ToBitmap(), 50, false, out grayframe, out color, out listRect);
                // MessageBox.Show("2");
                //int z = con.count;
                //pictureBox6.Image = color;
                //pictureBox3.Image = grayframe;
                // MessageBox.Show("3");
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
                    string temp = "";
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
                    box[i].Location = new System.Drawing.Point(x + i * 50, 0);
                    box[i].Size = new Size(50, 100);
                    box[i].SizeMode = PictureBoxSizeMode.StretchImage;
                    box[i].Image = ch;
                    //panel1.Controls.Add(box[i]);
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
                    box[i + c_x].Location = new System.Drawing.Point(x + i * 50, 100);
                    box[i + c_x].Size = new Size(50, 100);
                    box[i + c_x].SizeMode = PictureBoxSizeMode.StretchImage;
                    box[i + c_x].Image = ch;

                    //panel1.Controls.Add(box[i + c_x]);
                }
                #region hienthi
                
                lblCardNumber.Text = lblCardNo.Text;
                if (GateID == 0)
                {
                    lblTimeIn.Text = DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy");
                    //textBox2.Text = DateTime.Now.ToString("dd/MM/yyyy ");
                }

                if (checkxe == true)
                {
                    lblVehicle.Text = "Xe Ôtô";
                    vehicleType = 1;
                }
                else
                {
                    lblVehicle.Text = "Xe Máy";
                    vehicleType = 0;
                }
                #endregion

                bienso.Trim();
                do
                {
                    bienso = bienso.Replace("\n", "");
                } while (bienso.IndexOf("\n") != -1);
                lblLicense.Text = bienso;
                txtLicense.Text = bienso;

                //string Str1 = bienso.Substring(0, 2); //lay 2 chu so dau cua bien so de xac dinh tinh / thanh pho
                //int n = int.Parse(Str1);             


            }
        }
        public void ProcessImage(string urlImage)
        {
            PlateImagesList.Clear();
            PlateTextList.Clear();

            Bitmap img = new Bitmap(pictureBox1.Image);
            //using (FileStream fs = new FileStream(m_path + "aa.bmp", FileMode.Open))
            //{
            //    pictureBox1.Image = Image.FromStream(fs);
            //    fs.Close();
            //}
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
        public void FindLicensePlate(Bitmap image) //tim vi tri bien so xe trong hinh anh
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




                   // pictureBox6.Image = tmp.ToBitmap();
                   // pictureBox6.Update();




                    //string pl = this.Ocr(tmp2.ToBitmap());

                    //PlateTextList.Add(pl);
                }

                Image<Bgr, Byte> showimg = new Image<Bgr, Byte>(image.Size);
                //showimg = frame.Resize(imageBox1.Width, imageBox1.Height, 0);
                //imageBox1.Image = showimg;

            }

        }

        private void txtCardNo_TextChanged(object sender, EventArgs e) //su kien quet the
        {

        }

        //xoa thong tin xe dang hien thi
        public void reset()
        {
            //pictureBox1.Image = null;
            //pictureBox3.Image = null;
            //pictureBox6.Image = null;
            //imageBox1.Image = null;
            //panel1.Controls.Clear();
            cleanError();
            lblVehicle.Text = "";
            lblLicense.Text = "";
            lblCost.Text = "VND";
            lblTimeIn.Text = "";
            lblTimeOut.Text = "";
            lblTicket.Text = "";
            lblName.Text = "";
            //lblCardNumber.Text = "";
            txtLicense.Text = "";
        } 

        //Thong bao
        public void Error(int a)
        {
            labelX11.BackColor = Color.Red;
            lblCost.BackColor = Color.Red;
            labelX11.Text = "Lỗi:";
            lblCost.Text = mes.mes(a);
        }
        public void Passed(int a)
        {
            labelX11.BackColor = Color.CadetBlue;
            lblCost.BackColor = Color.CadetBlue;
            labelX11.Text = "Thông qua:";
            lblCost.Text = mes.mes(a);
        }
        public void cleanError()
        {
            labelX11.BackColor = Color.White;
            lblCost.BackColor = Color.White;
            labelX11.Text = "Số tiền:";
            lblCost.Text = "VND";
        }
    }
}
