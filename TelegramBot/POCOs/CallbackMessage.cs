using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.POCOs
{
    class CallbackMessage
    {
        public int message_id { get; set; }
        public CallbackMessageFrom from { get; set; }
        public CallbackMessageChat chat { get; set; }
        public int date { get; set; }
        public string text { get; set; }
    }
}
