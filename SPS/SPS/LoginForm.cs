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
        public int accountID { get; set; }
        public int userID { get; set; }
        BUS_User busUser = new BUS_User();
        BUS_Account busAccount = new BUS_Account();
        public int parkingID;
        Messages mes = new Messages();
        #endregion
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            buttonOK();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            // txtUsername.Text = parkingID.ToString();
        }
        private void buttonOK()
        {
            if (busAccount.checkAccount(txtUsername.Text)) //kiem tra account co ton tai khong, co bi khoa khong va co phai bao ve khong
            {
                if (busAccount.getPassword(txtUsername.Text).Equals(txtPassword.Text)) //kiem tra mat khau
                {
                    accountID = busAccount.getAccountID(txtUsername.Text);
                    if (busUser.checkUser(accountID, parkingID)) //kiem tra bao ve co dung bai do xe khong
                    {
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
            else
            {
                if (MessageBox.Show(mes.mes(12), "Kiểm tra tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                }
            }
        }

        private void LoginForm_KeyDown(object sender, KeyEventArgs e) //cai dat phim tat
        {
            if (e.KeyCode == Keys.F1)
            {
                txtUsername.Select();
            }
            if (e.KeyCode == Keys.F2)
            {
                txtPassword.Select();
            }
        }


    }
}
