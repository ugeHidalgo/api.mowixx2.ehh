using API.Mowizz2.EHH.Configs;
using API.Mowizz2.EHH.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Mowizz2.EHH.Services
{
    public class TransactionsService
    {
        private readonly IMongoCollection<Transaction> _transactions;

        public TransactionsService(IDataBaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var dataBase = client.GetDatabase(settings.DatabaseName);

            _transactions = dataBase.GetCollection<Transaction>(settings.TransactionsCollectionName);
        }

        public HealthStatusData Check()
        {
            return new HealthStatusData
            {
                Connected = _transactions != null,
                CollectionName = _transactions.CollectionNamespace.CollectionName
            };
        }

        public async Task<List<Transaction>> ImportTransactions(List<Transaction> transactionsToImport)
        {
            await _transactions.InsertManyAsync(transactionsToImport);
            return transactionsToImport;
        }
    }
}
