using API.Mowizz2.EHH.Configs;
using API.Mowizz2.EHH.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Mowizz2.EHH.Services
{
    public class ConceptsService
    {
        private readonly IMongoCollection<Concept> _mongoCollection;

        public ConceptsService(IDataBaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var dataBase = client.GetDatabase(settings.DatabaseName);

            _mongoCollection = dataBase.GetCollection<Concept>(settings.ConceptsCollectionName);
        }

        public HealthStatusData Check()
        {
            return new HealthStatusData
            {
                Connected = _mongoCollection != null,
                CollectionName = _mongoCollection.CollectionNamespace.CollectionName
            };
        }

        public async Task<List<Concept>> GetAll()
        {
            return await _mongoCollection.FindAsync(concept => true).Result.ToListAsync();
        }

        public async Task<List<Concept>> GetAllForCompany(string company)
        {
            return await _mongoCollection.FindAsync(concept => concept.Company == company).Result.ToListAsync();
        }

        public async Task<Concept> GetById(string id)
        {
            var concept = await _mongoCollection.FindAsync(x => x.Id == id).Result.FirstOrDefaultAsync();
            return concept;
        }

        public async Task<List<Concept>> CreateConcepts(List<Concept> concepts)
        {
            await _mongoCollection.InsertManyAsync(concepts);
            return concepts;
        }
    }
}
