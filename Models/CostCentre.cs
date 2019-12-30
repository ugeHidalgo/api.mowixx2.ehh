using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace API.Mowizz2.EHH.Models
{
    public class CostCentre
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

        [BsonElement("comments")]
        public string Comments { get; set; }

        [BsonElement("created")]
        public DateTime Created { get; set; }

        [BsonElement("updated")]
        public DateTime Updated { get; set; }        

        [BsonElement("company")]
        public string Company { get; set; }
    }
}
