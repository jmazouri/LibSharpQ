using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CSCore;
using CSCore.DSP;
using CSCore.SoundIn;
using CSCore.Streams;
using LibSharpQ.Models;

namespace LibSharpQ.Examples
{
    public static class Visualizer
    {
        private static WasapiCapture _soundIn;
        private static IDasClient _client;
        private static SingleBlockNotificationStream _stream;

        public static async Task Execute(IDasClient client)
        {
            _client = client;

            _soundIn = new WasapiLoopbackCapture();
            _soundIn.Initialize();

            SoundInSource soundInSource = new SoundInSource(_soundIn);

            //const FftSize fftSize = FftSize.Fft4096;

            _stream = new SingleBlockNotificationStream(soundInSource.ToSampleSource());
            _stream.SingleBlockRead += HandleEvent;

            var notificationWave = _stream.ToWaveSource(16);

            byte[] buffer = new byte[notificationWave.WaveFormat.BytesPerSecond / 2];
            soundInSource.DataAvailable += (s, aEvent) =>
            {
                int read;
                while ((read = notificationWave.Read(buffer, 0, buffer.Length)) > 0) ;
            };

            _soundIn.Start();

            await Task.Delay(-1);
        }

        static int rateLimit = 15000;
        static int count = 0;

        private static void HandleEvent(object sender, SingleBlockReadEventArgs e)
        {
            if (count < rateLimit)
            {
                count++;
                return;
            }
            else
            {
                count = 0;
            }

            float modified = MathF.Abs(e.Left + e.Right);

            int capped = (int)(255 * modified);
            capped = capped > 255 ? 255 : capped;
            capped = capped < 0 ? 0 : capped;

            var col = new Color(0, (byte)capped, 0);

            Console.WriteLine("Sending " + capped);

            //Current bug with the local Das API and concurrent/async requests...
            _client.SendSignal(new Signal("Music", (19, 0), col)).GetAwaiter().GetResult();
            _client.SendSignal(new Signal("Music", (20, 0), col)).GetAwaiter().GetResult();
            _client.SendSignal(new Signal("Music", (21, 0), col)).GetAwaiter().GetResult();
        }
    }
}
