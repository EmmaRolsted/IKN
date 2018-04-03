using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace tcp
{
	class file_client
	{
		/// <summary>
		/// The PORT.
		/// </summary>
		const int PORT = 9000;
		/// <summary>
		/// The BUFSIZE.
		/// </summary>
		const int BUFSIZE = 1000;

		/// <summary>
		/// Initializes a new instance of the <see cref="file_client"/> class.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments. First ip-adress of the server. Second the filename
		/// </param>
		private file_client (string[] args)
		{
			TcpClient ClientSocket = new TcpClient ();
			try
			{
				ClientSocket.Connect(args[0], PORT);
				Console.WriteLine("Connected to {0}, port {1}", args[0], PORT);
			}
			catch
			{
				Console.WriteLine ("No connection");
			}
			//Get path and name from user
			//Console.WriteLine ("What with pathname would you like?");
			//string path = Console.ReadLine ();
			string filePath = args[1];

			//Writes to server
			NetworkStream serverStream = ClientSocket.GetStream();
			LIB.writeTextTCP (serverStream, filePath);

			long fileSize = LIB.getFileSizeTCP (serverStream);
			while (fileSize == 0) {
				Console.WriteLine ("File not found. Input a valid file");
				Console.WriteLine ("Enter new file: ");
				filePath = Console.ReadLine ();

				Console.WriteLine($"Requesting filename '{filePath}'");

				LIB.writeTextTCP (serverStream, filePath);
				fileSize = LIB.getFileSizeTCP (serverStream);
			}
			Console.WriteLine ("File does exist!");
			//Calls receivefile
			//string filename = LIB.extractFileName (path);
			receiveFile (filePath, serverStream);
		}
		/*private file_client (string[] args)
		{
			string fileSizeStr = "";
			long fileSizeLong = 0;
			string filePath = string.Empty;

			TcpClient ClientSocket = new TcpClient ();
			try
			{
				ClientSocket.Connect("10.0.0.2", PORT);
				Console.WriteLine("Connected to 10.0.0.2, port {0}", PORT);
			}
			catch
			{
				Console.WriteLine ("No connection");
			}

			Console.WriteLine ("What with pathname would you like?");
			filePath = Console.ReadLine();

			NetworkStream serverStream = ClientSocket.GetStream();
			LIB.writeTextTCP (serverStream, filePath);
			//Get path and name from user

			//Console.WriteLine (filePath);

			//Console.WriteLine ("Written to server");

			//fileSize = LIB.getFileSizeTCP(serverStream);
			fileSizeStr = LIB.readTextTCP(serverStream);
			fileSizeLong = Convert.ToInt64 (fileSizeStr);
			//fileSizeLong = int.Parse (fileSizeStr);
			Console.WriteLine (fileSizeStr);

			while(fileSizeLong == 0) {
				Console.WriteLine ("File does not exists");
				Console.WriteLine ("What with pathname would you like?");
				filePath = Console.ReadLine ();
				LIB.writeTextTCP (serverStream, filePath);
				fileSizeStr = LIB.readTextTCP(serverStream);
				fileSizeLong = int.Parse (fileSizeStr);
			} 

			Console.WriteLine (fileSizeLong);
				
			//Calls receivefile
			//string filename = LIB.extractFileName (path);
			receiveFile (filePath, serverStream);
		}
		*/
		/// <summary>
		/// Receives the file.
		/// </summary>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		/// <param name='io'>
		/// Network stream for reading from the server
		/// </param>
		private void receiveFile (String filePath, NetworkStream io)
		{
			string fileName = string.Empty;
			string fileDirectory = string.Empty;
			long fileSize = 0;
			int bytesRead = 0;
			byte[] buffer = new byte[BUFSIZE];

			//Receive size in string 
			//string sizeStr = LIB.readTextTCP(io);
			//long size = Convert.ToInt64 (sizeStr);

			//Console.WriteLine ("Size is: {0}", size);

			//Create directory
			//fileDirectory = "/root/Desktop/";
			//Directory.CreateDirectory (fileDirectory);

			//Get filename
			//fileName = LIB.extractFileName (filePath);

			//Skal modtage hvad server sender, og whileloop, der afhænger af antal bytes der modtages 
			fileName = LIB.extractFileName(filePath);
			//string directory = "/root/Desktop/Files/";
			//Directory.CreateDirectory (directory);

			FileStream Fs = new FileStream (filePath, FileMode.OpenOrCreate, FileAccess.Write);

			Console.WriteLine ("Reading file " + fileName + "...");

			do
			{
				bytesRead = io.Read (buffer, 0, BUFSIZE);
				Fs.Write(buffer, 0, bytesRead);
				Console.WriteLine("Read bytes: " + bytesRead.ToString());
			}while (bytesRead > 0);


			//Lukker fil
			Fs.Close ();
		}

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main (string[] filename)
		{
			Console.WriteLine ("Client starts...");
			new file_client (filename);
		}

	}
}
