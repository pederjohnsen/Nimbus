using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Collections.ObjectModel;

namespace Nimbus.Network
{
    public class NimboidSession
    {


        public delegate void ShowChatDelegate(string user, string message);
        public delegate void AddLineDelegate(ChatWindow cw, string user, string message);

        public Collection<ChatWindow> Chats;

        public String User;
        public String Password;

        public int UserID;

        public TcpClient tcpClient;

        private Thread thrMessaging;

        private StreamReader srReceiver;

        private StreamWriter swSender;

        private delegate void NewChat();

        private String _resp;

        public event EventHandler ConnectionChange;

        public String Resp
        {
            get
            {
                return _resp;
            }
            set
            {
                _resp = value;
                Console.WriteLine("Server: {0}", _resp);
            }

        }

        private bool _connected;

        public bool Connected
        {
            get { return _connected; }
            set { _connected = value; }
        }





        public NimboidSession()
        {
            Chats = new Collection<ChatWindow>();
            
        }

        private void LoggedIn()
        {



        }

        public void Login()
        {
            tcpClient = new TcpClient();
            IPAddress ip = IPAddress.Parse("192.168.42.67");
            tcpClient.BeginConnect(ip, 8001, OnConnect, tcpClient);
        }

        public void OnConnectionChange()
        {
            if (ConnectionChange != null) ConnectionChange(this, null);
        }

        private void OnConnect(IAsyncResult ar)
        {
            if (!tcpClient.Connected) return;
            
            swSender = new StreamWriter(tcpClient.GetStream());
            srReceiver = new StreamReader(tcpClient.GetStream());
            Resp = srReceiver.ReadLine();
            if (Resp.Contains("OK")) swSender.WriteLine("USER {0}", User);
            else return;
            swSender.Flush();
            Resp = srReceiver.ReadLine();
            if (Resp.Contains("OK")) swSender.WriteLine("PASS {0}", Password);
            else return;
            swSender.Flush();
            Resp = srReceiver.ReadLine();
            if (Resp.Contains("OK"))
            {
                Connected = true;
                OnConnectionChange();
                thrMessaging = new Thread(new ThreadStart(ReceiveMessages));
                thrMessaging.IsBackground = true;
                thrMessaging.Start();
            }

        }

        private void ReceiveMessages()
        {
            while (Connected)
            {

                // Show the messages in the log TextBox

                Resp = srReceiver.ReadLine();
                if (Resp == null)
                {
                    Connected = false;
                    OnConnectionChange();
                    break;
                }
                if (Resp.StartsWith("PING")) SendToServer("PONG");

                if (Resp.StartsWith("FRIENDS")){
                    
                    Factory.FriendsMan.StartPopulate(Resp.Substring(8));

                }
                if (Resp.StartsWith("CHAT FROM"))
                {

                    String[] split = Resp.Split(' ');
                    Console.WriteLine("Incoming Chat from {0}", split[2]);
                    bool found = false;
                    if (Chats.Count != 0)
                    {
                        foreach (ChatWindow cw in Chats)
                        {
                            if (cw.User == split[2])
                            {
                                found = true;
                                string message = "";
                                for (int i = 3; i < split.Length; i++)
                                {
                                    message = message + split[i] + ' ';

                                }
                                Addline(cw, cw.User, message); // a function that adds a line to the current chat
                            }

                        }
                    }
                    if (!found)
                    {

                        ShowChatWindow(split[2], split[3]);

                    }

                }

            }


        }

        public void ShowChatSync(string user)
        {
            foreach(ChatWindow c in Chats){
                if (c.User == user)
                {
                    c.Show();
                    c.Focus();
                    return;
                }
            }
            ChatWindow temp = new ChatWindow(user);
            Chats.Add(temp);
            temp.Show();
        }

        public void ShowChatHandler(string user, string message)
        {
            ChatWindow temp = new ChatWindow(user, message);
            Chats.Add(temp);
            temp.Show();
            FlashWindow.Flash(temp);
        }

        

        public void ShowChatWindow(string sUser, string sMessage)
        {
            ShowChatDelegate handler = new ShowChatDelegate(ShowChatHandler);
            Factory.MainForm.BeginInvoke(handler, new object[] { sUser, sMessage });
           
        }

        private void AddLineHandler(ChatWindow cw, string user, string message)
        {
            cw.AddLine(user, message);
            if (!cw.Focused) FlashWindow.Flash(cw);
        }

        public void Addline(ChatWindow cw, string sUser, string sMessage)
        {
            AddLineDelegate handler = new AddLineDelegate(AddLineHandler);
            Factory.MainForm.BeginInvoke(handler, new object[] {cw, sUser, sMessage});
                                   
        }



        public void SendToServer(string p)
        {
            swSender.WriteLine(p);
            swSender.Flush();
        }



        internal void CloseConnection()
        {
            if (Connected) SendToServer("QUIT");
            tcpClient.Close();
        }
    }
}
