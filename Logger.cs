using System;
using System.Threading;

namespace NsoftRTT
{
    class Logger
    {
        public static int numberOfMessages = 0;
        public static long rtt = 0;
        public static long aToB = 0;
        public static long bToA = 0;
        public static long slowestTime = 0;

        public static void Start(int mps)
        {
            Timer timer = new System.Threading.Timer(
                x => { Log(mps); },
                null,
                1000,
                1000
            );
            //Console.ReadLine();
        }

        private static void Log(int mps)
        {
            string timestamp = DateTime.UtcNow.ToString();
            Console.WriteLine(timestamp + " Total nr. of messages: " + numberOfMessages + " | Messages per second: " + mps
                + "\nSlowest time: " + slowestTime + "ms" 
                + "\nAverage A->B->A: " + GetAverageValue(rtt) + "ms"
                + "\nAverage A->B: " + GetAverageValue(aToB) + "ms"
                + "\nAverage B->A: " + GetAverageValue(bToA) + "ms"
                );

            Console.WriteLine("--------------------------------------------------------------------");
        }

        private static string GetAverageValue(long input)
        {
            return string.Format("{0:N4}", ((double)input / numberOfMessages));
        }
    }
}
