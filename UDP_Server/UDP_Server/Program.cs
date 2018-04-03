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

		private UDP_Server()
		{
			var server = new UdpClient (PORT); 
			// End point
			IPEndPoint EndPoint = new IPEndPoint (IPAddress.Any, PORT); 


			while (true) {
			
				// Receive from client 
				byte[] recvData = server.Receive (ref EndPoint);
				string data = Encoding.ASCII.GetString (recvData);


				Console.WriteLine ("Received from client: " + data); 


				// Get info to client 
				string m = GetMeasurement (data);


				// Send back 
				byte[] ReturnData = Encoding.ASCII.GetBytes (m); 
				server.Send (ReturnData, ReturnData.Length, EndPoint); 

			}
		}


		public string GetMeasurement(string client_letter){

			string Path; 

			switch (client_letter = client_letter.ToUpper())
			{

			case "L":
				Path = "/proc/loadavg"; 
				Console.WriteLine ("Loadavg"); 
				break; 
			case "U": 
				Path = "/proc/uptime"; 
				Console.WriteLine ("Uptime"); 
				break; 
			default: 
				Console.WriteLine ("Error");
				return "Unvalid input. Valid inputs: l or u\n"; 

			}
			return "Reading" + Path + File.ReadAllText (Path);

		}


		static void Main (string[] args)
		{
			while (true) {
				Console.WriteLine ("Server starting"); 
				new UDP_Server ();
			}
			
		}
	}
}
