using API.Mowizz2.EHH.Configs;
using API.Mowizz2.EHH.Models;
using MongoDB.Driver;

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
    }
}
