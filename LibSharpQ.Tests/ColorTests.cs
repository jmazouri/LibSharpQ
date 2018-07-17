using System;
using System.Collections.Generic;
using System.Text;
using LibSharpQ.Models;
using Xunit;

namespace LibSharpQ.Tests
{
    public class ColorTests
    {
        [Fact]
        public void TestThreeDigitHexParsing()
        {
            Color color = new Color("#F00");

            Assert.Equal(255, color.Rgb.r);
            Assert.Equal(0, color.Rgb.g);
            Assert.Equal(0, color.Rgb.b);
        }

        [Fact]
        public void TestSixDigitHexParsing()
        {
            Color color = new Color("#00FF00");

            Assert.Equal(0, color.Rgb.r);
            Assert.Equal(255, color.Rgb.g);
            Assert.Equal(0, color.Rgb.b);
        }

        [Fact]
        public void TestHexFormatting()
        {
            Color color = new Color(0, 0, 255);

            Assert.Equal("#0000FF", color.HexCode);
        }
    }
}
