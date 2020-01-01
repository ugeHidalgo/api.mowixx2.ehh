using API.Mowizz2.EHH.Configs;
using API.Mowizz2.EHH.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Mowizz2.EHH.Services
{
    public class CostCentresService
    {
        private readonly IMongoCollection<CostCentre> _mongoCollection;

        public CostCentresService(IDataBaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var dataBase = client.GetDatabase(settings.DatabaseName);

            _mongoCollection = dataBase.GetCollection<CostCentre>(settings.CostCentresCollectionName);
        }

        public HealthStatusData Check()
        {
            return new HealthStatusData
            {
                Connected = _mongoCollection != null,
                CollectionName = _mongoCollection.CollectionNamespace.CollectionName
            };
        }

        public async Task<List<CostCentre>> GetAllAsync()
        {
            return await _mongoCollection.FindAsync(CostCentre => true).Result.ToListAsync();
        }

        public async Task<List<CostCentre>> GetAllForCompanyAsync(string company)
        {
            return await _mongoCollection.FindAsync(CostCentre => CostCentre.Company == company).Result.ToListAsync();
        }

        public async Task<CostCentre> GetByIdAsync(string id)
        {
            var CostCentre = await _mongoCollection.FindAsync(x => x.Id == id).Result.FirstOrDefaultAsync();
            return CostCentre;
        }

        public async Task<CostCentre> GetByNameAsync(string name)
        {
            var CostCentre = await _mongoCollection.FindAsync(x => x.Name == name).Result.FirstOrDefaultAsync();
            return CostCentre;
        }

        public CostCentre GetByName(string name)
        {
            var CostCentre = _mongoCollection.Find(x => x.Name == name).FirstOrDefault();
            return CostCentre;
        }

        public async Task<List<CostCentre>> CreateCostCentresAsync(List<CostCentre> CostCentres)
        {
            await _mongoCollection.InsertManyAsync(CostCentres);
            return CostCentres;
        }
    }
}
