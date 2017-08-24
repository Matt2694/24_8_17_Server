using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace _24_8_17_Server
{
    class Program
    {
        private TcpListener listener = null;
        Socket s = null;
        private List<Thread> clientList = new List<Thread>();

        static void Main(string[] args)
        {
            Program server = new Program();

            //using Sockets

            server.Run(IPAddress.Loopback, 5000);

            //using Tcp

            //server.Run();
        }

        private void Run(IPAddress ip, int port)
        {
            try
            {
                s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s.Bind(new IPEndPoint(ip, port));
                s.Listen(10);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            Console.WriteLine("ready");
            ConnectClients();
        }

        private void Run()
        {
            try
            {
                listener = new TcpListener(5000);
                listener.Start();
                Console.WriteLine("Ready");
                ConnectClients();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (listener != null)
                {
                    listener.Stop();
                }
            }
        }

        private void ConnectClients()
        {
            //using Sockets

            Console.WriteLine("Waiting for incoming client connections...");
            HandleClient clientHandler;
            while (true)
            {
                Socket newSocket = s.Accept();
                clientHandler = new HandleClient(newSocket);
                clientList.Add(new Thread(new ThreadStart(clientHandler.Echo)));
                clientList[clientList.Count - 1].Start();
            }

            //using Tcp

            //Console.WriteLine("Waiting for incoming client connections...");
            //HandleClient clientHandler;
            //while (true)
            //{
            //    clientHandler = new HandleClient(listener.AcceptTcpClient());
            //    clientList.Add(new Thread(new ThreadStart(clientHandler.Echo)));
            //    clientList[clientList.Count - 1].Start();
            //}
        }
    }
}
