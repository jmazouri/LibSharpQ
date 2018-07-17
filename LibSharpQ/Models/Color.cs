using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LibSharpQ.Models
{
    public class Color
    {
        private string _hexCode;

        [JsonProperty("code")]
        public string HexCode
        {
            get => _hexCode;
            set
            {
                _hexCode = value;
                CalculateRgb();
            }
        }

        public string Name { get; set; }
        public (byte r, byte g, byte b) Rgb { get; private set; }

        private void CalculateRgb()
        {
            //Taken from
            //https://github.com/dotnet/corefx/blob/master/src/System.Drawing.Common/src/System/Drawing/ColorTranslator.cs

            if (HexCode.Length == 7)
            {
                Rgb =
                (
                    Convert.ToByte(HexCode.Substring(1, 2), 16),
                    Convert.ToByte(HexCode.Substring(3, 2), 16),
                    Convert.ToByte(HexCode.Substring(5, 2), 16)
                );
            }
            else if (HexCode.Length == 4)
            {
                string r = char.ToString(HexCode[1]);
                string g = char.ToString(HexCode[2]);
                string b = char.ToString(HexCode[3]);

                Rgb = (Convert.ToByte(r + r, 16), Convert.ToByte(g + g, 16), Convert.ToByte(b + b, 16));
            }
            else
            {
                Rgb = (0, 0, 0);
            }
        }

        public Color(string hexCode)
        {
            HexCode = hexCode;
        }

        public Color(byte r, byte g, byte b)
        {
            _hexCode = $"#{r:x2}{g:x2}{b:x2}".ToUpper();
            Rgb = (r, g, b);
        }
        
        public Color()
        {
            HexCode = "#000";
        }
    }
}
