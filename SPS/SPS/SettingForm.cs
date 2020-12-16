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
        BUS_ParkingPlace busPK = new BUS_ParkingPlace();
        #endregion
        public SettingForm()
        {
            InitializeComponent();
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            ParkingName = comboBoxEx1.Text;
            ParkingID = Convert.ToInt32(comboBoxEx1.SelectedValue);
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
            this.Close();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            getGate();
            getMethod();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
        private void getParking()
        {
            var parkingID = Properties.Settings.Default.ParkingPlace;
            
            comboBoxEx1.DataSource = busPK.getParkingPlace();
            comboBoxEx1.DisplayMember = busPK.getParkingPlace().Columns[1].ToString();
            comboBoxEx1.ValueMember = busPK.getParkingPlace().Columns[0].ToString();
        }
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
    }
}
