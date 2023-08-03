using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Super_Camp_Bot
{
    static internal class CommandsActions
    {
        static public void SetMorningMessage(TgUser user)
        {
            TgAPI.botClient.SendTextMessageAsync(user.ChatId, "Відправте повідомленя, яке буде додано до ранкового привітання");
            user.UpdateStatus("SendingMorningMessage");


            return; 

            //string morningmessage = ;
            //TgAPI.botClient.


        }




        static public async void SetMorningMessage(TgUser user, string  message)
        {
            //дщгіка додавання повідомлень до бд
            await MongoData.AddMorningMessage(message); 
            TgAPI.SendMessage(user.ChatId, message+ " - додана до можливих варінтів");
            user.UpdateStatus("Menu");
            
            return;
        }

        static public  void SendMorningMessage(long chatId, MorningMessage message)
        {
            if (message.Message != null)
            {
                TgAPI.SendMessage(chatId, message.Message);

            }
            
        
        }


        static public async void SendMorningMessage(Message message)
        {
           await  TgAPI.botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId);
            List<MorningMessage> morningMessages = await MongoData.GetMorningMessages();
            if (!(morningMessages.Count == 0))
            {
                MorningMessage morningMessage = new MorningMessage();
                bool gettruemessage = false;
                foreach (var messageinit in morningMessages)
                {
                    if (messageinit.ThatMessage && messageinit.Message!=null)
                    {
                        
                        morningMessage = messageinit;
                        gettruemessage = true;
                        break;
                    }
                }
                if (gettruemessage)
                {
                    SendMorningMessage(message.Chat.Id, morningMessage);
                }
                else
                {
                    Random random = new Random();
                    int randomIndex = random.Next(morningMessages.Count);
                    morningMessage = morningMessages[randomIndex];
                    
                    SendMorningMessage(message.Chat.Id, morningMessage);

                }

            }
            

            

        }
    }
}
