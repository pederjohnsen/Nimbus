using System;
using System.Collections.Generic;
using System.Text;

namespace Nimbus.Network
{
    public class Session
    {

        string sessionHash;
        string email;
        public bool LoggedIn { get; set; }

        public LoginResult Login()
        {
            LoginResult lr = LoginForm.ShowLogin(Factory.CurrentTheme);

            if (lr.Passed)
            {
                Console.WriteLine("Sending Hash {0}", lr.Hash);
                sessionHash = lr.Hash;
                email = lr.Email;
                LoggedIn = true;
           }

            return lr;


        }
    }
}
