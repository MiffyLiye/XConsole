using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MiffyLiye.XConsole
{
    public sealed class XTextReader : TextReader
    {
        private readonly object _consumeMutex = new object();
        private readonly object _produceMutex = new object();        
        private readonly ConcurrentQueue<char> _pendingCharacters = new ConcurrentQueue<char>();
        private TimeSpan _timeOutLimit;
        private readonly TimeSpan _retryInterval = TimeSpan.FromSeconds(0.1);
        public bool PendingRead { get; private set; }

        public XTextReader(TimeSpan timeOutLimit = default(TimeSpan))
        {
            _timeOutLimit = timeOutLimit == default(TimeSpan) ? TimeSpan.FromSeconds(3) : timeOutLimit;
        }

        public void ResetTimeOutLimit(TimeSpan timeOutLimit)
        {
            _timeOutLimit = timeOutLimit;
        }

        public void Send(string value)
        {
            lock(_produceMutex){
                foreach (var c in value)
                {
                    _pendingCharacters.Enqueue(c);
                }
            }
        }
        
        public void SendLine(string value)
        {
            if (value.Contains("\n"))
            {
                throw new InvalidDataException("Input cannot contain multiple lines.");
            }
            Send(value + Environment.NewLine);
        }

        public override string ReadLine()
        {
            var chars = new List<char>();
            lock (_consumeMutex)
            {
                PendingRead = true;
                while (true){
                    Wait.Until(
                        () => _pendingCharacters.Any(), 
                        retryLimit: (int)(1 + _timeOutLimit.TotalMilliseconds / _retryInterval.TotalMilliseconds), 
                        retryInterval: _retryInterval,
                        label: "Waiting inputs");

                    char c;
                    if (_pendingCharacters.TryDequeue(out c))
                    {
                        if (c == '\n') {
                            break;
                        }
                        chars.Add(c);
                    }
                }
                PendingRead = false;
            }
            var result = chars.Aggregate("", (s, e) => s + e);

            if (Environment.NewLine.Contains('\r') && result.EndsWith("\r")) {
                return result.Substring(0, result.Length - 1);
            }
            return result;
        }

        public override int Read()
        {
            throw new NotImplementedException();
        }
    }
}