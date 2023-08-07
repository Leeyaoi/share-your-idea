using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using Ideas;
using Menu;

namespace Users
{
    class User
    {
        public string login = "";
        public string password = "";
        public int role = -1;

        public void Set(string log, string pass, int rol)
        {
            login = log;
            password = pass;
            role = rol;
        }

        public bool IsThemHere(User[] users)
        {
            for(int i = 0; i < users.Length; i++)
            {
                User user = users[i];
                if (user.role == role && user.login == login && user.password == password)
                {
                    return true;
                }
            }
            return false;
        }

        public void LoggedOut()
        {
            login = "";
            password = "";
            role = -1;
        }
    }
}