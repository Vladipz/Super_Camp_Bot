using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Super_Camp_Bot
{
    static internal class TgAPI
    {
        static public ITelegramBotClient botClient = new TelegramBotClient("6415411499:AAF028u6rol4046JlbBa7aXjgr9WKGHt2Xs");
        static public string ChatBotName = "Super_Camp_Bot"; 
        public static async void SendMessage(long chatId,string message)
        {
           await botClient.SendTextMessageAsync(chatId, message);
         
        
        }

        public static async void SendMessage(long chatId, string message, ParseMode parseMode)
        {
            await botClient.SendTextMessageAsync(chatId, message, parseMode: parseMode);

        }
       
    }
}
