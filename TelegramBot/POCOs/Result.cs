using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.POCOs
{
    class Result
    {
        public int update_id { get; set; }
        public Message message { get; set; }
        public CallbackQuery callback_query { get; set; }
    }
}
