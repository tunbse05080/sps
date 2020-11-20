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
using BUS_SmartParking;

namespace SmartParking
{
    public partial class SettingForm : Form
    {
        #region
        public string SelectedText { get; set; }
        public string ParkingName { get; set; }
        public int ParkingID { get; set; }
        BUS_ParkingPlace busPK = new BUS_ParkingPlace();
        #endregion
        public SettingForm()
        {
            InitializeComponent();
            comboBox1.DataSource = busPK.getParkingPlace();
            comboBox1.DisplayMember = busPK.getParkingPlace().Columns[1].ToString();
            comboBox1.ValueMember = busPK.getParkingPlace().Columns[0].ToString();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            ParkingName = comboBox1.Text;
            ParkingID = Convert.ToInt32(comboBox1.SelectedValue);
            if (rbtIn.Checked == true)
            {
                SelectedText = "Gate In";
            }
            else
            {
                SelectedText = "Gate Out";
            }
            this.Close();
        }

    }
}
