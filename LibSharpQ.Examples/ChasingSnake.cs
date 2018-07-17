using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LibSharpQ.Models;

namespace LibSharpQ.Examples
{
    public class ChasingSnake
    {
        private static Queue<int> _lastSignals = new Queue<int>(2);

        public static async Task Execute(IDasClient client)
        {
            //A bunch of basic loops to iterate through the
            //keys on the border of the keyboard

            //Tilde to Backspace
            for (int i = 1; i <= 15; i++)
            {
                await SetLed(client, i, 1);
            }

            //Backspace to R-ctrl
            for (int i = 2; i <= 5; i++)
            {
                await SetLed(client, 15, i);
            }

            //R-ctrl to L-ctrl
            for (int i = 14; i >= 1; i--)
            {
                if (i == 10)
                {
                    i -= 5;
                }

                await SetLed(client, i, 5);
            }

            //L-ctrl to Tilde
            for (int i = 4; i >= 1; i--)
            {
                await SetLed(client, 1, i);
            }

            //Clean up after ourselves
            await client.DeleteSignals(_lastSignals);
        }

        private static async Task SetLed(IDasClient client, int x, int y)
        {
            //Keep a "trail" of 2 keys
            if (_lastSignals.Count > 2)
            {
                await client.DeleteSignal(_lastSignals.Dequeue());
            }

            var signal = new Signal("Snake", (x, y), new Color("#F00"));
            _lastSignals.Enqueue((await client.SendSignal(signal)).Id);

            //We need a slight delay, because the built-in transition
            //animations on signals is too slow
            await Task.Delay(600);
        }
    }
}
