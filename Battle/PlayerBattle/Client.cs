using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace PokemonPocket
{
    class Client
    {
        public static void Start()
        {
            TcpClient client = null;
            try
            {
                // Connect to server
                string addr = "127.0.0.1";
                int port = 8000;
                client = new TcpClient(addr, port);

                // Get network stream
                NetworkStream stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                BinaryWriter writer = new BinaryWriter(stream);

                // Send message
                Console.Write(">>> ");
                writer.Write(Console.ReadLine());
                // Receive message
                Console.WriteLine(reader.ReadString());

                // Close
                reader.Close();
                writer.Close();
                stream.Close();
            }
            catch
            {
                client.Close();
            }
        }
    }
}