using API.Mowizz2.EHH.Configs;
using API.Mowizz2.EHH.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Mowizz2.EHH.Facades
{    
    public class BankAccountsService
    {
        private readonly IMongoCollection<BankAccount> _bankAccounts;

        public BankAccountsService(IDataBaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var dataBase = client.GetDatabase(settings.DatabaseName);

            _bankAccounts = dataBase.GetCollection<BankAccount>(settings.BankAccountsCollectionName);
        }

        public HealthStatusData Check()
        {
            return new HealthStatusData
            {
                Connected = _bankAccounts != null,
                CollectionName = _bankAccounts.CollectionNamespace.CollectionName
            };
        }

        public async Task<List<BankAccount>> Get()
        {
            return await _bankAccounts.FindAsync(bankAccount => true).Result.ToListAsync();
        }

        public async Task<BankAccount> Get(string id)
        {
            var bankAccount = await _bankAccounts.FindAsync(x => x.Id == id).Result.FirstOrDefaultAsync();
            return bankAccount;
        }

        public async Task<List<BankAccount>> CreateBankAccounts(List<BankAccount> bankAccounts)
        {
            await _bankAccounts.InsertManyAsync(bankAccounts);
            return bankAccounts;
        }
    }
}
