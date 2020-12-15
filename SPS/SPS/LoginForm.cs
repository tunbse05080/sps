using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS_SPS;

namespace SPS
{
    public partial class LoginForm : Form
    {
        #region
        public int userID { get; set; }
        BUS_User busUser = new BUS_User();
        public int parkingID;
        Messages mes = new Messages();
        #endregion
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            
            if (busUser.checkUser(txtUsername.Text, parkingID) == true)
            {
                if (busUser.getPassword(txtUsername.Text).Equals(txtPassword.Text))
                {
                    userID = busUser.getUserID(txtUsername.Text);
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                else
                {
                    if (MessageBox.Show(mes.mes(12), "Kiểm tra tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                    }
                }
            }
            else
            {
                if (MessageBox.Show(mes.mes(12), "Kiểm tra tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                }
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
           // txtUsername.Text = parkingID.ToString();
        }
    }
}
