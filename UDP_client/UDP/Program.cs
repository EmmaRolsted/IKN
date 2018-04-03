using System;
using System.Net; 
using System.Text; 
using System.Net.Sockets;

namespace UDP
{
	class UDP_Client
	{
		const int PORT = 9000; 

		private UDP_Client(string[] args) {
		
			if (args.Length != 2) {
				Console.WriteLine ("Error"); 
				Environment.Exit (1);
			}


			Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

			// New UDP client 
			var client = new UdpClient();

			//  Connect
			IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(args[0]), PORT);
			client.Connect(endPoint); 

			// Request to server
			byte[] sData = Encoding.ASCII.GetBytes(args[1]);
			client.Send (sData, sData.Length);

			// Receive from server 
			//byte[] recvData = client.Receive(ref endPoint); 
			//string data = Encoding.ASCII.GetString (recvData).ToLower(); 
			//Console.WriteLine (data);

			recvData (socket);


		}

		private void recvData(Socket socket) {
		
			Byte[] recvBytes = new byte[1000];
			socket.Receive (recvBytes); 
			string data = Encoding.ASCII.GetString (recvBytes).ToLower ();
			Console.WriteLine (data);
		
		
		}


			

		public static void Main (string[] args)
		{

			Console.WriteLine ("Client starting"); 
			new UDP_Client (args);
		
		}
	}
}
