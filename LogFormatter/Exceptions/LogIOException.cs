using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogFormatter.Exceptions
{
    public class LogIOException: Exception
    {
        public string Message = string.Empty;
        public string Source = string.Empty;
        
        public LogIOException(string message, string source) : base(message)
        {
            Message = message;
            Source = source;
        }
    }
}
