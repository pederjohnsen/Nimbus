
using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections.ObjectModel;

namespace Nimboid
{


	public class NexServer
	{
		
		private IPAddress LocalIP;
        private Thread thrListener;
        private TcpListener tlsClient;
		private TcpClient tcpClient;
        bool ServRunning = false;
		
		public static Collection<Connection> Conns = new Collection<Connection>();
		
		
		
		
		public NexServer (IPAddress _localIp)
		{
			
			LocalIP = _localIp;
			
		}
		
		public void KillServer()
		{
		ServRunning = false;	
		thrListener.Abort();
  
			
			
		}
		
		public void Listen()
		{
			
			tlsClient = new TcpListener(LocalIP, 8001);
			
            tlsClient.Start();
			ServRunning = true;
			thrListener = new Thread(KeepListening);
            thrListener.IsBackground = true;
			thrListener.Start();
					
			
		}
		
		private void KeepListening()
        {

        

        	while (ServRunning == true)
            {

            tcpClient = tlsClient.AcceptTcpClient();

            Connection newConnection = new Connection(tcpClient);
			Conns.Add(newConnection);
            
            }

         }
		
	}
}
