using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Super_Camp_Bot
{
    internal class MorningMessage
    {
        [BsonId]
        private ObjectId _id
        {
            get; set;
        }

        public string? Message
        {
            get; set;
        }
        public bool ThatMessage
        {
            get; set;


        }


        public MorningMessage(string? message)
        {
            Message = message;
            ThatMessage = false; 
        }
        public MorningMessage()
        {
            Message = "";
            ThatMessage = false;
        }
    }
}
