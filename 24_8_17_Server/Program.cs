using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
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
            server.Run(IPAddress.Loopback, 5000);
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

        private void Communicate(Socket clSock)
        {
            try
            {
                IPEndPoint remoteIPEndPoint = clSock.RemoteEndPoint as IPEndPoint;
                IPEndPoint localIPEndPoint = clSock.LocalEndPoint as IPEndPoint;

                byte[] buffer = new byte[1024];
                while (clSock.Receive(buffer) > 0)
                {
                    string str = Encoding.ASCII.GetString(buffer);
                    byte[] msg = Encoding.ASCII.GetBytes(str);
                    clSock.Send(msg);
                }
                clSock.Shutdown(SocketShutdown.Both);
                clSock.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
            Console.WriteLine("Waiting for incoming client connections...");
            HandleClient clientHandler;
            while (true)
            {
                Socket newSocket = s.Accept();
                clientHandler = new HandleClient(newSocket);
                clientList.Add(new Thread(new ThreadStart(clientHandler.Echo)));
                clientList[clientList.Count - 1].Start();
            }

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
    class HandleClient
    {
        private TcpClient client;
        private Socket clSock;
        private StreamReader reader = null;
        private StreamWriter writer = null;

        public HandleClient(TcpClient newClient)
        {
            client = newClient;
        }

        public HandleClient(Socket newSocket)
        {
            clSock = newSocket;
        }

        public void Echo()
        {
            try
            {
                IPEndPoint remoteIPEndPoint = clSock.RemoteEndPoint as IPEndPoint;
                IPEndPoint localIPEndPoint = clSock.LocalEndPoint as IPEndPoint;

                byte[] buffer = new byte[1024];
                while (clSock.Receive(buffer) > 0)
                {
                    string str = Encoding.ASCII.GetString(buffer);
                    byte[] msg = Encoding.ASCII.GetBytes(str);
                    clSock.Send(msg);
                }
                clSock.Shutdown(SocketShutdown.Both);
                clSock.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //reader = new StreamReader(client.GetStream());
            //writer = new StreamWriter(client.GetStream());
            //writer.AutoFlush = true;

            //while (true)
            //{
            //    string msg = reader.ReadLine();
            //    writer.WriteLine(msg);
            //}
        }
    }
}
