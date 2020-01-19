using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Mowizz2.EHH.Models
{
    public class ImportTransaction
    {        
        [BsonElement("amount")]
        public double Amount { get; set; }

        [BsonElement("accountAmount")]
        public double AccountAmount { get; set; }

        [BsonElement("transactionType")]
        public int TransactionType { get; set; }

        [BsonElement("conceptName")]
        public string ConceptName { get; set; }

        [BsonElement("costCentreName")]
        public string CostCentreName { get; set; }

        [BsonElement("accountName")]
        public string AccountName { get; set; }

        [BsonElement("comments")]
        public String Comments { get; set; }

        [BsonElement("date")]
        public string Date { get; set; }

        [BsonElement("company")]
        public string Company { get; set; }
    }
}
