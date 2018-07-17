using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LibSharpQ.Models;

namespace LibSharpQ.Examples
{
    public static class FKeyRainbow
    {
        private static readonly string[] _colors = 
            new string[] { "#9400D3", "#4B0082", "#0000FF", "#00FF00", "#FFFF00", "#FF7F00", "#FF0000" };

        public static async Task Execute(IDasClient client)
        {
            Console.WriteLine("Rainbow time!");

            List<int> sentSignalIds = new List<int>();

            //From the F1 key to the F12 key
            for (int i = 3; i <= 15; i++)
            {
                //Double up the index since we only have enough colors
                //for half the keys
                int colorIndex = (i - 3) / 2;

                var signal = new Signal("Rainbow!", (i, 0), _colors[colorIndex]);
                var sent = await client.SendSignal(signal);

                sentSignalIds.Add(sent.Id);
            }

            Console.WriteLine("Rainbow sent! Push enter to clear");
            Console.ReadLine();

            foreach (var id in sentSignalIds)
            {
                await client.DeleteSignal(id);
            }
        }
    }
}
