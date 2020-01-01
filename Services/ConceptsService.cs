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

        public async Task<List<Concept>> GetAllAsync()
        {
            return await _mongoCollection.FindAsync(concept => true).Result.ToListAsync();
        }

        public async Task<List<Concept>> GetAllForCompanyAsync(string company)
        {
            return await _mongoCollection.FindAsync(concept => concept.Company == company).Result.ToListAsync();
        }

        public async Task<Concept> GetByIdAsync(string id)
        {
            var concept = await _mongoCollection.FindAsync(x => x.Id == id).Result.FirstOrDefaultAsync();
            return concept;
        }

        public async Task<Concept> GetByNameAsync(string name)
        {
            var concept = await _mongoCollection.FindAsync(x => x.Name == name).Result.FirstOrDefaultAsync();
            return concept;
        }

        public Concept GetByName(string name)
        {
            var concept = _mongoCollection.Find<Concept>(x => x.Name == name).FirstOrDefault();
            return concept;
        }

        public async Task<List<Concept>> CreateConceptsAsync(List<Concept> concepts)
        {
            await _mongoCollection.InsertManyAsync(concepts);
            return concepts;
        }
    }
}
