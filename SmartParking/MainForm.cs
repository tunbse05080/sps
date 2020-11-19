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

namespace SmartParking
{
    public partial class MainForm : Form
    {
#region
        private VideoCaptureDevice CAM;
        private Bitmap BMP;
        private FilterInfoCollection CAMS;
        System.Windows.Forms.Timer tmr = null;
        #endregion
        public MainForm()
        {
            InitializeComponent();
            CAMS = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo info in CAMS)
            {
                comboBox1.Items.Add(info.Name);
            }
            comboBox1.SelectedIndex = 0;
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
            CAM.Stop();
            this.Close();
        }
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
        private void CallSetting()
        {
            using (SettingForm form2 = new SettingForm())
            {
                if (form2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    lblGate.Text = form2.SelectedText;
                }
            }
            StartTimer();
            startCamera();
        }
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

        private void btnCapture_Click(object sender, EventArgs e)
        {
            CapturePhoto();
        }
        private void CapturePhoto()
        {
            pictureBox_C.Image = pictureBox1.Image;
        }
    }
}
