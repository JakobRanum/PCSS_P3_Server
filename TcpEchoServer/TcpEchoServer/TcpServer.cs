
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
	

	public class TcpEchoServer{

		public static void sender (object argument){
			TcpClient client = (TcpClient)argument;

			try{
				StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.ASCII) { AutoFlush = true };

				int messagesSend = 0;
				messages.Add("Welcome to chatRoom Ofir");

				foreach (var message in messages){
					messagesSend++;
					writer.WriteLine("i got: " + message);
				}

				while (client.Connected){
					if (messages.Count > messagesSend){
						try{
							writer.WriteLine("i got: " + messages.ElementAt(messages.Count-1));
							messagesSend++;
						}
						catch{
							Console.WriteLine("error, client disconnected");
						}
					}

				}

				//Console.WriteLine("Client disconnected sender");

			}

			catch (ThreadAbortException){
				client.Close();
				Console.WriteLine("Connection error...");
				Thread.Sleep(2000);
			}

			finally{
				Console.WriteLine("finally");
			}
		}





		public static void reciever(object argument) {
			TcpClient client = (TcpClient)argument;

			try{
				StreamReader reader = new StreamReader(client.GetStream(), Encoding.ASCII);

				Console.WriteLine("Now listening to client");

				while (client.Connected) {
					if (true) {
						try{
							string message = reader.ReadLine();
							if (message != null){
								messages.Add(message);
							}
							Console.WriteLine(messages.ElementAt(messages.Count-1));
						}
						catch{
							reader.Close();
							client.Close();
							Console.WriteLine("Connection Error");
							break;
						}
					}
				}
				Console.WriteLine("Client disconnected");

			}

			catch (ThreadAbortException){
				client.Close();
				Console.WriteLine("Connection error...");
			}

			finally{
				Console.WriteLine("finally");
			}
		}






		public static List<String> messages = new List<string>();

		public static void Main ()
		{
			
			Console.WriteLine ("Starting echo server...");

			int port = 11000;
			int nameId = 0;
			IPAddress myIp = IPAddress.Parse ("192.168.0.11");

			TcpListener listener = new TcpListener (myIp, port);

			listener.Start ();

			while (true) { 
				TcpClient client = listener.AcceptTcpClient ();

				//sender thread created and started
				Thread senderThread = new Thread(sender);
				senderThread.Name = nameId.ToString();
				senderThread.Start(client);

				//reciever thread created and started
				Thread recieverThread = new Thread(reciever);
				recieverThread.Name = nameId.ToString();
				recieverThread.Start(client);

				nameId++;

			}
		}
	}
}

