using System;
using MiffyLiye.XConsole;
using Xunit;

namespace XConsoleTest
{
    public class XTextWriterTest
    {
        [Theory]
        [InlineData("hello")]
        [InlineData("hello\n")]
        [InlineData("hello"+"\r\n")]
        [InlineData("hello"+"\n"+"goodbye")]
        [InlineData("hello"+"\r\n"+ "goodbye")]
        public void should_get_output_when_write(string value)
        {
            var textWriter = new XTextWriter();

            textWriter.Write(value);

            Assert.Equal(value, textWriter.Output);
        }

        [Theory]
        [InlineData("hello")]
        [InlineData("hello\n")]
        [InlineData("hello" + "\r\n")]
        [InlineData("hello" + "\n" + "goodbye")]
        [InlineData("hello" + "\r\n" + "goodbye")]
        public void should_get_output_when_write_line(string value)
        {
            var textWriter = new XTextWriter();

            textWriter.WriteLine(value);

            Assert.Equal(value + Environment.NewLine, textWriter.Output);
        }

        [Fact]
        public void should_get_empty_line_when_write_line_void()
        {
            var textWriter = new XTextWriter();

            textWriter.WriteLine();

            Assert.Equal("" + Environment.NewLine, textWriter.Output);
        }

        [Fact]
        public void should_get_output_lines()
        {
            var textWriter = new XTextWriter();

            textWriter.Write("hello" + Environment.NewLine + "goodbye");
            var outputLines = textWriter.OutputLines;

            Assert.Equal(new[] {"hello", "goodbye"}, outputLines);
        }

        [Theory]
        [InlineData("hello")]
        [InlineData("hello\n")]
        [InlineData("hello" + "\r\n")]
        [InlineData("hello" + "\n" + "goodbye")]
        [InlineData("hello" + "\r\n" + "goodbye")]
        public void should_get_empty_output_when_clear_output(string value)
        {
            var textWriter = new XTextWriter();

            textWriter.WriteLine(value);
            textWriter.ClearOutput();

            Assert.Equal(string.Empty, textWriter.Output);
        }
    }
}