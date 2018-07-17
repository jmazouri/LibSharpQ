using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LibSharpQ.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace LibSharpQ.Examples
{
    public class ImageLoader
    {
        public static async Task Execute(IDasClient client)
        {
            Console.WriteLine();
            Console.WriteLine("This will transpose the top left 24x6 pixels of the image to the keyboard");
            Console.WriteLine("Pick an image: ");

            var imageFiles = Directory.GetFiles("images");

            for (int i = 0; i < imageFiles.Length; i++)
            {
                Console.WriteLine($"{i}. {Path.GetFileName(imageFiles[i])}");
            }

            int index;

            while (!int.TryParse(Console.ReadLine(), out index))
            {
                Console.WriteLine("Invalid selection - please choose an index");
            }

            if (index < 0 || index > imageFiles.Length - 1)
            {
                Console.WriteLine($"Invalid selection - please choose a number between 0 and {imageFiles.Length - 1}");
            }

            await LoadImage(client, Image.Load(imageFiles[index]));
        }

        private static async Task LoadImage(IDasClient client, Image<Rgba32> image)
        {
            int xMax = image.Width > 24 ? 24 : image.Width;
            int yMax = image.Height > 6 ? 6 : image.Height;

            for (int x = 0; x < xMax; x++)
                for (int y = 0; y < yMax; y++)
                {
                    var color = image[x, y];
                    if (color.A <= 200)
                    {
                        continue;
                    }

                    await client.SendSignal(new Signal("Image", (x, y), new Color(color.R, color.G, color.B)));
                }
        }
    }
}
