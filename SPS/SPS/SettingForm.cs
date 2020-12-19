using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BUS_SPS;

namespace SPS
{
    public partial class SettingForm : Form
    {
        #region
        public int SelectedGate { get; set; }
        public string ParkingName { get; set; }
        public int ParkingID { get; set; }
        public int EnterMethod { get; set; }
        public int working { get; set; }
        public string cameraLink { get; set; }
        public int cameraMethod { get; set; } //0 - No, 1 - Yes
        BUS_ParkingPlace busPK = new BUS_ParkingPlace();
        #endregion
        public SettingForm()
        {
            InitializeComponent();
            //comboBoxEx1.LostFocus += new EventHandler(comboBoxEx1_LostFocus);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            ParkingName = comboBoxEx1.Text;
            ParkingID = Convert.ToInt32(comboBoxEx1.SelectedValue);
            updateParking(ParkingID);
            if (rbtIn.Checked == true)
            {
                SelectedGate = 0;
                updateGate(0);
            }
            else
            {
                SelectedGate = 1;
                updateGate(1);
            }
            if (swbtnEnter.Value == true)
            {
                EnterMethod = 1;
                updateMethod(1);
            }
            else
            {
                EnterMethod = 0;
                updateMethod(0);
            }
            if(chkCameraIP.Checked == true)
            {
                cameraMethod = 1;
                cameraLink = txtStream.Text;
                updateCameraMethod(1);
                updateCameraStream(txtStream.Text);
            }
            else
            {
                cameraMethod = 0;
                updateCameraMethod(0);
            }
            this.Close();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            getGate();
            getMethod();
            getCameraMethod();
            getCameraStream();
            if(working == 0)
            {               
                getParking();
                comboBoxEx1.Enabled = true;
                chkCameraIP.Enabled = true;
            }
            else if(working == 1)
            {                
                getParking();
                comboBoxEx1.Enabled = false;
                chkCameraIP.Enabled = false;
            }
            if (chkCameraIP.Checked == false)
            {
                txtStream.Enabled = false;
            }
            else
            {
                txtStream.Enabled = true;
            }
            comboBoxEx1.Select();          
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        //get bai do xe
        private void getParking()
        {
            comboBoxEx1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxEx1.DataSource = busPK.getParkingPlace();
            comboBoxEx1.DisplayMember = busPK.getParkingPlace().Columns[1].ToString();
            comboBoxEx1.ValueMember = busPK.getParkingPlace().Columns[0].ToString();
            var parkingID = Properties.Settings.Default.ParkingPlace;
            if (busPK.checkParking(parkingID))
            {
                comboBoxEx1.SelectedValue = parkingID.ToString();
            }       
        }
        private void updateParking(int value)
        {
            Properties.Settings.Default.ParkingPlace = value;
            Properties.Settings.Default.Save();
        }

        //phuong thuc Nhap bang tay / tu dong
        private void getMethod()
        {
            var method = Properties.Settings.Default.Method;
            if (method == 0)
            {
                swbtnEnter.Value = false;
            }
            else
            {
                swbtnEnter.Value = true;
            }
        }
        private void updateMethod(int value)
        {
            Properties.Settings.Default.Method = value;
            Properties.Settings.Default.Save();
        }

        //get thong tin cong
        private void getGate()
        {
            var gate = Properties.Settings.Default.Gate;
            if(gate == 0)
            {
                rbtIn.Checked = true;
                rbtOut.Checked = false;
            }
            else
            {
                rbtIn.Checked = false;
                rbtOut.Checked = true;
            }
        }
        private void updateGate(int value)
        {
            Properties.Settings.Default.Gate = value;
            Properties.Settings.Default.Save();
        }

        //get link camera 
        private void getCameraStream()
        {
            var streamLink = Properties.Settings.Default.rtsp;
            txtStream.Text = streamLink;
            cameraLink = streamLink;
        }
        private void updateCameraStream(string value)
        {
            Properties.Settings.Default.rtsp = value;
            Properties.Settings.Default.Save();
        }
        private void getCameraMethod()
        {
            var camMethod = Properties.Settings.Default.cameraIP;
            if (camMethod == 0)
            {
                chkCameraIP.Checked = false;
            }
            else
            {
                chkCameraIP.Checked = true;
            }
        }
        private void updateCameraMethod(int value)
        {
            Properties.Settings.Default.cameraIP = value;
            Properties.Settings.Default.Save();
        }

        //su kien phim tat
        private void SettingForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                rbtIn.Checked = true;
            }
            if (e.KeyCode == Keys.F2)
            {
                rbtOut.Checked = true;
            }
            if (e.KeyCode == Keys.F3)
            {
                swbtnEnter.Value = !swbtnEnter.Value;
            }
            if (e.KeyCode == Keys.F5)
            {
                chkCameraIP.Checked = !chkCameraIP.Checked;
            }
            if (e.KeyCode == Keys.Up||e.KeyCode ==Keys.Down)
            {
                comboBoxEx1.Select();
                if(rbtIn.Checked == true)
                {
                    rbtIn.Checked = true;
                }
                if (rbtOut.Checked == true)
                {
                    rbtOut.Checked = true;
                }
            }
        }

        private void chkCameraIP_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCameraIP.Checked == false)
            {
                txtStream.Enabled = false;
            }
            else
            {
                txtStream.Enabled = true;
            }
        }
        //giu focus o combobox
        //private void comboBoxEx1_Leave(object sender, EventArgs e)
        //{
        //    comboBoxEx1.Select();
        //}
        //private void comboBoxEx1_LostFocus(object sender, EventArgs e)
        //{
        //    comboBoxEx1.Select();
        //}
    }
}
