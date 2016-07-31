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
    #region Usages
        #region Start
            var xReader = new XTextReader();
            var xWriter = new XTextWriter();
            var runner = new Library(xReader, xWriter);
            var task = Task.Run(() => runner.Run());
        #endregion

        #region First see welcome message
            var lastOutputLinesCount = 0;
            Wait.Until(() => xWriter.OutputLines.Count > lastOutputLinesCount);
            Wait.Until(() => xReader.PendingRead);
            
            var expectedOutput = "Welcome!" + NewLine;
            Assert.Equal(expectedOutput, xWriter.Output);
        #endregion
            
        #region Send input then see output
            lastOutputLinesCount = xWriter.OutputLines.Count;

            xReader.SendLine("hello");
            Wait.Until(() => xWriter.OutputLines.Count > lastOutputLinesCount);
            Wait.Until(() => xReader.PendingRead);
            
            expectedOutput += "hello" + NewLine;
            Assert.Equal(expectedOutput, xWriter.Output);
        #endregion

        #region Send enter then exit
            xReader.SendLine("");
            Wait.Until(() => task.IsCompleted);
            
            expectedOutput += "Goodbye!" + NewLine;
            Assert.Equal(expectedOutput, xWriter.Output); 
        #endregion
    #endregion          
        }
    }
}
