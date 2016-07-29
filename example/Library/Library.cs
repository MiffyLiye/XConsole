using System.IO;

namespace ClassLibrary
{
    public class Library
    {
        private readonly TextReader _textReader;
        private readonly TextWriter _textWriter;

        public Library(TextReader textReader, TextWriter textWriter)
        {
            _textReader = textReader;
            _textWriter = textWriter;
        }

        public void Run()
        {    
            _textWriter.WriteLine("Welcome!");
            
            var input = _textReader.ReadLine();
            _textWriter.WriteLine(input);

            _textReader.ReadLine();
            _textWriter.WriteLine("Goodbye!");
        }
    }
}
