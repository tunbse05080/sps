extern alias dess;
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
using System.Threading;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace SPS
{
    public partial class MainForm : Form
    {
        #region
        private VideoCaptureDevice CAM;
        private Bitmap BMP;
        private FilterInfoCollection CAMS;
        dess::Emgu.CV.Capture cameraCapture;
        System.Windows.Forms.Timer tmr = null;
        BUS_ParkingPlace busPK = new BUS_ParkingPlace();
        BUS_Card busCard = new BUS_Card();
        BUS_MonthlyTicket busTicket = new BUS_MonthlyTicket();
        BUS_Transaction busTrans = new BUS_Transaction();
        BUS_Image busImage = new BUS_Image();
        BUS_Price busPrice = new BUS_Price();
        BUS_User busUser = new BUS_User();
        Messages mes = new Messages();
        public int CarFree { get; set; }
        public int MotorFree { get; set; }
        public int ParkingID { get; set; }
        private int accountID;
        //private int userID;
        private int GateID;
        private int CardID;
        private int TransID;
        private double price;
        private int ImageID;
        private int enterMethod; // 1-nhap bien so bang tay, 0-kiem tra bien so tu dong
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
        bool expiredTicket = false; //ve thang het han hoac ve thang khong dung bai
        int vehicleType; //loai xe 0-xemay, 1-oto
        int ticketType; //loai ve 0-ve thang, 1-vengay/block
        //int timeOfblock = 2; //thoi gian moi block (gio) 
        int working = 0; //trang thai lam viec.0-khong lam viec, 1-lamviec
        string pictureLink; //duong dan hinh anh up len imageshark
        private string url; //duong dan stream
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
                toolStripComboBox1.Text = "Không tìm thấy camera";
                string message = "Không tìm thấy camera. Kết nối với camera!";
                string caption = "Không tìm thấy camera";
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
            txtCardNo.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(mes.mes(1), "Hỏi Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (CAM != null && CAM.IsRunning)
                {
                    CAM.Stop();
                }
                if (cameraCapture != null)
                {
                    cameraCapture.Stop();
                }
                if (working == 1)
                {
                    updateUser(1);
                }               
                Application.Exit();
            }
            txtCardNo.Focus();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            pictureBox_WC.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox_WC.Height = pictureBox_WC.Width / 16 * 9;
            pictureBox1.Height = pictureBox1.Width / 16 * 9;
            toolStripComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            txtLicense1.CharacterCasing = CharacterCasing.Upper;
            txtLicense2.CharacterCasing = CharacterCasing.Upper;
            //imageBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            //imageBox1.Width = imageBox1.Height / 9 * 16;
            if (working == 0)
            {
                btnCapture.Enabled = false;
                btnEnter.Enabled = false;
                btnLogin.Text = "Đăng nhập";
            }
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
            txtCardNo.Focus();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            CallSetting();
            txtCardNo.Focus();
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (working == 0)
            {
                Error(13);
                return;
            }
            if (busPK.getMotorFree(ParkingID) == 0 && busPK.getCarFree(ParkingID) == 0)
            {
                Error(7);
            }
            else if (txtCardNo.Text != "")
            {
                lblCardNo.Text = txtCardNo.Text;
                txtCardNo.Text = "";
                if (enterMethod == 0)
                {
                    autoCapture();
                }
                else
                {
                    GetVehicleInfo();
                }


            }
            else
            {
                if (enterMethod == 0)
                {
                    autoCapture();
                }
                else
                {
                    GetVehicleInfo();
                }
            }
            txtCardNo.Focus();
        }

        
        //Get info from Setting
        private void CallSetting()
        {
            SettingForm form2 = new SettingForm();
            form2.working = working;
            if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GateID = form2.SelectedGate;
                ParkingID = form2.ParkingID;
                enterMethod = form2.EnterMethod;
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

            StartTimer();
            if (CAMS.Count > 0)
            {
                startCamera();
            }
            else
            {
                startStream();
            }
            reset();
        }
        //Get info from LoginForm
        private void CallLogin()
        {
            LoginForm form2 = new LoginForm();
            form2.parkingID = ParkingID;
            if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                accountID = form2.accountID;
                //userID = form2.userID;
                working = 1;
                updateUser(0);
                btnLogin.Text = "Đăng xuất";
                btnCapture.Enabled = true;
                btnEnter.Enabled = true;
                lblSecureName.Text = busUser.getNamebyAccountID(accountID);
            }

        }
        //start webcam camera
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

        //start Camera IP
        private void startStream()
        {
            if (cameraCapture != null)
            {
                cameraCapture.Stop();
            }
            try
            {
                cameraCapture = new dess::Emgu.CV.Capture("rtsp://admin:DQQHRY@192.168.31.88:554");
                cameraCapture.SetCaptureProperty(dess::Emgu.CV.CvEnum.CapProp.FrameWidth, 1280);
                cameraCapture.SetCaptureProperty(dess::Emgu.CV.CvEnum.CapProp.FrameHeight, 720);
                cameraCapture.SetCaptureProperty(dess::Emgu.CV.CvEnum.CapProp.Fps, 15);
                cameraCapture.ImageGrabbed += ProcessFrame;
                cameraCapture.Start();
            }
            catch (Exception)
            {
                string message = "Không tìm thấy camera. Kết nối với camera!";
                string caption = "Không tìm thấy camera";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(this, message, caption, buttons);

                if (result == DialogResult.Yes)
                {

                    // Closes the parent form.
                    Application.Exit();

                }
                throw;
            }
        }
        private void ProcessFrame(object sender, EventArgs arg)
        {
            dess::Emgu.CV.Mat image = new dess::Emgu.CV.Mat();
            dess::Emgu.CV.Mat frame_copy = new dess::Emgu.CV.Mat();
            cameraCapture.Retrieve(image);
            if (image != null)
            {
                frame_copy = image;
                pictureBox_WC.Image = frame_copy.ToImage<dess::Emgu.CV.Structure.Bgr, byte>().Bitmap;
                FreeMemory(image);
            }
            else
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                // get response
                WebResponse resp = req.GetResponse();
                //get stream
                Stream stream = resp.GetResponseStream();
                if (!stream.CanRead)
                {
                    //try reconnecting the camera
                    captureButtonClick(null, null); //pause
                    cameraCapture.Dispose();//get rid
                    captureButtonClick(null, null); //reconnect
                }
            }


        }
        private void captureButtonClick(object sender, EventArgs e)
        {
            url = "rtsp://admin:DQQHRY@192.168.31.88:554"; //add this
                                                           //... the rest of the code
        }
        private void FreeMemory(dess::Emgu.CV.Mat image)
        {
            using (image)
            {
                using (image.Bitmap)
                {

                }
            }
//image.Bitmap.Dispose();
//image.Dispose()
;
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
            pictureBox1.Image = FixedSize(pictureBox_WC.Image, 533, 300);
            if (System.IO.File.Exists(m_path + "aa.bmp")) //xoa file aa.bmp neu file da ton tai
            {
                System.IO.File.Delete(m_path + "aa.bmp");
            }
            pictureBox1.Image.Save(m_path + "aa.bmp", System.Drawing.Imaging.ImageFormat.Bmp);


        }
        //resize photo
        static Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Red);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        //kiem tra card 0-chua su dung, 1-da dang ky ve thang,2-hong,3-khoa,4-dang su dung
        private void checkCard()
        {

            if (busCard.checkCard(lblCardNo.Text) == false)
            {
                Error(2);
                chkCard = false;
                return;
            }
            else if (GateID == 0 && busCard.getCardStatus(lblCardNo.Text) == 4)
            {
                Error(3);
                chkCard = false;
                return;
            }
            else if (busCard.getCardStatus(lblCardNo.Text) == 3)
            {
                Error(4);
                chkCard = false;
                return;
            }
            else if (busCard.getCardStatus(lblCardNo.Text) == 2)
            {
                Error(4);
                chkCard = false;
                return;
            }
            else if (GateID == 1 && (busCard.getCardStatus(lblCardNo.Text) == 0 || busCard.getCardStatus(lblCardNo.Text) == 1))
            {
                Error(11);
                chkCard = false;
                return;
            }
            else
            {
                chkCard = true;
                CardID = busCard.getCardID(lblCardNo.Text);
            }
        }

        //kiem tra bien so xe
        private void checkLicense()
        {
            chkLicense = true;
            TransID = 0;
            if (txtLicense1.Text.Length < 3 || lblLicense.Text.Length < 6)
            {
                chkLicense = false;
                Error(8);
                return;
            }

            if (busTrans.checkLicense(lblLicense.Text) == true)
            {
                //lblTicket.Text = "haha";
                // lblTimeOut.Text = busTrans.checkLicenseTimeOut(lblLicense.Text);

                if (GateID == 0 && busTrans.checkLicenseTimeOut(lblLicense.Text) == "")
                {
                    //lblTicket.Text = "hahaha";
                    chkLicense = false;
                    Error(9);
                    return;
                }
                else if (GateID == 1 && busTrans.checkLicenseTimeOut(lblLicense.Text) != "")
                {
                    chkLicense = false;
                    Error(10);
                    return;
                }
                else
                {
                    TransID = busTrans.getTransactionID(lblLicense.Text);
                    if (GateID == 1 && (busCard.getCardID(lblCardNo.Text) != busTrans.getCardID(TransID) || ParkingID != busTrans.getParkingID(TransID)))
                    {
                        chkLicense = false;
                        Error(11);
                        return;
                    }
                }
                checkTicketType();
            }

            if (GateID == 0 && busTrans.checkLicense(lblLicense.Text) == false)
            {
                checkTicketType();
            }
            if (GateID == 1 && busTrans.checkLicense(lblLicense.Text) == false)
            {
                chkLicense = false;
                Error(10);
                return;
            }
        }

        //phan loai ve thang, ve ngay/block
        private void checkTicketType()
        {
            expiredTicket = false;
            if (busTicket.checkLicense(lblLicense.Text) == true)
            {

                if (busCard.getCardID(lblCardNo.Text) != busTicket.getCardIDbyLicense(lblLicense.Text))
                {

                    Error(11);
                    return;
                }
                if (busTicket.getParkingIDbyLicense(lblLicense.Text) != ParkingID) //ve thang gui khong dung bai, tinh nhu ve ngay
                {
                    chkLicense = true;
                    expiredTicket = true;
                    lblTicket.Text = "Vé tháng không đúng bãi";
                    lblTicket.BackColor = Color.Yellow;
                    Warning(5);
                    return;
                }
                lblTicket.Text = "Vé tháng";
                ticketType = 0;
                lblName.Text = busTicket.getName(lblLicense.Text);
                if (DateTime.Parse(busTicket.getExpiryDate(lblLicense.Text)) < DateTime.Now || DateTime.Parse(busTicket.getStartDate(lblLicense.Text)) > DateTime.Now)
                {
                    //lblTimeOut.Text = busTicket.getExpiryDate(txtLicense.Text);
                    chkLicense = true; //ve thang het han, tinh tien nhu ve ngay, block
                    expiredTicket = true;
                    lblTicket.Text = "Vé tháng hết hạn";
                    lblTicket.BackColor = Color.Yellow;
                    Warning(5);
                    return;
                }
                else
                {
                    chkLicense = true;
                    Passed(6);
                    return;
                }

            }
            else if (busTicket.checkLicense(lblLicense.Text) == false)
            {
                if (busCard.getCardStatus(lblCardNo.Text) == 1) //dung ve thang nhung xe chua dang ky ve thang
                {
                    chkLicense = false;
                    Error(15);
                    return;
                }
                else
                {
                    lblTicket.Text = "Vé ngày/block";
                    ticketType = 1;
                    chkLicense = true;
                    Passed(6);
                }               
            }
        }

        //insert/update bang Transaction, Table khi quet the hoac nhan nut Capture
        private void autoCapture()
        {
            checkCard();
            if (chkCard == true)
            {
                GetVehicleInfo();
                checkLicense();
                if (chkLicense == true)
                {
                    if (GateID == 0) //cong vao
                    {

                        if (vehicleType == 0)
                        {
                            if (busPK.getMotorFree(ParkingID) > 0)
                            {
                                lblTimeIn.Text = DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy");
                                insertTransaction();
                                TransID = busTrans.getTransactionID(lblLicense.Text);
                                updateCard(4);
                                updateMotoFree();
                                new Thread(() =>
                                {
                                    Thread.CurrentThread.IsBackground = true;
                                    /* run your code here */
                                    pictureLink = UploadImageToImageShack(m_path + "aa.bmp");
                                    insertImage();
                                    //Console.WriteLine("Hello, world");
                                }).Start();

                            }
                            else
                            {
                                Error(7);
                            }
                        }
                        else
                        {
                            if (busPK.getCarFree(ParkingID) > 0)
                            {
                                lblTimeIn.Text = DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy");
                                insertTransaction();
                                TransID = busTrans.getTransactionID(lblLicense.Text);
                                updateCard(4);
                                updateCarFree();
                                new Thread(() =>
                                {
                                    Thread.CurrentThread.IsBackground = true;
                                    /* run your code here */
                                    pictureLink = UploadImageToImageShack(m_path + "aa.bmp");
                                    insertImage();
                                    //Console.WriteLine("Hello, world");
                                }).Start();

                            }
                            else
                            {
                                Error(7);
                            }
                        }
                    }
                    else //cong  ra
                    {
                        if (vehicleType == 0)
                        {
                            TransID = busTrans.getTransactionID(lblLicense.Text);
                            lblTimeIn.Text = DateTime.Parse(busTrans.getTimeInbyTransID(TransID)).ToString("hh:mm:ss tt dd/MM/yyyy");
                            ImageID = busImage.getImageID(TransID);
                            totalPrice();
                            updateTransaction();
                            if (ticketType == 1)
                            {
                                updateCard(0);
                            }
                            if (ticketType == 0)
                            {
                                updateCard(1);
                            }
                            updateMotoFree();
                            new Thread(() =>
                            {
                                Thread.CurrentThread.IsBackground = true;
                                /* run your code here */
                                pictureLink = UploadImageToImageShack(m_path + "aa.bmp");
                                updateImage();
                                //Console.WriteLine("Hello, world");
                            }).Start();

                        }
                        else
                        {
                            TransID = busTrans.getTransactionID(lblLicense.Text);
                            lblTimeIn.Text = DateTime.Parse(busTrans.getTimeInbyTransID(TransID)).ToString("hh:mm:ss tt dd/MM/yyyy");
                            ImageID = busImage.getImageID(TransID);
                            totalPrice();
                            updateTransaction();
                            if (ticketType == 1)
                            {
                                updateCard(0);
                            }
                            if (ticketType == 0)
                            {
                                updateCard(1);
                            }
                            updateCarFree();
                            new Thread(() =>
                            {
                                Thread.CurrentThread.IsBackground = true;
                                /* run your code here */
                                pictureLink = UploadImageToImageShack(m_path + "aa.bmp");
                                updateImage();
                                //Console.WriteLine("Hello, world");
                            }).Start();

                        }
                    }

                }
                else
                {

                }
            }
        }

        ////insert/update bang Transaction, Table khi nhan nut Nhap
        private void manualEnter()
        {

            checkCard();
            if (chkCard == true)
            {
                //CapturePhoto();
                checkLicense();
                if (chkLicense == true)
                {
                    if (GateID == 0) //cong vao
                    {

                        if (vehicleType == 0)
                        {
                            if (busPK.getMotorFree(ParkingID) > 0)
                            {
                                lblTimeIn.Text = DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy");
                                insertTransaction();
                                TransID = busTrans.getTransactionID(lblLicense.Text);
                                updateCard(4);
                                updateMotoFree();
                                new Thread(() =>
                                {
                                    Thread.CurrentThread.IsBackground = true;
                                    /* run your code here */
                                    pictureLink = UploadImageToImageShack(m_path + "aa.bmp");
                                    insertImage();
                                    //Console.WriteLine("Hello, world");
                                }).Start();
                            }
                            else
                            {
                                Error(7);
                            }
                        }
                        else
                        {
                            if (busPK.getCarFree(ParkingID) > 0)
                            {
                                lblTimeIn.Text = DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy");
                                insertTransaction();
                                TransID = busTrans.getTransactionID(lblLicense.Text);
                                updateCard(4);
                                updateCarFree();
                                new Thread(() =>
                                {
                                    Thread.CurrentThread.IsBackground = true;
                                    /* run your code here */
                                    pictureLink = UploadImageToImageShack(m_path + "aa.bmp");
                                    insertImage();
                                    //Console.WriteLine("Hello, world");
                                }).Start();

                            }
                            else
                            {
                                Error(7);
                            }
                        }
                    }
                    else //cong  ra
                    {
                        if (vehicleType == 0)
                        {

                            TransID = busTrans.getTransactionID(lblLicense.Text);
                            ImageID = busImage.getImageID(TransID);
                            totalPrice();
                            updateTransaction();
                            if (ticketType == 1)
                            {
                                updateCard(0);
                            }
                            if (ticketType == 0)
                            {
                                updateCard(1);
                            }
                            updateMotoFree();
                            lblTimeIn.Text = DateTime.Parse(busTrans.getTimeInbyLicense(lblLicense.Text)).ToString("hh:mm:ss tt dd/MM/yyyy");
                            lblTimeOut.Text = DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy");
                            new Thread(() =>
                            {
                                Thread.CurrentThread.IsBackground = true;
                                /* run your code here */
                                pictureLink = UploadImageToImageShack(m_path + "aa.bmp");
                                updateImage();
                                //Console.WriteLine("Hello, world");
                            }).Start();

                        }
                        else
                        {

                            TransID = busTrans.getTransactionID(lblLicense.Text);
                            ImageID = busImage.getImageID(TransID);
                            totalPrice();
                            updateTransaction();
                            if (ticketType == 1)
                            {
                                updateCard(0);
                            }
                            if (ticketType == 0)
                            {
                                updateCard(1);
                            }
                            updateCarFree();
                            lblTimeIn.Text = DateTime.Parse(busTrans.getTimeInbyLicense(lblLicense.Text)).ToString("hh:mm:ss tt dd/MM/yyyy");
                            lblTimeOut.Text = DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy");
                            new Thread(() =>
                            {
                                Thread.CurrentThread.IsBackground = true;
                                /* run your code here */
                                pictureLink = UploadImageToImageShack(m_path + "aa.bmp");
                                updateImage();
                                //Console.WriteLine("Hello, world");
                            }).Start();

                        }
                    }

                }
                else
                {

                }
            }
        }

        //tinh tien
        private void totalPrice()
        {
            price = 0;
            DateTime TimeIn = DateTime.Parse(busTrans.getTimeInbyLicense(lblLicense.Text));
            if (expiredTicket == true)
            {
                if (DateTime.Parse(busTicket.getExpiryDate(lblLicense.Text)) > TimeIn)
                {
                    TimeIn = DateTime.Parse(busTicket.getExpiryDate(lblLicense.Text));
                }

            }
            DateTime TimeOut = DateTime.Now;
            //lay thong tin block
            int timeFirstBlock = busPrice.gettimeFirstBlock(ParkingID, vehicleType);
            int timeNextBlock = busPrice.gettimeNextBlock(ParkingID, vehicleType);

            //tinh thoi gian gui xe
            int numberOfDays = Convert.ToInt32(Math.Ceiling((TimeOut - TimeIn).TotalDays));
            int numberOfhours = Convert.ToInt32(Math.Ceiling((TimeOut - TimeIn).TotalHours));
            int numberOfBlocks = 0;
            //lay thong tin gia ve
            double dailyPrice = busPrice.getDailyPrice(ParkingID, vehicleType);
            if (Convert.ToInt32(dailyPrice) == 0)
            {
                numberOfBlocks = ((numberOfhours - timeFirstBlock) > 0) ? 1 + Convert.ToInt32(Math.Ceiling((Convert.ToDouble(numberOfhours - timeFirstBlock) / Convert.ToDouble(timeNextBlock)))) : 1;
            }
            double firstBlockPrice = busPrice.getFirstBlockPrice(ParkingID, vehicleType);
            double nextBlockPrice = busPrice.getNextBlockPrice(ParkingID, vehicleType);
            if (ticketType == 0 && expiredTicket == false)
            {
                price = 0;
                lblTotalTime.Text = numberOfDays.ToString() + " ngày";
            }
            if (ticketType == 1 || expiredTicket == true)
            {
                price = (Convert.ToInt32(dailyPrice) == 0) ? firstBlockPrice + (numberOfBlocks - 1) * nextBlockPrice : numberOfDays * dailyPrice;
                lblTotalTime.Text = (Convert.ToInt32(dailyPrice) == 0) ? numberOfBlocks.ToString() + " block" : numberOfDays.ToString() + " ngày";
            }
            labelX11.BackColor = Color.Yellow;
            lblCost.BackColor = Color.Yellow;
            labelX11.Text = "Số tiền:";
            lblCost.Text = price.ToString() + "VND";

        }

        //upload anh len imageshack
        private string UploadImageToImageShack(string imageAddress)
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

        //insert bang Transaction
        private void insertTransaction()
        {
            //Tao DTO
            //int userID = 0;
            //DTO_Transaction trans = new DTO_Transaction(0, lblTimeIn.Text, "", lblLicense.Text, ticketType, 0, CardID, ParkingID, vehicleType);
            DTO_Transaction trans = new DTO_Transaction(0, DateTime.Now.ToString(), lblLicense.Text, ticketType, CardID, ParkingID, vehicleType, accountID);
            // Them
            if (busTrans.insertTransaction(trans))
            {
                //MessageBox.Show("Thêm thành công");
            }
            else
            {
                MessageBox.Show("Thêm ko thành công");
                Error(100);
            }
        }

        //update bang Transaction
        private void updateTransaction()
        {
            //Tao DTO
            //int userID = 0;
            DTO_Transaction trans = new DTO_Transaction(TransID, DateTime.Now.ToString(), price, accountID);
            // Sửa
            if (busTrans.updateTransaction(trans))
            {
                //  MessageBox.Show("Sửa thành công");
            }
            else
            {
                // MessageBox.Show("Sửa ko thành công");
                Error(100);
            }
        }

        //update bang Card
        private void updateCard(int status)
        {
            DTO_Card card = new DTO_Card(CardID, status);
            if (busCard.updateCard(card))
            {
                // MessageBox.Show("Sửa thành công");
            }
            else
            {
                //MessageBox.Show("Sửa ko thành công");
                Error(100);
            }
        }
        //update bang user
        private void updateUser(int status)
        {
            DTO_User user = new DTO_User(accountID, status);
            if (busUser.updateUser(user))
            {

            }
            else
            {
                Error(100);
            }
        }
        //insert bang Image
        private void insertImage()
        {
            //Tao DTO
            //DTO_Transaction trans = new DTO_Transaction(0, lblTimeIn.Text, "", lblLicense.Text, ticketType, 0, CardID, ParkingID, vehicleType);
            DTO_Image image = new DTO_Image(0, pictureLink, TransID);
            // Them
            if (busImage.insertImage(image))
            {
                //  MessageBox.Show("Thêm thành công");

            }
            else
            {
                //   MessageBox.Show("Thêm ko thành công");
                Error(100);
            }
        }

        //update bang Image
        private void updateImage()
        {
            DTO_Image image = new DTO_Image(ImageID, pictureLink);
            if (busImage.updateImage(image))
            {
                //  MessageBox.Show("Sửa thành công");
            }
            else
            {
                //  MessageBox.Show("Sửa ko thành công");
                Error(100);
            }
        }

        //update bang ParkingPlace
        private void updateCarFree()
        {
            DTO_ParkingPlace parking = new DTO_ParkingPlace();
            if (GateID == 0)
            {
                parking = new DTO_ParkingPlace(ParkingID, busPK.getCarFree(ParkingID) - 1, 0);
            }
            else
            {
                parking = new DTO_ParkingPlace(ParkingID, busPK.getCarFree(ParkingID) + 1, 0);
            }
            if (busPK.updateCarParking(parking))
            {
                // MessageBox.Show("Sửa thành công");
                lblCar.Text = busPK.getCarFree(ParkingID).ToString();

            }
            else
            {
                // MessageBox.Show("Sửa ko thành công");
                Error(100);
            }
        }
        private void updateMotoFree()
        {
            int MotoSlot = busPK.getMotorFree(ParkingID);
            DTO_ParkingPlace parking = new DTO_ParkingPlace();
            if (GateID == 0)
            {
                parking = new DTO_ParkingPlace(ParkingID, 0, MotoSlot - 1);
            }
            else
            {
                parking = new DTO_ParkingPlace(ParkingID, 0, MotoSlot + 1);
            }
            if (busPK.updateMotoParking(parking))
            {
                // MessageBox.Show("Sửa thành công");
                lblMotor.Text = busPK.getMotorFree(ParkingID).ToString();
            }
            else
            {
                //  MessageBox.Show("Sửa ko thành công");
                Error(100);
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
                string license1 = "";
                string license2 = "";
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
                    license1 += temp;
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
                    license2 += temp;
                    box[i + c_x].Location = new System.Drawing.Point(x + i * 50, 100);
                    box[i + c_x].Size = new Size(50, 100);
                    box[i + c_x].SizeMode = PictureBoxSizeMode.StretchImage;
                    box[i + c_x].Image = ch;

                    //panel1.Controls.Add(box[i + c_x]);
                }
                #region hienthi

                lblCardNumber.Text = lblCardNo.Text;
                //if (GateID == 0)
                //{
                //    lblTimeIn.Text = DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy");
                //    //textBox2.Text = DateTime.Now.ToString("dd/MM/yyyy ");
                //}
                //if (GateID == 1)
                //{
                //    //lblTimeIn.Text = DateTime.Parse(busTrans.getTimeInbyTransID(TransID)).ToString("hh:mm:ss tt dd/MM/yyyy");
                //    lblTimeOut.Text = DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy");
                //}
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
                license1.Trim();
                do
                {
                    license1 = license1.Replace("\n", "");
                } while (license1.IndexOf("\n") != -1);
                license2.Trim();
                do
                {
                    license2 = license2.Replace("\n", "");
                } while (license2.IndexOf("\n") != -1);

                lblLicense.Text = bienso;
                txtLicense1.Text = license1;
                txtLicense2.Text = license2;

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

        //su kien quet the
        private void txtCardNo_TextChanged(object sender, EventArgs e)
        {
            if (txtCardNo.Text.Length == 10)
            {
                if (working == 0)
                {
                    Error(13);
                    txtCardNo.Clear();
                    return;
                }
                lblCardNo.Text = txtCardNo.Text;
                txtCardNo.Clear();
                txtCardNo.Focus();
                if (busPK.getMotorFree(ParkingID) == 0 && busPK.getCarFree(ParkingID) == 0)
                {
                    Error(7);
                }
                else if (enterMethod == 0)
                {
                    autoCapture();
                }
                else
                {
                    GetVehicleInfo();
                }
            }
        }
        //hien thi thong tin
        private void showInformation()
        {
            //checkLicense();
            lblLicense.Text = txtLicense1.Text + txtLicense2.Text;
            lblCardNumber.Text = lblCardNo.Text;
            //if (GateID == 0)
            //{
            //    lblTimeIn.Text = DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy");
            //    //textBox2.Text = DateTime.Now.ToString("dd/MM/yyyy ");
            //}

            if (txtLicense1.Text.Length == 3)
            {
                lblVehicle.Text = "Xe Ôtô";
                vehicleType = 1;
            }
            else if (txtLicense1.Text.Length == 4)
            {
                lblVehicle.Text = "Xe Máy";
                vehicleType = 0;
            }
            else
            {
                lblVehicle.Text = "";
                vehicleType = 0;
            }

            //if (GateID == 1)
            //{
            //    lblTimeIn.Text = DateTime.Parse(busTrans.getTimeInbyLicense(lblLicense.Text)).ToString("hh:mm:ss tt dd/MM/yyyy");
            //    lblTimeOut.Text = DateTime.Now.ToString("hh:mm:ss tt dd/MM/yyyy");
            //}
        }

        //nhap bien so  xe bang  tay khi click nut Nhap
        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (working == 0)
            {
                Error(13);
                return;
            }
            //chkLicense = true;
            showInformation();
            manualEnter();
            txtCardNo.Focus();
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
            txtLicense1.Text = "";
            txtLicense2.Text = "";
            lblTotalTime.Text = "";
            lblTimeIn.Text = "";
            lblTimeOut.Text = "";
        }

        //Thong bao
        private void Error(int a)
        {
            labelX11.BackColor = Color.Red;
            lblCost.BackColor = Color.Red;
            labelX11.Text = "Lỗi:";
            lblCost.Text = mes.mes(a);
        }
        private void Passed(int a)
        {
            labelX11.BackColor = Color.White;
            lblCost.BackColor = Color.White;
            labelX11.Text = "Thông qua:";
            lblCost.Text = mes.mes(a);
        }
        private void Warning(int a)
        {
            labelX11.BackColor = Color.Yellow;
            lblCost.BackColor = Color.Yellow;
            labelX11.Text = "Cảnh báo:";
            lblCost.Text = mes.mes(a);
        }
        private void cleanError()
        {
            labelX11.BackColor = Color.White;
            lblCost.BackColor = Color.White;
            lblTicket.BackColor = Color.White;
            labelX11.Text = "Số tiền:";
            lblCost.Text = "VND";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            txtCardNo.Focus();
            if (working == 0)
            {
                CallLogin();

                return;
            }
            if (working == 1)
            {
                if (MessageBox.Show(mes.mes(14), "Hỏi đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    working = 0;
                    updateUser(1);
                    btnCapture.Enabled = false;
                    btnEnter.Enabled = false;
                    btnLogin.Text = "Đăng nhập";
                    lblSecureName.Text = "_ _ _";
                    return;
                }
            }
        }
        int a = 0;
        private void MainForm_KeyDown(object sender, KeyEventArgs e) //su kien phim tat
        {           
            if (e.KeyCode == Keys.Enter)
            {
                if (a==0)
                {
                    txtLicense1.Select();
                    txtLicense1.BackColor = Color.Yellow;
                    txtLicense2.BackColor = Color.White;
                    a = 1;
                }
                else
                {
                    txtLicense2.Select();
                    txtLicense2.BackColor = Color.Yellow;
                    txtLicense1.BackColor = Color.White;
                    a = 0;
                }
            }
            if(e.KeyCode == Keys.Space)
            {
                txtCardNo.Select();
            }
        }
    }
}
