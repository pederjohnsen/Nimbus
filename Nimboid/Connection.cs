
using System;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Collections;

namespace Nimboid
{

  public class Connection
    {


      System.Timers.Timer timer;
      int TimeSincePing = 0;

        public TcpClient tcpClient;

        // The thread that will send information to the client
        private Thread gatherer;
        private Thread thrSender;
        private DBAccess db;
        private StreamReader srReceiver;

        private StreamWriter swSender;

        private string currUser;

        private string strResponse;
        
        private string user = "#NOT AUTHENTICATED YET#";

        public string User
        {
            get { return user; }
            set { user = value; }
        }
		private string pass;

        private string _ip;

        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

    // User Details

        private int _id = -1;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private ArrayList _friends;

        public ArrayList Friends
        {
            get { return _friends; }
            set { _friends = value; }
        }
        private String _status;

        public String Status
        {
            get { return _status; }
            set 
            { 
                _status = value;
                InformFriendsOfStatChange();
            }
        }

        private void InformFriendsOfStatChange()
        {

            foreach (Connection conn in NexServer.Conns)
            {
                foreach (int fid in Friends)
                {
                    if (conn.Id == fid) conn.SendMessage("STATOF {0} {1}", User, Status);

                }

            }

        }


      


        // The constructor of the class takes in a TCP connection

        public Connection(TcpClient tcpCon)
        {
            db = new DBAccess();
            _friends = new ArrayList();

            timer = new System.Timers.Timer(1000);
            timer.AutoReset = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);

            tcpClient = tcpCon;
            Ip = ((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString();
            // The thread that accepts the client and awaits messages

            thrSender = new Thread(AcceptClient);

            // The thread calls the AcceptClient() method

            thrSender.Start();

        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            
            TimeSincePing++;
            if (TimeSincePing == 60) SendMessage("PING");
            if (TimeSincePing == 90) SendMessage("PING");
            if (TimeSincePing > 120)
            {
                timer.Stop();
                CloseConnection();

            }
        }



        private void CloseConnection()
        {

            // Close the currently open objects

            Console.WriteLine("Connection with {0} terminated", this.user);
            

            tcpClient.Close();

            srReceiver.Close();

            swSender.Close();
			
			NexServer.Conns.Remove(this);

            timer.Stop();

            thrSender.Abort();
            gatherer.Abort();

            

        }

        public void SendMessage(String message)
        {
            Console.WriteLine("Sending: {0} to {1}", message, user);
            swSender.WriteLine(message);
            swSender.Flush();

        }

		public void SendMessage(String message, params object[] paramlist){

            Console.WriteLine("Sending: {0}", String.Format(message, paramlist));
		swSender.WriteLine(message, paramlist);
		swSender.Flush();
			
		}
        // Occures when a new client is accepted
        private void InformClient()
        {
            if (_friends.Count > 0)
            {
                string temp = "";
                foreach (int fid in _friends)
                {

                    temp = temp + "#";
                    temp = temp + db.GetUsername(fid);
                }
                SendMessage("FRIENDS " + temp);
            }
            
        }

        private void PopulateClass()
        {
            Id = db.GetUserID(User);
            _friends = db.GetFriends(Id);
            InformClient();

        }

        private void AcceptClient()
        {

            srReceiver = new System.IO.StreamReader(tcpClient.GetStream());

            swSender = new System.IO.StreamWriter(tcpClient.GetStream());
            
			swSender.WriteLine("OK");
			swSender.Flush();

            // Read the account information from the client

            currUser = srReceiver.ReadLine();
			string[] comm = currUser.Split(' ');
			if (comm[0] == "USER") {
				user = comm[1];
				Console.WriteLine("Connected User: {0}", user);
				swSender.WriteLine("OK", user);
				swSender.Flush();
				currUser = srReceiver.ReadLine();
				comm = currUser.Split(' ');
				if (comm[0] == "PASS") {
					pass = comm[1];
					Console.WriteLine("Password {0} Received", pass);
                    if (db.CheckPassword(user, pass))
                    {
                        swSender.WriteLine("OK");
                        swSender.Flush();
                    }
                    else
                    {
                        swSender.WriteLine("FAIL");
                        swSender.Flush();
                        Console.WriteLine("Authentication Failed");
                        CloseConnection();
                    }
				}
				else 
				{
				CloseConnection();
					return;
				}
				
			}
			else
			{
				CloseConnection();
				return;	
			}

            try
            {
                gatherer = new Thread(PopulateClass);
                gatherer.Start();

                timer.Start();

                while ((strResponse = srReceiver.ReadLine()) != "")
                {
                    Console.WriteLine("Received: {0}", strResponse);
                    ServerMessage message = new ServerMessage(strResponse);
                    if (message.Invalid) SendMessage("Invalid Command");
                    else
                    {
                        switch (message.Command)
                        {
                            case "QUIT":
                                CloseConnection();
                                break;

                            case "CHAT":
                               
                                    foreach (Connection conn in NexServer.Conns)
                                    {
                                        if (conn.user == message.User)
                                        {
                                            conn.SendMessage("CHAT FROM {0} {1}", this.user, message.Message);


                                        }

                                    }

                                

                                break;
                            
                            case "PONG":
                                TimeSincePing = 0;
                                break;
                            case "STAT":
                                Status = message.Message;

                                break;
                            case "ID":
                                if (Id != -1) SendMessage("ID " + Id.ToString());
                                else SendMessage("ID " + db.GetUserID(User).ToString());
                                break;
                            case "FRIENDS":
                                if (_friends.Count == 0)
                                {
                                    _friends = db.GetFriends(Id);
                                    Console.WriteLine("Received {0} friends", _friends.Count);
                                }
                                if (_friends.Count > 0)
                                {
                                    string temp = "";
                                    foreach (int fid in _friends)
                                    {

                                        temp = temp + "#";
                                        temp = temp + db.GetUsername(fid);
                                    }
                                    SendMessage("FRIENDS " + temp);
                                }
                                else SendMessage("NO FRIENDS");
                                break;
                            default:
                                swSender.WriteLine("Unknown Command: {0}", strResponse);
                                swSender.Flush();
                                break;
                        }
                    }
					
                }

                CloseConnection();

            }

            catch
            {

                // If anything went wrong with this user, disconnect him

                CloseConnection();

            }

        }

    }
}
