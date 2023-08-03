using Amazon.SecurityToken.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Super_Camp_Bot
{
    public class TgUser
    {
        [BsonId]
        private ObjectId _id
        {
            get; set;
        }
        [BsonElement("username")]
        public string UserName
        {
        get; set; }


        public long UserId
        {
        get; set; }
        [BsonElement("role")]

        public string Role
        {
        
        get; set; }
        [BsonElement("chatId")]

        public long ChatId
        {

            get; set;
        }

        public string State
        {
            get; set;
        }


        public TgUser(string userName, long userId, long chatid, string role, string state) 
        {
            UserName = userName;
            UserId = userId;
            Role = role;
            State = state;
            ChatId = chatid;
        }
        
        public TgUser(string userName, long userId,long chatid) : this(userName, userId, chatid, "mainuser", "Невідомо")
        {
            UserName = userName;
            UserId = userId;
            ChatId = chatid; 
        }
        public TgUser(string userName, long userId) : this(userName, userId, 0, "mainuser", "Невідомо")
        {
            UserName = userName;
            UserId = userId;
        }

        public TgUser(string userName) 
        {
            UserName = userName;

        }

        public TgUser() : this("Unknown")
        {
            UserName = string.Empty;

        }


        public async void UpdateStatus(string status)
        {
            State = status; 
            await MongoData.UpdetaState(this, status);
        }
    }
}
