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
        #endregion
        public LoginForm()
        {
            InitializeComponent();
        }
    }
}
