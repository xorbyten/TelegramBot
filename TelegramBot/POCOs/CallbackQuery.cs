using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.POCOs
{
    class CallbackQuery
    {
        public string id { get; set; }
        public CallbackFrom from { get; set; }
        public CallbackMessage message { get; set; }
        public string chat_instance { get; set; }
        public string data { get; set; }
    }
}
