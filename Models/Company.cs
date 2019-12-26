using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Mowizz2.EHH.Models
{
    public class Company
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

        [BsonElement("created")]
        public DateTime Created { get; set; }

        [BsonElement("updated")]
        public DateTime Updated { get; set; }
    }
}
