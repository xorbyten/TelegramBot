using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using TelegramBot.Classes;
using TelegramBot.Models;

namespace TelegramBot
{
    class Program
    {
        static BotClass bot;
        static void Main(string[] args)
        {
            // Dependency Injection
            //bot = new BotClass(new DBHandler());
            bot = new BotClass(new EFCoreDBHandler());

            Console.WriteLine(">> Bot started...");
            bot.StartBot();
            Console.WriteLine(">> Press any key to stop bot...");
            Console.ReadKey();
        }
    }
}
