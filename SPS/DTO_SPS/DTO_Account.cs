using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_SPS
{
    public class DTO_Account
    {
        private int accountID;
        private string username;
        private string password;
        private int roleID;

        public int MyRoleID
        {
            get { return roleID; }
            set { roleID = value; }
        }


        public string MyPassword
        {
            get { return password; }
            set { password = value; }
        }


        public string MyUsername
        {
            get { return username; }
            set { username = value; }
        }


        public int MyAccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }

        public DTO_Account()
        {
        }

        public DTO_Account(int accountID, string username, string password, int roleID)
        {
            this.accountID = accountID;
            this.username = username;
            this.password = password;
            this.roleID = roleID;
        }
    }
}
