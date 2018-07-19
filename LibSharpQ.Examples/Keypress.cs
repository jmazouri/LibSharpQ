using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LibSharpQ.Models;

namespace LibSharpQ.Examples
{
    public static class Keypress
    {
        public static async Task Execute(IDasClient client)
        {
            ConsoleKey key = ConsoleKey.A;
            Console.WriteLine("Type keys in the console window and watch them glow!");
            Console.WriteLine("Hit escape to quit");

            while (key != ConsoleKey.Escape)
            {
                var input = Console.ReadKey();
                key = input.Key;

                if (key == ConsoleKey.Escape) { continue; }

                char inputChar = Char.ToUpper(input.KeyChar);
                var signal = new Signal("Hotspot", "KEY_" + inputChar, new Color(255, 0, 0));

                var result = await client.SendSignal(signal);
                await Task.Delay(1000);
                await client.DeleteSignal(result.Id);
            }
        }
    }
}
