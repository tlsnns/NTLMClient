using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMBMessageParser
{
    public class SMB2Client
    {
        SMB2Worker SMB2Worker;
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public SMB2Client(string strIpAddress, string strUserName, string strPassword)
        {
            UserName = strUserName;
            Password = strPassword;
            SMB2Worker = new SMB2Worker(strIpAddress, 445);
        }
        public void Login()
        {
            try
            {
                SMB2Worker.Negotiate();
                SMB2Worker.SessionSetup(UserName, Password);
            }
            catch
            {
                throw;
            }
        }
        public void Logout()
        {
            try
            {
                SMB2Worker.SessionLogoff();
                SMB2Worker.Close();
            }
            catch
            {
                throw;
            }
        }
        public SMB2Tree CreateTree(string strShareName)
        {
            try
            {
                return new SMB2Tree(SMB2Worker, strShareName, SMB2Worker.TreeConnect(strShareName));
            }
            catch
            {
                Logout();
                throw;
            }
        }
    }
}
