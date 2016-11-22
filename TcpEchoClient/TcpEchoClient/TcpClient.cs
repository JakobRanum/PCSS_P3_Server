
using System;
using System.IO;
//using System.Net;
using System.Net.Sockets;
using System.Threading;
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

				Console.Write ("Enter to send: ");
				string lineToSend = "hej fra sagen";
				Console.WriteLine ("Sending to server: " + lineToSend);
				writer.WriteLine (lineToSend);
				string lineReceived = reader.ReadLine ();
				Console.WriteLine ("Received from server: " + lineReceived);

			} catch {
				Console.WriteLine ("failed");
			}
		}
	}
}