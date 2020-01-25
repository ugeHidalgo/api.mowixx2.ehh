using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace API.Mowizz2.EHH.Models
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("active")]
        public Boolean Active { get; set; }

        [BsonElement("iban")]
        public string Iban { get; set; }

        [BsonElement("amount")]
        public double Amount { get; set; }

        [BsonElement("comments")]
        public string Comments { get; set; }

        [BsonElement("created")]
        public DateTime Created { get; set; }

        [BsonElement("updated")]
        public DateTime Updated { get; set; }

        [BsonElement("userName")]
        public string UserName { get; set; }

        [BsonElement("company")]
        public string Company { get; set; }
    }
}
