using System;
using System.Threading.Tasks;
using Xunit;
using ClassLibrary;
using MiffyLiye.XConsole;

namespace Tests
{
    public class Tests
    {
        private string NewLine => Environment.NewLine;

        [Fact]
        public void test_reader_send_line_and_writer_output() 
        {
            var xReader = new XTextReader();
            var xWriter = new XTextWriter();
            var runner = new Library(xReader, xWriter);
            var task = Task.Run(() => runner.Run());

            Wait.Until(() => xReader.PendingRead);
            
            Assert.Equal(
                "Welcome!" + NewLine,
                xWriter.Output);
            
            var lastOutput = xWriter.Output;
            var lastOutputLinesCount = xWriter.OutputLines.Count;

            xReader.SendLine("hello");
            Wait.Until(() => xWriter.OutputLines.Count > lastOutputLinesCount);
            Wait.Until(() => xReader.PendingRead);
            
            Assert.Equal(
                lastOutput 
                + "hello" + NewLine, 
                xWriter.Output);

            lastOutput = xWriter.Output;
            lastOutputLinesCount = xWriter.OutputLines.Count;

            xReader.SendLine("");
            task.Wait();

            Assert.Equal(
                lastOutput 
                + "Goodbye!" + NewLine, 
                xWriter.Output);            
        }
    }
}
