using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using System.IO;
using System.Threading;
using DTO__SmartParking;
using BUS_SmartParking;

namespace SmartParking
{
    public partial class MainForm : Form
    {
#region
        private VideoCaptureDevice CAM;
        private Bitmap BMP;
        private FilterInfoCollection CAMS;
        System.Windows.Forms.Timer tmr = null;
        BUS_ParkingPlace busPK = new BUS_ParkingPlace();
        public int CarFree { get; set; }
        public int MotorFree { get; set; }
        #endregion
        public MainForm()
        {
            InitializeComponent();
           CAMS = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (CAMS.Count > 0)
            {
                foreach (FilterInfo info in CAMS)
                {
                    comboBox1.Items.Add(info.Name);
                }
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.Text = "No cameras found";
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

        private void Form1_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            CallSetting();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (CAMS.Count > 0)
            {
                CAM.Stop();
            }
            this.Close();
        }

        //start camera
        private void startCamera()
        {
            if (CAM != null && CAM.IsRunning)
            {
                CAM.Stop();
            }
            CAM = new VideoCaptureDevice(CAMS[comboBox1.SelectedIndex].MonikerString);
            CAM.NewFrame += CAM_NewFrame;
            CAM.Start();
        }
        private void CAM_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            BMP = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = BMP;
            //   throw new NotImplementedException();
        }

        //Get info from Setting
        private void CallSetting()
        {
            using (SettingForm form2 = new SettingForm())
            {
                if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    lblGate.Text = form2.SelectedText;
                    lblParkingName.Text = form2.ParkingName;
                    lblNumberCar.Text = busPK.getCarFree(form2.ParkingID).ToString();
                    lblNumberMotor.Text = busPK.getMotorFree(form2.ParkingID).ToString();
                }
            }
            StartTimer();
            if (CAMS.Count > 0)
            {
                startCamera();
            }               
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
            lblTime.Text = DateTime.Now.ToString();
        }
        private void btnSetting_Click(object sender, EventArgs e)
        {
            CallSetting();
        }

        //Capture button
        private void btnCapture_Click(object sender, EventArgs e)
        {
            CapturePhoto();
        }

        //capture photo
        private void CapturePhoto()
        {
            pictureBox_C.Image = pictureBox1.Image;
        }
    }
}
