using API.Mowizz2.EHH.Configs;
using API.Mowizz2.EHH.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Mowizz2.EHH.Facades
{
    public class BankAccountsFacade
    {
        private readonly IMongoCollection<BankAccount> _bankAccounts;

        public BankAccountsFacade(IDataBaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var dataBase = client.GetDatabase(settings.DatabaseName);

            _bankAccounts = dataBase.GetCollection<BankAccount>(settings.BankAccountsCollectionName);
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
    }
}
