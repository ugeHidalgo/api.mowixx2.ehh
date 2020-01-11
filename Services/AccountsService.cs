using API.Mowizz2.EHH.Configs;
using API.Mowizz2.EHH.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Mowizz2.EHH.Services
{
    public class AccountsService
    {
        private readonly IMongoCollection<Account> _accounts;

        public AccountsService(IDataBaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var dataBase = client.GetDatabase(settings.DatabaseName);

            _accounts = dataBase.GetCollection<Account>(settings.AccountsCollectionName);
        }

        public HealthStatusData Check()
        {
            return new HealthStatusData
            {
                Connected = _accounts != null,
                CollectionName = _accounts.CollectionNamespace.CollectionName
            };
        }

        public async Task<List<Account>> GetAllAsync()
        {
            return await _accounts.FindAsync(account => true).Result.ToListAsync();
        }

        public async Task<List<Account>> GetAllForCompanyAsync(string company)
        {
            return await _accounts.FindAsync(account => account.Company == company).Result.ToListAsync();
        }

        public async Task<Account> GetByIdAsync(string id)
        {
            var account = await _accounts.FindAsync(x => x.Id == id).Result.FirstOrDefaultAsync();
            return account;
        }

        public async Task<Account> GetByNameAsync(string name)
        {
            var account = await _accounts.FindAsync(x => x.Name == name).Result.FirstOrDefaultAsync();
            return account;
        }

        public Account GetByName(string name)
        {
            var account = _accounts.Find(x => x.Name == name).FirstOrDefault();
            return account;
        }

        public async Task<List<Account>> CreateAccounts(List<Account> accounts)
        {
            await _accounts.InsertManyAsync(accounts);
            return accounts;
        }
    }
}
