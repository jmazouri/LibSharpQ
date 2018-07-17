using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibSharpQ.Models;

namespace LibSharpQ.Examples
{
    class Program
    {
        private static bool quit = false;
        private static IDasClient _client;

        static async Task Main(string[] args)
        {
            //All the demos will use the local API
            _client = new LocalDasClient();

            Console.WriteLine("Local Das Client started, endpoint: " + _client.BaseUrl);

            while (!quit)
            {
                await PresentOptions();
            }
        }

        static async Task PresentOptions()
        {
            Console.WriteLine("Select a demo: ");
            Console.WriteLine("1. Rainbow F-Keys");
            Console.WriteLine("2. Snake");
            Console.WriteLine("3. Audio Visualizer");
            Console.WriteLine("4. Image Loader");

            Console.WriteLine("8. Clear All Signals");
            Console.WriteLine("9. Quit");

            Console.Write("> ");

            int.TryParse(Console.ReadLine(), out int index);

            switch (index)
            {
                case 1:
                    await FKeyRainbow.Execute(_client);
                    break;
                case 2:
                    await ChasingSnake.Execute(_client);
                    break;
                case 3:
                    await Visualizer.Execute(_client);
                    break;
                case 4:
                    await ImageLoader.Execute(_client);
                    break;
                case 8:
                    await _client.DeleteAllSignals();
                    break;
                case 9:
                    quit = true;
                    break;
                default:
                    Console.WriteLine("I don't recognize that option");
                    break;
            }
        }
    }
}
