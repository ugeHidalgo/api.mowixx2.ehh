using API.Mowizz2.EHH.Configs;
using API.Mowizz2.EHH.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Mowizz2.EHH.Services
{
    public class CompaniesService
    {
        private readonly IMongoCollection<Company> _mongoCollection;

        public CompaniesService(IDataBaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var dataBase = client.GetDatabase(settings.DatabaseName);

            _mongoCollection = dataBase.GetCollection<Company>(settings.CompaniesCollectionName);
        }

        public HealthStatusData Check()
        {
            return new HealthStatusData
            {
                Connected = _mongoCollection != null,
                CollectionName = _mongoCollection.CollectionNamespace.CollectionName
            };
        }

        public async Task<List<Company>> Get()
        {
            return await _mongoCollection.FindAsync(company => true).Result.ToListAsync();
        }

        public async Task<Company> Get(string id)
        {
            var company = await _mongoCollection.FindAsync(x => x.Id == id).Result.FirstOrDefaultAsync();
            return company;
        }

        public async Task<List<Company>> CreateCompanies(List<Company> companies)
        {
            await _mongoCollection.InsertManyAsync(companies);
            return companies;
        }
    }
}
