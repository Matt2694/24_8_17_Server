using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace _24_8_17_Server
{
    class Program
    {
        private static TcpListener listener = null;
        private static StreamReader reader = null;
        private static StreamWriter writer = null;

        static void Main(string[] args)
        {
            Program server = new Program();
            server.Run();
        }

        private void Run()
        {
            try
            {
                listener = new TcpListener(5000);
                listener.Start();
                Console.WriteLine("Ready");

                HandleClient(listener.AcceptTcpClient());
            }
            catch(Exception e)
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

        private void HandleClient(TcpClient tcpClient)
        {
            reader = new StreamReader(tcpClient.GetStream());
            writer = new StreamWriter(tcpClient.GetStream());

            while (true)
            {
                string msg = reader.ReadLine();
                writer.WriteLine(msg);
                writer.Flush();
            }
        }
    }
}
