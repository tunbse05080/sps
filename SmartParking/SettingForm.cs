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
    public partial class SettingForm : Form
    {
        public string SelectedText { get; set; }
        public SettingForm()
        {
            InitializeComponent();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
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
