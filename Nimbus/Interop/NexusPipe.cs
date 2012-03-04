using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.IO;

namespace Nimbus.Interop
{
    public class NexusPipe
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern SafeFileHandle CreateNamedPipe(
           String pipeName,
           uint dwOpenMode,
           uint dwPipeMode,
           uint nMaxInstances,
           uint nOutBufferSize,
           uint nInBufferSize,
           uint nDefaultTimeOut,
           IntPtr lpSecurityAttributes);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int ConnectNamedPipe(
           SafeFileHandle hNamedPipe,
           IntPtr lpOverlapped);

        public const uint DUPLEX = (0x00000003);
        public const uint FILE_FLAG_OVERLAPPED = (0x40000000);

        public const string PIPE_NAME = "\\\\.\\pipe\\nimbusnamedpipe";
        public const uint BUFFER_SIZE = 512;

        delegate void ListeningDelegate();
        public delegate void MessageReceivedHandler(string message);

        public event MessageReceivedHandler MessageReceived;

        public NexusPipe()
        {

        }

        public void StartListening()
        {
            ListeningDelegate L = new ListeningDelegate(Listen);
            L.BeginInvoke(null, null);

        }

        private void Listen()
        {
            SafeFileHandle clientPipeHandle;
            while (true)
            {
                clientPipeHandle = CreateNamedPipe(
                   PIPE_NAME,
                   DUPLEX | FILE_FLAG_OVERLAPPED,
                   0x00000004,
                   255,
                   BUFFER_SIZE,
                   BUFFER_SIZE,
                   0,
                   IntPtr.Zero);

                //failed to create named pipe
                if (clientPipeHandle.IsInvalid)
                    break;

                int success = ConnectNamedPipe(
                   clientPipeHandle,
                   IntPtr.Zero);

                //failed to connect client pipe
                if (success != 1)
                    break;

                //client connection successfull
                //handle client communication
                Read(clientPipeHandle);
            }
        }

        void Read(SafeFileHandle handle)
        {
         
            FileStream stream = new FileStream(handle, FileAccess.ReadWrite, (int)BUFFER_SIZE, true);
            byte[] buffer = new byte[BUFFER_SIZE];
            ASCIIEncoding encoder = new ASCIIEncoding();

            while (true)
            {
                int bytesRead = 0;

                try
                {
                    bytesRead = stream.Read(buffer, 0, (int)BUFFER_SIZE);
                }
                catch
                {
                    //read error has occurred
                    break;
                }

                //client has disconnected
                if (bytesRead == 0)
                    break;

                //fire message received event
                if (this.MessageReceived != null)
                    this.MessageReceived(encoder.GetString(buffer, 0, bytesRead));
            }

            handle.Close();

        }
    }
}
