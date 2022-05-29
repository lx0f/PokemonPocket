using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace PokemonPocket
{
    class Server
    {
        public static void Start()
        {
            TcpListener server = null;
            try
            {
                // Initiate server
                int port = 8000;
                string addr = "127.0.0.1";
                server = new TcpListener(IPAddress.Parse(addr), port);

                // Start listening for clients
                server.Start();
                Console.WriteLine("Game hosted on");
                Console.WriteLine($"IP: {addr}");
                Console.WriteLine($"Port: {port}");
                Console.WriteLine("Waiting for connection...");

                // Accept client connection
                TcpClient client = server.AcceptTcpClient();

                // Get network stream
                NetworkStream stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                BinaryWriter writer = new BinaryWriter(stream);

                // Receive message
                Console.WriteLine(reader.ReadString());
                Console.Write(">>> ");
                writer.Write(Console.ReadLine());

                reader.Close();
                writer.Close();
                stream.Close();
            }
            catch
            {
                // Stop game
                server.Stop();
            }
        }
    }
}