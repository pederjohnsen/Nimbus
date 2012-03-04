using System;
using System.Collections.Generic;
using System.Text;

namespace Nimboid
{
    class ServerMessage
    {

        public String Command;
        public String User;
        public String Message;
        public bool Invalid = false;

        public ServerMessage(String reply)
        {
            String[] res = reply.Split(' ');
            Command = res[0];

            if (res.Length < ArgsRequired())
            {
                Invalid = true;
                return;

            }

            if (Command == "CHAT")
            {
                User = res[1];
                for (int i = 2; i < res.Length; i++)
                {
                    Message = Message + res[i] + ' ';

                }

            }
            if (Command == "STAT")
            {
                for (int i = 1; i < res.Length; i++)
                {
                    Message = Message + res[i] + ' ';

                }

            }


        }

        private bool NeedsUser()
        {

            return (Command == "CHAT");

        }

        private int ArgsRequired()
        {
            if (Command == "CHAT") return 3;
            else if (Command == "STAT") return 2;
            else return 1;

        }
    }
}
