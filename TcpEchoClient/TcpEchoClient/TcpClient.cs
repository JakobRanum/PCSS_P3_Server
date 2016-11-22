
using System;
using System.IO;
//using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
namespace TcpEchoClient
{
	class TcpEchoClient
	{
		static void Main(string[] args)
		{
			
			Console.WriteLine ("Starting echo client...");

			int port = 11000;

			try {
				TcpClient client = new TcpClient ("192.168.0.14", port);
				NetworkStream stream = client.GetStream ();
				StreamReader reader = new StreamReader (stream);
				StreamWriter writer = new StreamWriter (stream) { AutoFlush = true };





				while (true) {
					
					Console.Write("Enter to send: ");

					string lineToSend = Console.ReadLine();
					Console.WriteLine("Sending to server: " + Console.ReadLine());
					writer.WriteLine(lineToSend);
					string lineReceived = reader.ReadLine();
					Console.WriteLine("Received from server: " + lineReceived);
				}


			} catch {
				Console.WriteLine ("failed");
			}


		}
	}
}