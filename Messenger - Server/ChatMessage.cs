using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger___Server
{
    public class ChatMessage
    {
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }

        public ChatMessage(string content)
        {
            Content = content;
            TimeStamp = DateTime.Now;
        }

        public override string ToString()
        {
            return $"[{TimeStamp}] {Content}";
        }
    }
}