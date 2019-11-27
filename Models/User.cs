using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace API.Mowizz2.EHH.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userName")]
        public string UserName { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("firstName")]
        public string FirstName { get; set; }

        [BsonElement("lastName")]
        public string LastName { get; set; }        

        [BsonElement("active")]
        public bool Active { get; set; }

        [BsonElement("created")]
        public DateTime Created { get; set; }

        [BsonElement("updated")]
        public DateTime Updated { get; set; }

        [BsonElement("eMail")]
        public string EMail { get; set; }
    }
}
