using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace API.Mowizz2.EHH.Models
{
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("amount")]
        public double Amount { get; set; }

        [BsonElement("accountAmount")]
        public double AccountAmount { get; set; }

        [BsonElement("transactionType")]
        public int TransactionType { get; set; }

        [BsonElement("concept")]
        public Concept Concept { get; set; }

        [BsonElement("costCentre")]
        public CostCentre CostCentre { get; set; }

        [BsonElement("account")]
        public Account Account { get; set; }

        [BsonElement("comments")]
        public String Comments { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("company")]
        public string Company { get; set; }
    }
}
