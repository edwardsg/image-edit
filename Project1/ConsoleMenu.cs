using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1
{
    class ConsoleMenu
    {
		private ImageProcessor processor;
		public ImageProcessor Processor
		{
			set
			{
				processor = value;
			}
		}
		
        public void ShowMenu()
        {
			string command = "";

			while (!command.Equals("9"))
			{
				Console.WriteLine("\nImage Editor\n");

				Console.WriteLine("1. Resize");
				Console.WriteLine("2. Contrast");
				Console.WriteLine("3. Brightness");
				Console.WriteLine("4. Saturation");
				Console.WriteLine("5. Blur");
				Console.WriteLine("6. Sharpen");
				Console.WriteLine("7. Edge Detection");
				Console.WriteLine("8. Save File");
				Console.WriteLine("9. Quit");

				Console.Write("\nEnter command: ");

				command = Console.ReadLine();

				if (command.Equals("1"))
				{
					Console.Write("Enter new width: ");
					int width = int.Parse(Console.ReadLine());
					Console.Write("Enter new height: ");
					int height = int.Parse(Console.ReadLine());

					processor.Resize(width, height);

					Console.WriteLine("Resize succeeded.");
				}

				if (command.Equals("2"))
				{
					double contrast = 3;
					while (contrast < 0 || contrast > 2)
					{
						Console.Write("Enter contrast change: ");
						contrast = double.Parse(Console.ReadLine());
						if (contrast < 0 || contrast > 2)
						{
							Console.WriteLine("Invalid input. Please input a contrast between 0 and 2");
						}
					}

					processor.Contrast(contrast);

					Console.WriteLine("Contrast succeeded.");
				}

				if (command.Equals("3"))
				{
					double brightness;
					do
					{
						Console.Write("Enter brightness change: ");
						brightness = double.Parse(Console.ReadLine());
						if (brightness < 0)
						{
							Console.WriteLine("Invalid input. Please input a brightness greater than 0");
						}
					} while (brightness < 0);

					processor.Brighten(brightness);

					Console.WriteLine("Brightness succeeded.");
				}

				if (command.Equals("4"))
				{
					double saturation;
					do
					{
						Console.Write("Enter saturation change: ");
						saturation = double.Parse(Console.ReadLine());
						if (saturation < 0)
						{
							Console.WriteLine("Invalid input. Please input a saturation greater than 0");
						}
					} while (saturation < 0);

					processor.Saturate(saturation);

					Console.WriteLine("Saturation succeeded.");
				}

				if (command.Equals("5"))
				{



					Console.WriteLine("Blur succeeded.");
				}

				if (command.Equals("6"))
				{



					Console.WriteLine("Sharpen succeeded.");
				}

				if (command.Equals("7"))
				{



					Console.WriteLine("Edge Detection succeeded.");
				}

				if (command.Equals("8"))
				{
					Console.Write("Enter file name: ");
					String savename = Console.ReadLine();



					Console.WriteLine("Saving file succeeded.");
				}

				if (command.Equals("9"))
				{



					Console.WriteLine("Quitting...");
				}
			}

			processor.Exit();
        }
    }
}
