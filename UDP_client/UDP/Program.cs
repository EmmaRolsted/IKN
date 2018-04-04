using System;
using System.Net; 
using System.Text; 
using System.Net.Sockets;

namespace UDP
{
	class UDP_Client
	{
		
		const int PORT = 9000; 
		const int BUF = 1000; 


		private UDP_Client(string[] args) {

			string ip = args[0]; 
			string string_send = args[1]; 

			Socket socket = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); 
			IPAddress address = IPAddress.Parse (ip); 
			IPEndPoint endPoint = new IPEndPoint (address, PORT); 


			// Sending via socket
			byte[] send_buffer = Encoding.ASCII.GetBytes (string_send); 
			socket.SendTo (send_buffer, endPoint); 


			// Receiving 
			byte[] recvdata = new byte[BUF]; 
			socket.Receive (recvdata); 
			Console.WriteLine ("Received input: {0}", Encoding.ASCII.GetString (recvdata));

		}


		public static void Main (string[] args)
		{

			Console.WriteLine ("Client starts..."); 
			new UDP_Client (args);
		
		}
	}
}
