using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NsoftRTT
{
    public class Thrower
    {
        public static void Throw(string target, int port, int mps, int size)
        {
            try
            {
                double interval = (double)1000 / mps;

                IPAddress ipAddress = IPAddress.Parse(target);
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
                Timer timerThrow = new System.Threading.Timer(
                    x => { ThrowCallback(ipAddress, ipEndPoint, size); },
                    null,
                    TimeSpan.Zero,
                    TimeSpan.FromMilliseconds(interval)
                );
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(0);
            }
        }

        private static void ThrowCallback(IPAddress ipAddress, IPEndPoint ipEndPoint, int size)
        {
            byte[] responseData = new byte[1000];
            char[] msg = new char[size];
            for(int i = 0; i < size; i++)
            {
                msg[i] = 'A';
            }
            string msgAsString = new string(msg);
            Logger.numberOfMessages++;

            string[] payloadAsArray = new string[] { size.ToString(), Logger.numberOfMessages.ToString(), msgAsString };
            string payloadAsString = String.Join(';', payloadAsArray);
            byte[] payloadAsByteArray = Encoding.ASCII.GetBytes(payloadAsString);

            long startMilliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            Socket socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipEndPoint);
            socket.Send(payloadAsByteArray);

            int bufferSize = socket.Receive(responseData);

            long endMilliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            string data = Encoding.ASCII.GetString(responseData, 0, bufferSize);
            string[] dataSplitted = data.Split(';');

            int messageNumber = Int32.Parse(dataSplitted[1]);
            long millisecondsFromCapturer = Int64.Parse(dataSplitted[0]);

            if (endMilliseconds - startMilliseconds > Logger.slowestTime)
            {
                Logger.slowestTime = endMilliseconds - startMilliseconds;
            }

            Logger.aToB += (millisecondsFromCapturer - startMilliseconds);
            Logger.bToA += (endMilliseconds - millisecondsFromCapturer);
            Logger.rtt += (endMilliseconds - startMilliseconds);

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
