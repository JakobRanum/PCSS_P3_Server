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
		//global list containing all messages in the chatroom
		public static List<String> messages = new List<string>();


		//function run by the threads for sending messages from the client
		public static void sender(object argument)
		{
			TcpClient client = (TcpClient)argument;

			try
			{
				StreamWriter writer = new StreamWriter(client.GetStream(), Encoding.ASCII) { AutoFlush = true };

				int messagesSend = 0;



				foreach (var message in messages)
				{ //sending old messages.
					messagesSend++;
					writer.WriteLine(message);
				}

				while (client.Connected)
				{
					//if new messages are added to the messages list by other clients they are pulled to this client as well
					if (messages.Count > messagesSend)
					{
						try
						{
							writer.WriteLine(messages.ElementAt(messages.Count - 1));
							messagesSend++;
						}

						catch
						{
							messagesSend++;
							Console.WriteLine("error, client disconnected");
						}
					}
				}
					
			}

			catch (ThreadAbortException)
			{
				client.Close();
				Console.WriteLine("Connection error...");
				Thread.Sleep(2000);
			}

		}

		//function run by the threads for recieving messages from the client
		public static void reciever(object argument)
		{
			TcpClient client = (TcpClient)argument;


			try
			{
				StreamReader reader = new StreamReader(client.GetStream(), Encoding.ASCII);
				Console.WriteLine("Now listening to client");

				while (client.Connected)
				{
					if (true)
					{
						try
						{
							string message = reader.ReadLine();

							if (message != null)
							{
								messages.Add(message);
								Console.WriteLine(messages.ElementAt(messages.Count - 1));
							}
						}

						catch
						{
							reader.Close();
							client.Close();
							Console.WriteLine("Connection Error");
							break;
						}
					}
				}
				Console.WriteLine("Client disconnected");
			}

			catch (ThreadAbortException)
			{
				client.Close();
				Console.WriteLine("Connection error...");
			}
		}

		//main executing the server program
		public static void Main()
		{
			//adding welcome message to messages list
			messages.Add("Welcome to the P3 chatroom");

			TcpListener listener;
			try
			{
				string currentIP = "";

				foreach (var addr in Dns.GetHostEntry(string.Empty).AddressList)
				{
					//current ip adress retrieved
					if (addr.AddressFamily == AddressFamily.InterNetwork)
					{
						currentIP = addr.ToString();
					}
				}

				Console.WriteLine("Starting echo server on IP: {0}", currentIP);

				int port = 11000;
				int nameId = 0;

				IPAddress myIp = IPAddress.Parse(currentIP);
				listener = new TcpListener(myIp, port);
				listener.Start();

				Console.WriteLine("Server started, listening for clients on port: '{0}'", port);

				//Multi threading for multiple clients
				while (true)
				{
					//listening for new clients
					TcpClient client = listener.AcceptTcpClient();

					//sender thread created
					Thread senderThread = new Thread(sender);

					//sender thread is named for reference
					senderThread.Name = nameId.ToString();

					//sender thread started with the newly connected client as argument
					senderThread.Start(client);

					//reciever thread created
					Thread recieverThread = new Thread(reciever);

					//reciever thread is named for reference
					recieverThread.Name = nameId.ToString();

					//reciever thread started with the newly connected client as argument
					recieverThread.Start(client);

					//nametag iterated
					nameId++;
				}
			}

			catch
			{

				Console.WriteLine("Could not start server");
			}
		}
	}
}