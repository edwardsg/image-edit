using System;
using System.Threading;

namespace Project1
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class ImageEditor
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
			ConsoleMenu menu = new ConsoleMenu();
			ImageProcessor processor = new ImageProcessor(); // Game class

			// Get image file name from user and give to image processor
			Console.Write("Enter file name: ");
			string fileName = Console.ReadLine();
			processor.FileName = fileName;

			// Give console access to image processor
			menu.Processor = processor;

			// Give menu and processing separate threads
			Thread consoleThread = new Thread(new ThreadStart(menu.ShowMenu));
			Thread processorThread = new Thread(new ThreadStart(processor.Run));

			consoleThread.Start();
			processorThread.Start();

			consoleThread.Join();
			processorThread.Join();

			processor.Dispose();
        }
    }
#endif
}
