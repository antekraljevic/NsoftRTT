using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace NsoftRTT
{
    public class Capturer
    {
        public static void Capture(int port, string address)
        {
            byte[] buffer = new byte[1000];
            string data = null;

            IPAddress ipAddress = IPAddress.Parse(address);
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
            
            int count = 0;

            Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);


            socket.Bind(ipEndPoint);
            socket.Listen();

            while (true)
            {

                Console.WriteLine("Waiting for clients... {0}", count);
                Socket confd = socket.Accept();

                int bufferSize = confd.Receive(buffer);
                long nowInMilliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                data += Encoding.ASCII.GetString(buffer, 0, bufferSize);

                string[] dataSplitted = data.Split(';');

                int size = Int32.Parse(dataSplitted[0]);
                int messageNumber = Int32.Parse(dataSplitted[1]);

                string[] payloadAsArray = new string[] { nowInMilliseconds.ToString(), messageNumber.ToString(), dataSplitted[2] };
                string payloadAsString = String.Join(';', payloadAsArray);
                byte[] payloadAsByteArray = Encoding.ASCII.GetBytes(payloadAsString);

                data = null;

                Console.WriteLine("Message received, sending message back to thrower...");

                confd.Send(payloadAsByteArray);

                confd.Close();
                count++;
            }
        }
    }
}
