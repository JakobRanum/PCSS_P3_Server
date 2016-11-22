
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;



namespace TcpEchoServer
{
	

	public class TcpEchoServer
	{

		// Counting a's! 
		public static void countFunction (object argument)
		{
			TcpClient client = (TcpClient)argument; 

			try {

				StreamWriter writer = new StreamWriter (client.GetStream (), Encoding.ASCII) { AutoFlush = true };
				StreamReader reader = new StreamReader (client.GetStream (), Encoding.ASCII);

				String inputLine = reader.ReadLine();

				writer.WriteLine("i got: " + pik); 

				reader.Close(); 
				writer.Close(); 
				client.Close(); 

				Console.WriteLine("Connection with client closed..."); 
			} 

			catch (ThreadAbortException) {
				Console.WriteLine ("Connection error...");
				Thread.Sleep (2000); 
			}

			finally{
				if (client != null) {
					client.Close (); 
				}
			}
		}

		public static List<String> pik = new List<string>();

		public static void Main ()
		{
			
			Console.WriteLine ("Starting echo server...");

			int port = 11000;
			int nameId = 0;
			IPAddress myIp = IPAddress.Parse ("192.168.0.14");

			TcpListener listener = new TcpListener (myIp, port);
			listener.Start ();

			while (true) { 
				TcpClient client = listener.AcceptTcpClient ();
				Thread thread = new Thread (countFunction);
				thread.Name = nameId.ToString();
				thread.Start (client);

				nameId++;

			}
		}
	}
}

