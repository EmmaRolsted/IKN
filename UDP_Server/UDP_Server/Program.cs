using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace UDP
{
	class UDP_Server
	{
		const int PORT = 9000; 
		const int BUF = 1000;


		private UDP_Server()
		{

			UdpClient listener = new UdpClient (PORT); 
			IPEndPoint IP_Endpoint = new IPEndPoint (IPAddress.Any, PORT);

			while (true) {
			
				// Receiving bytes and saving IP
				byte[] recv_byte = listener.Receive (ref IP_Endpoint); 
				string recv_data = Encoding.ASCII.GetString (recv_byte, 0, recv_byte.Length);

				Console.WriteLine ("Received: {0}", recv_data); 

				if (recv_data.ToLower () != "l" && recv_data.ToLower () != "u") {
					string errorMsg = "Wrong Char";
					listener.Send (Encoding.ASCII.GetBytes(errorMsg), errorMsg.Length, IP_Endpoint);
				}
				else if (recv_data.ToLower () == "l") {
					// Reading
					string loadavg = System.IO.File.ReadAllText (@"/proc/loadavg"); 
					Console.WriteLine ("Sender loadavg...");
					// Sending
					listener.Send (Encoding.ASCII.GetBytes (loadavg), loadavg.Length, IP_Endpoint); 
				} 
				else if (recv_data.ToLower () == "u") {
					// Reading
					string uptime = System.IO.File.ReadAllText (@"/proc/uptime");
					Console.WriteLine ("Sender uptime..."); 
					// Sending
					listener.Send (Encoding.ASCII.GetBytes (uptime), uptime.Length, IP_Endpoint);
				}

			
			}
				
		}

		static void Main (string[] args)
		{
				Console.WriteLine ("Server starts..."); 
				new UDP_Server ();
		}
	}
}
