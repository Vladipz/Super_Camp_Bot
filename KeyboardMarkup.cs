using Telegram.Bot.Types.ReplyMarkups;

namespace Super_Camp_Bot
{
    static internal class KeyboardMarkup
    {
        static public InlineKeyboardMarkup inlineKeyboard = new(new[]
        {
           new []
           {
                InlineKeyboardButton.WithCallbackData(text: "Я тут", callbackData: "IamHere"),  
           }

        });

    }
}
