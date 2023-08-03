using MongoDB.Driver;
using Super_Camp_Bot;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

internal class Program
{

    static MongoClient client = new MongoClient("mongodb+srv://vladdanilchuk9:GcrmHxJ0HPjn25MQ@cluster0.ioxuzpc.mongodb.net/?retryWrites=true&w=majority");
    static long userId = 0;
    static public TgUser user = new TgUser();
    private static async Task Main(string[] args)
    {
        
        BotCommandScopeChat botCommandScopeChat = new BotCommandScopeChat();

        botCommandScopeChat.ChatId = 663881584;

        BotCommandScopeAllChatAdministrators botCommandScopeAllChatAdmin = new BotCommandScopeAllChatAdministrators();


        //BotCommand setmorningmessage = new BotCommand() { Command = "/setmorningmessage", Description = "Створити повідомлення" };
        //BotCommand getusers = new BotCommand() { Command = "/getusers", Description = "Отримати всіх користувачів 🙀" };




        List<BotCommand> commands = new List<BotCommand>()
        {
             new BotCommand() { Command = "/setmorningmessage", Description = "Створити повідомлення" },
             new BotCommand() { Command = "/getusers", Description = "Отримати всіх користувачів 🙀" },
             new BotCommand() { Command = "/test", Description = "тест" }


        };

        List<BotCommand> commandsForGroup = new List<BotCommand>()
        {
             new BotCommand() { Command = "/sendmorningmessage", Description = "Добрий ранок ⛅☀️"  }
             


        };
        BotCommandScope botCommandScopeDefault = new BotCommandScopeDefault();
         await TgAPI.botClient.SetMyCommandsAsync(commands, botCommandScopeChat, "uk");

         await TgAPI.botClient.SetMyCommandsAsync(commandsForGroup, botCommandScopeAllChatAdmin, "uk");

        //await TgAPI.botClient.DeleteMyCommandsAsync( botCommandScopeAllChatAdmin, "uk");
        //await TgAPI.botClient.DeleteMyCommandsAsync(botCommandScopeChat, "uk");  





        //var db = client.GetDatabase("Cluster0");
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
        };



        TgAPI.botClient.StartReceiving(
             updateHandler: HandleUpdateAsync,
             pollingErrorHandler: HandlePollingErrorAsync,
             receiverOptions: receiverOptions);


        var me = await TgAPI.botClient.GetMeAsync();

        Console.WriteLine($"Start listening for @{me.Username}");
        Console.ReadLine();
    }

    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
       // update.Message = null; 

        if (update.Message is not { } message)
            return;


        var chatId = message.Chat.Id;

        //новий користувач
        if (!(message.NewChatMembers is not { } members) &&  ! (message.NewChatMembers[0].Username == TgAPI.ChatBotName))
        {
            
                List<TgUser> TgUsersADD = new List<TgUser>();

                foreach (var User in members)
                {
                    string username = User.Username ?? ""; // Використовуємо тернарний оператор для перевірки на null

                    TgUsersADD.Add(new TgUser(username, User.Id) { });

                    //Data.tgUsers.Add(new TgUser {UserName = User.Username, UserId = User.Id }); 
                }
                await MongoData.AddUsers(TgUsersADD);
                await botClient.DeleteMessageAsync(chatId, message.MessageId);
            
            
            return;
        }




        //// Я тебе не знаю
        //if (!Data.tgUsers.Any(user => user.UserId == message.From.Id))
        //{
        //    await botClient.SendTextMessageAsync(message.Chat.Id, "Я тебе не знаю");
        //    return; 
        //}





        if (message.Chat.Type.ToString() == "Private" && message.From != null)
        {

            if (message.Text == "/start")
            {
                List<TgUser> TgUsersADD = new List<TgUser>();
                string username = message.From.Username ?? "";
                TgUsersADD.Add(new TgUser(username, message.From.Id, message.From.Id) { });
                await MongoData.AddUsers(TgUsersADD);

            }


            // Я тебе не знаю
            if (!MongoData.tgAdmins.Any(user => user.UserId == message.From.Id))
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Я тебе не знаю");
                return;
            }

            if (user.UserId == 0 || (user.UserId != message.From.Id))
            {
                user = await MongoData.GetUser(message.From.Id);
            }
            if (message.Text is not { } messageText)
                return;


            switch (user.State)
            {
                case "Menu":
                    switch (messageText)
                    {
                        case "/setmorningmessage":
                            CommandsActions.SetMorningMessage(user);
                            break;
                        case "/getusers":
                            Tools.PrintAllusers(message.Chat.Id);
                            break;
                        case "/sendmorningmessage":
                            break;


                        case "/test":
                            
                            break;
                        default:
                            break;
                    }


                    break;
                case "SendingMorningMessage":
                    CommandsActions.SetMorningMessage(user, messageText);

                    break;


            }


















            //var Text = "FirstName: " + message.From.FirstName + "\n" + "LastName: " + message.From.LastName + "\n" + "LastName: " + message.From.Id + "Username: " + "\n" + message.From.Username;
            //var parseMode = ParseMode.Html;

            ////string Text = 
            //await botClient.SendTextMessageAsync(chatId, Text, parseMode: parseMode);
            //await botClient.SendTextMessageAsync(chatId, Text);


        }
        else if ((message.Chat.Type.ToString() == "Group" || message.Chat.Type.ToString() == "Supergroup") && message.From != null)
        {
            if (message.Text is not { } messageText)
                return;

            string sendmessage = "/sendmorningmessage@" + TgAPI.ChatBotName;


            switch (messageText)
            {
                case "Дай список":
                    string Text = "";
                    foreach (var user in Data.tgUsers)
                    {
                        Text += "@" + user.UserName + " ";
                    }
                    await botClient.SendTextMessageAsync(chatId, Text);
                    break;
                case "/sendmorningmessage@Super_Camp_Bot":
                    CommandsActions.SendMorningMessage(message); 

                    break; 
            }
        }
        return;




        //switch (messageText)
        //{
        //    case "/setdate@this_bot":
        //        await SendMessageWithInlinekeyboard(chatId, Markaps.CalendarCreating(DateTime.Now), "Оберіть дати, коли потрібно зробити нагадування");
        //        break;


        //    default:
        //        break;
        //}
    }

    static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }


}

