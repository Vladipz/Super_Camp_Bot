using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace Super_Camp_Bot
{
    static internal class Tools
    {
        static public async void PrintAllusers(long chatId)
        {
            List<TgUser> tgUsers = await MongoData.GetAllUserAsync();
            var parseMode = ParseMode.Html;
            string Text = ""; 
            for (int i = 0; i < tgUsers.Count; i++)
            {
                Text += "<b>Нікнейм:</b> " + tgUsers[i].UserName + "\n";
                Text += "<b>Id:</b> " + tgUsers[i].UserId + "\n";
                Text += "<b>Роль:</b> " + tgUsers[i].Role + "\n";
                Text += "\n"; 
            }
            TgAPI.SendMessage(chatId, Text, parseMode); 
       


        }
    }
}
