
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Linq;


namespace TcpEchoServer
{
	

	public class TcpEchoServer
	{

		// Counting a's! 
		public static void countFunction (object argument)
		{
			TcpClient client = (TcpClient)argument; 

			try {
				StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.ASCII) { AutoFlush = true };
				StreamReader reader = new StreamReader(client.GetStream(), Encoding.ASCII);

				Console.WriteLine("Trying to connect with client"); 
			} 

			catch (ThreadAbortException) {
				Console.WriteLine ("Connection error...");
				Thread.Sleep (2000); 
			}

			finally{
				Console.WriteLine("Client connected");
				StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.ASCII) { AutoFlush = true };
				StreamReader reader = new StreamReader(client.GetStream(), Encoding.ASCII);

				int messagesSend = 0;



				messages.Add("her en gamle beskeder");
				messages.Add("mere pis");

				Console.WriteLine("now sending messages");
				foreach (var message in messages){
					messagesSend++;
					writer.WriteLine("i got: " + message);
				}

				Console.WriteLine("Now listening to client");

				while (true){
					//Console.WriteLine("Recieved message: " + reader.ReadLine());
					if (reader.ReadLine() != null) {
						messages.Add(reader);
						writer.WriteLine("i got: " + messages.ElementAt(messages.Count-1));
					}

					//if (messages.Count > messagesSend) {
					//	writer.WriteLine("i got: " + messages.ElementAt(messagesSend));
					//	messagesSend++;
					//}

				}
				Console.WriteLine("Closing connection");
				reader.Close();
				writer.Close();
				client.Close();
			}
		}

		public static List<String> messages = new List<string>();

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

