using System;
using System.Net;

namespace Nimboid
{
	class MainClass
	{
		
		public static void Main (string[] args)
		{
			IPAddress ip = null;
			for (int i = 0; i < args.Length; i ++)
			{
				if (args[i] == "-ip") ip = IPAddress.Parse(args[i+1]);
					
			}
			
			if (ip != null) 
			{
			
				Console.WriteLine(String.Format("Creating Server at: {0}" ,ip.ToString()));
				NexServer server = new NexServer(ip);
				
				Console.WriteLine("Listen.");
				server.Listen();
				Console.WriteLine("Listening.");
				bool serveron = true;
				while (serveron)
				{
                    string res = Console.ReadLine();
                    string[] split;
					
					if (res.ToUpper() == "QUIT") 
					{
						Console.WriteLine("Quitting");
						serveron = false;
						server.KillServer();
						server = null;
                        
					}
					else if (res.ToUpper() == "LIST")
                    {
						foreach (Connection conn in NexServer.Conns)
						{
							
						 Console.WriteLine("{0} - {1}", conn.Ip , conn.User);
							
						}
						
					}
                    else if (res.ToUpper().StartsWith("SAY "))
                    {
                        split = res.Split(' ');
                        bool found = false; ;
                        foreach (Connection conn in NexServer.Conns)
                        {
                            if (conn.User == split[1])
                            {
                                conn.SendMessage(split[2]);
                                Console.WriteLine("Sending {0} to {1}", split[2], conn.User);
                                found = true;
                            }
                            if (!found) Console.WriteLine("User {0} not found", split[1]);
                        }
                    }
                    else if (res.ToUpper().StartsWith("USERID "))
                    {
                        split = res.Split(' ');
                        DBAccess db = new DBAccess();
                        Console.WriteLine("UserID for user {0} is {1}", split[1],db.GetUserID(split[1]));

                    }
                    else
                    {
                        if (NexServer.Conns.Count == 0) Console.WriteLine("No Current Connections.");
                        else
                        {
                            foreach (Connection conn in NexServer.Conns)
                            {
                                conn.SendMessage(res);
                            }
                        }
                    }
				}
				
				
			}
			
		}
	}
}
