using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace tcp
{
	class file_server
	{
		/// <summary>
		/// The PORT
		/// </summary>
		const int PORT = 9000;
		/// <summary>
		/// The BUFSIZE
		/// </summary>
		const int BUFSIZE = 1000;

		/// <summary>
		/// Initializes a new instance of the <see cref="file_server"/> class.
		/// Opretter en socket.
		/// Venter på en connect fra en klient.
		/// Modtager filnavn
		/// Finder filstørrelsen
		/// Kalder metoden sendFile
		/// Lukker socketen og programmet
 		/// </summary>
		private file_server ()
		{		
			string filePath = string.Empty;
			long fileSizeLong = 0;
			string fileSizeStr = string.Empty;

			TcpListener ServerSocket = new TcpListener (PORT);
			TcpClient ClientSocket = default(TcpClient);
			ServerSocket.Start ();
			Console.WriteLine("Server started");
			ClientSocket = ServerSocket.AcceptTcpClient ();
			Console.WriteLine ("Accepted connection");

			NetworkStream networkStream = ClientSocket.GetStream();
			filePath = LIB.readTextTCP (networkStream);
			Console.WriteLine ("The client wants " + filePath);

			//Gets file size, and checks whether it exists
			fileSizeLong = LIB.check_File_Exists (filePath);
			fileSizeStr = fileSizeLong.ToString ();
			LIB.writeTextTCP (networkStream, fileSizeStr);

			while (fileSizeLong == 0) {
				Console.WriteLine ("Error, file does not exist");
				//Writes size to client as string

				filePath = LIB.readTextTCP (networkStream);
				Console.WriteLine (filePath);

				//Gets file size, and checks whether it exists
				fileSizeLong = LIB.check_File_Exists (filePath);
				LIB.writeTextTCP (networkStream, fileSizeStr);

			}

			Console.WriteLine ("The size of the file is {0} bytes", fileSizeLong);

			//sendFile(filename, size,
			sendFile (filePath, fileSizeLong, networkStream);

			ClientSocket.Close ();
			ServerSocket.Stop ();
		}

		/// <summary>
		/// Sends the file.
		/// </summary>
		/// <param name='fileName'>
		/// The filename.
		/// </param>
		/// <param name='fileSize'>
		/// The filesize.
		/// </param>
		/// <param name='io'>
		/// Network stream for writing to the client.
		/// </param>
		private void sendFile (String fileName, long fileSize, NetworkStream io)
		{
			
			//Kan bøvle med hele stien!!
			Console.WriteLine (fileName);

			FileStream Fs = new FileStream (fileName, FileMode.Open, FileAccess.Read);

			Byte[] array = new Byte[BUFSIZE]; 
			int bytesRead = Fs.Read(array, 0, BUFSIZE);
			io.Write (array, 0, bytesRead);
			//Whileloop fortsætter, så længe der er bytes at sende (fra fil)
			while(bytesRead > 0)
			{
				bytesRead = Fs.Read(array, 0, BUFSIZE);
				io.Write (array, 0, bytesRead);
				//Console.WriteLine (bytesRead);
			}
			Console.WriteLine ("File sent");
			Fs.Close ();
		}

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main (string[] args)
		{
			Console.WriteLine ("Server starts...");
			while (true) {
				new file_server ();
			}
		}
	}
}
