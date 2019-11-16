using API.Mowizz2.EHH.Configs;
using API.Mowizz2.EHH.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace API.Mowizz2.EHH.Facades
{
    public class BankAccountFacade
    {
        private readonly IMongoCollection<BankAccount> _bankAccounts;

        public BankAccountFacade(IDataBaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var dataBase = client.GetDatabase(settings.DatabaseName);

            _bankAccounts = dataBase.GetCollection<BankAccount>(settings.BankAccountsCollectionName);
        }

        public List<BankAccount> Get()
        {
            return _bankAccounts.Find(bankAccount => true).ToList();
        }            
    }
}
