using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Mowizz2.EHH.Models
{
    public class BankAccount
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public string Iban { get; set; }

        public string Comments { get; set; }
        
        public BsonDateTime Created { get; set; }

        public BsonDateTime Updated { get; set; }

        public string UserName { get; set; }
    }
}
