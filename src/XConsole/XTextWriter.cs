using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MiffyLiye.XConsole
{
    public sealed class XTextWriter : TextWriter
    {
        public override Encoding Encoding => Encoding.ASCII;
        private readonly object _outputMutex = new object();
        private readonly StringBuilder _outputBuilder = new StringBuilder();
        
        public string Output
        {
            get
            {
                lock (_outputMutex)
                {
                    return _outputBuilder.ToString();
                }
            }
        }

        public IReadOnlyList<string> OutputLines
        {
            get
            {
                lock (_outputMutex)
                {
                    return _outputBuilder.ToString()
                        .Split(new[] { NewLine }, StringSplitOptions.None);
                }
            }
        }

        public override void Write(string value)
        {
            lock (_outputMutex)
            {
                _outputBuilder.Append(value);
            }
        }

        public override void WriteLine(string value)
        {
            Write(value + Environment.NewLine);
        }

        public override void WriteLine()
        {
            WriteLine("");
        }

        public void ClearOutput()
        {
            lock (_outputMutex)
            {
                _outputBuilder.Clear();
            }
        }

        public override void Write(char c)
        {
            throw new NotImplementedException();
        }
    }
}
