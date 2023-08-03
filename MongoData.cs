using MongoDB.Bson;
using MongoDB.Driver;
using System;


namespace Super_Camp_Bot
{
    static internal class MongoData
    {
        static private MongoClient client = new MongoClient("mongodb+srv://vladdanilchuk9:GcrmHxJ0HPjn25MQ@cluster0.ioxuzpc.mongodb.net/?retryWrites=true&w=majority");
        static public IMongoDatabase DB = client.GetDatabase("Cluster0");
        static  string DbWithTgUsersName = "TgUsers";
        static public string DbWithMorningMessages = "MorningMessages";

        static public List<TgUser> tgAdmins = DB.GetCollection<TgUser>(DbWithTgUsersName).Find(new BsonDocument("role", "admin")
).ToList(); // get colection with users 

        public static async Task AddUsers(List<TgUser> tgUsersInsert)
        {
            var usersCollection = DB.GetCollection<TgUser>(DbWithTgUsersName); // get colection with users 

            var tgUserscursor = await usersCollection.FindAsync(new BsonDocument());
            List<TgUser> tgUsers = tgUserscursor.ToList();
            List<TgUser> tgUsersExist = new List<TgUser>();

            foreach (var user in tgUsersInsert)
            {
                if (tgUsers.Exists(u => u.UserId == user.UserId || u.UserName == user.UserName))
                    tgUsersExist.Add(user);
            }
            tgUsersInsert.RemoveRange(0, tgUsersExist.Count);

            //foreach (var user in tgUsersExist)
            //    tgUsersInsert.Remove(user);
            if (!(tgUsersInsert.Count <= 0)) // checking that the list is empty
                await usersCollection.InsertManyAsync(tgUsersInsert);




        }
        public static async Task<TgUser> GetUser(long id)
        {
            TgUser user = new TgUser();
            var usersCollection = DB.GetCollection<TgUser>(DbWithTgUsersName); // get colection with users 
            //var usercursor = usersCollection.FindAsync(new BsonDocument("UserId", id));
            user = await usersCollection.Find(u => u.UserId == id).FirstOrDefaultAsync();

            return user;
        }

        public static async Task UpdetaState(TgUser user, string state)
        {
            var usersCollection = DB.GetCollection<TgUser>(DbWithTgUsersName);

            var filter = new BsonDocument("UserId", user.UserId);
            var updateSettings = new BsonDocument("$set", new BsonDocument("State", state));

            try
            {
                var result = await usersCollection.UpdateOneAsync(filter, updateSettings);
                Console.WriteLine($"Matched: {result.MatchedCount}; Modified: {result.ModifiedCount}");

            }
            catch (Exception)
            {
                Console.WriteLine("error when updating user status");
                throw;
            }





        }

        public static async Task AddMorningMessage(string message)
        {
            MorningMessage morningMessage = new MorningMessage(message);
            var MessagesCollection = DB.GetCollection<MorningMessage>(DbWithMorningMessages);
            await MessagesCollection.InsertOneAsync(morningMessage);
            Console.WriteLine("Add morning message : " + message + " to " + MessagesCollection); 
        }

        public static async Task<List<TgUser>> GetAllUserAsync()
        {
            var usersCollection = DB.GetCollection<TgUser>(DbWithTgUsersName); // get colection with users 

            var tgUserscursor = await usersCollection.FindAsync(new BsonDocument());
            List<TgUser> tgUsers = tgUserscursor.ToList();
            return tgUsers;
        }
        public static async Task<List<MorningMessage>> GetMorningMessages()
        {
            List<MorningMessage> morningMessages = new List<MorningMessage>();

            var morningmessagesCollection = DB.GetCollection<MorningMessage>(DbWithMorningMessages);
            long count = await morningmessagesCollection.CountDocumentsAsync(new BsonDocument());

            if (count == 0)
            {
                Console.WriteLine("Коллекция повідомлень пуста.");
               
            }
            else
            {
                Console.WriteLine($"В коллекции {count} документов.");
                using var cursor = await morningmessagesCollection.FindAsync(new BsonDocument());
                // из курсора получаем список данных
                morningMessages = cursor.ToList();
            }
            


            return morningMessages;
        }

    }
}
