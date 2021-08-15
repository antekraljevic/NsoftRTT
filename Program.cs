using System;

namespace NsoftRTT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args[0].ToLower() == "rttcalculator")
            {
                if (args.Length > 2)
                {
                    string mode = args[1];
                    int mps = 1;
                    int size = 200;
                    int port = 13000;
                    string target = null;
                    string bind = null;

                    switch (mode)
                    {
                        case "-thrw":
                            for (int i = 2; i < args.Length; i++)
                            {
                                if (args[i].ToLower() == "-port")
                                {
                                    if (i < args.Length - 1)
                                    {
                                        Int32.TryParse(args[i + 1], out port);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid command call. Please fix and try again.");
                                        Environment.Exit(0);
                                    }
                                    
                                }
                                if (args[i].ToLower() == "-mps")
                                {
                                    if (i < args.Length - 1)
                                    {
                                        Int32.TryParse(args[i + 1], out mps);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid command call. Please fix and try again.");
                                        Environment.Exit(0);
                                    }

                                    if (mps > 1000)
                                    {
                                        Console.WriteLine("Invalid -mps value: too high. Max value is 1000.");
                                        Environment.Exit(0);
                                    }
                                    else if (mps < 0)
                                    {
                                        Console.WriteLine("Invalid -mps value: must not be negative number.");
                                        Environment.Exit(0);
                                    }
                                }
                                if (args[i].ToLower() == "-size")
                                {
                                    if (i < args.Length - 1)
                                    {
                                        Int32.TryParse(args[i + 1], out size);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid command call. Please fix and try again.");
                                        Environment.Exit(0);
                                    }

                                    if (size < 50)
                                    {
                                        Console.WriteLine("Invalid -size value: too low. Min value is 50.");
                                        Environment.Exit(0);
                                    }
                                    else if (size > 3000)
                                    {
                                        Console.WriteLine("Invalid -mps value: too high. Max value is 3000");
                                        Environment.Exit(0);
                                    }
                                }
                                if (args[i].ToLower() == "-target")
                                {
                                    if (i < args.Length - 1)
                                    {
                                        target = args[i + 1] ?? null;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid command call. Please fix and try again.");
                                        Environment.Exit(0);
                                    }
                                }
                            }

                            if(target == null)
                            {
                                Console.WriteLine("Target parameter missing");
                                Environment.Exit(0);
                            }

                            Logger.Start(mps);
                            Thrower.Throw(target, port, mps, size);
                            break;
                        case "-cptr":
                            for (int i = 2; i < args.Length; i++)
                            {
                                if (args[i].ToLower() == "-port")
                                {
                                    if (i < args.Length - 1)
                                    {
                                        Int32.TryParse(args[i + 1], out port);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid command call. Please fix and try again.");
                                        Environment.Exit(0);
                                    }
                                }
                                if (args[i].ToLower() == "-bind")
                                {
                                    if (i < args.Length - 1)
                                    {
                                        bind = args[i + 1];
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid command call. Please fix and try again.");
                                        Environment.Exit(0);
                                    }
                                }
                            }

                            if (bind == null)
                            {
                                Console.WriteLine("Bind parameter missing");
                                Environment.Exit(0);
                            }

                            Capturer.Capture(port, bind);
                            break;
                        default:
                            Console.WriteLine("Insert valid operation mode: -thrw (thrower) or -cptr (capturer)");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid command call.");
                    Console.WriteLine("Please use something like:");
                    Console.WriteLine("dotnet run RTTCalculator –thrw –port 8888 –mps 50 –size 1000 ComputerB");
                    Console.WriteLine("or");
                    Console.WriteLine("dotnet run RTTCalculator –cptr –bind 192.168.1.1 –port 8888");
                }
            }
            else 
            {
                Console.WriteLine("Please invoke command properly by using:");
                Console.WriteLine("dotnet run RTTCalculator");
                Console.WriteLine("then add command parameters");
            }
        }
    }
}
