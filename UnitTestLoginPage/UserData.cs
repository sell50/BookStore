using BookStoreLIB;
using System;
using System.Collections.Generic;

namespace UnitTestLoginPage
{
    internal class UserData
    {
        public int UserID { set; get; }
        public string LoginName { set; get; }
        public string Password { set; get; }
        public Boolean LoggedIn { set; get; }

        public Boolean LogIn(string loginName, string passWord)
        {
            var dbUser = new DALUserInfo();
            UserID = dbUser.LogIn(loginName, passWord);
            if (UserID > 0)
            {
                LoginName = loginName;
                Password = passWord;
                LoggedIn = true;
                return true;
            }
            else
            {
                LoggedIn = false;
                return false;
            }

        }

        internal Dictionary<string, string> accountInfo(int input_userId)
        {
            var dbUser = new DALUserInfo();
            return dbUser.AccountInfo(input_userId);
        }

        public void Logout()
        {
            UserID = -1;
            LoggedIn = false;
            LoginName = null;
            Password = null;
        }
    }
}