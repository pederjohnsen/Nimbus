using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Nimbus.Network
{

    public class LoginResult 
    {
        public bool Passed = false;
        public string Hash = "";
        public string Email;
        public string Password;

        public LoginResult(bool passed)
        {
            Passed = passed;
        }

        public LoginResult(string hash, string email, string password)
        {
            Hash = hash;
            Email = email;
            Password = password;
            Passed = true;
        }
    }

    public class LoginManager
    {

        string loginscript = Globals.Homepage + "login.php";



        public LoginResult Login(string user, string pass)
        {
            LoginResult toReturn;
            
            WebClient wc = new WebClient();
            string loginstring = String.Format("{0}?username={1}&password={2}", loginscript, user, Security.HashString(pass));
            Console.WriteLine("Sending to: {0}", loginstring);
            string reply = wc.DownloadString(loginstring);
            if (reply.StartsWith("error"))
            {
                toReturn = new LoginResult(false);
                Console.WriteLine("Login Failed");
            }
            else
            {
                toReturn = new LoginResult(reply, user, pass);
                Console.WriteLine("Received Hash {0}", reply);
            }

            return toReturn;
        }

    }
}
