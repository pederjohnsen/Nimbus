using System;
using System.Collections.Generic;
using System.Text;

namespace Nimbus.Network
{
 
    public class Friend
    {
        private string _username;
        private string _status;

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Username, Status);
        }

    }
    

}
