using System;
using System.IO;
using System.Threading.Tasks;
using MiffyLiye.XConsole;
using Xunit;

namespace XConsoleTest
{
    public class XTextReaderTest
    {
        [Fact]
        public void should_wait_until_allow_read_line_when_read_line()
        {
            var textReader = new XTextReader();
            Assert.False(textReader.PendingRead);

            string actual = null;
            var task = Task.Run(() => actual = textReader.ReadLine());
            
            Wait.Until(() => textReader.PendingRead);
            Assert.Null(actual);

            textReader.SendLine("hello");
            task.Wait();

            Assert.False(textReader.PendingRead);
            Assert.Equal("hello", actual);
        }

        [Fact]
        public void should_read_multiple_times()
        {
            var textReader = new XTextReader();

            string actual = null;
            var task = Task.Run(
                () => 
                {
                    textReader.ReadLine(); 
                    actual = textReader.ReadLine();
                });
            
            Wait.Until(() => textReader.PendingRead);
            Assert.Null(actual);

            textReader.SendLine("hello");
            textReader.SendLine("goodbye");            
            task.Wait();

            Assert.False(textReader.PendingRead);
            Assert.Equal("goodbye", actual);
        }

        [Theory]
        [InlineData("hello"+"\n"+"goodbye")]
        [InlineData("hello"+"\n")]
        [InlineData("hello"+"\r\n"+"goodbye")]
        [InlineData("hello"+"\r\n")]
        public void should_throw_exception_when_input_has_line_breaks(string value)
        {
            var xReader = new XTextReader();

            Assert.Throws<InvalidDataException>(() => xReader.SendLine(value));
        }

        [Fact]
        public void should_throw_exception_when_read_time_out()
        {
            var xReader = new XTextReader(TimeSpan.FromSeconds(0.5));

            var start = DateTime.Now;
            Assert.Throws<TimeoutException>(() => xReader.ReadLine());
            var end = DateTime.Now;

            Assert.True(end - start > TimeSpan.FromSeconds(0.5));
            Assert.True(end - start < TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void should_throw_exception_when_read_time_out_after_reset()
        {
            var xReader = new XTextReader(TimeSpan.FromSeconds(3));
            xReader.ResetTimeOutLimit(TimeSpan.FromSeconds(0.1));

            var start = DateTime.Now;
            Assert.Throws<TimeoutException>(() => xReader.ReadLine());
            var end = DateTime.Now;

            Assert.True(end - start > TimeSpan.FromSeconds(0.1));
            Assert.True(end - start < TimeSpan.FromSeconds(0.5));
        }
    }
}
