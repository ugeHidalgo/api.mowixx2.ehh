using API.Mowizz2.EHH.Configs;
using API.Mowizz2.EHH.Models;
using AutoMapper;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Mowizz2.EHH.Services
{
    public class TransactionsService
    {
        private readonly IMongoCollection<Transaction> _transactions;        
        private readonly IMapper _mapper;
        private readonly IDataBaseSettings _settings;

        public TransactionsService(IDataBaseSettings settings, IMapper mapper)
        {
            _mapper = mapper;
            _settings = settings;
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

        public async Task<List<Transaction>> GetAllForCompanyAsync(string company)
        {
            return await _transactions.FindAsync(transaction => transaction.Company == company).Result.ToListAsync();
        }

        public async Task<List<Transaction>> ImportTransactions(List<ImportTransaction> transactionsToImport)
        {
            List<Transaction> transactions = new List<Transaction>();
            Transaction transaction;
            foreach (var transactionToImport in transactionsToImport)
            {
                transaction = _mapper.Map<ImportTransaction, Transaction>(transactionToImport);
                transaction.Concept = GetConcept(transactionToImport.ConceptName);
                transaction.BankAccount = GetBankAccount(transactionToImport.BankAccountName);
                transaction.CostCentre = GetCostCentre(transactionToImport.CostCentreName);
                transactions.Add(transaction);
            }
            if (transactions.Count > 0)
            {
                await _transactions.InsertManyAsync(transactions);
            }
            return transactions;
        }

        #region Private Methods

        private BankAccount GetBankAccount(string bankAccountName)
        {
            BankAccountsService _bankAccountService = new BankAccountsService(_settings);
            var bankAccount = _bankAccountService.GetByName(bankAccountName);
            return bankAccount;
        }

        private CostCentre GetCostCentre(string costCentreName)
        {
            CostCentresService _costCentreService = new CostCentresService(_settings);
            var costCentre = _costCentreService.GetByName(costCentreName);
            return costCentre;
        }

        private Concept GetConcept(string conceptName)
        {
            ConceptsService _conceptsService = new ConceptsService(_settings);
            var concept = _conceptsService.GetByName(conceptName);
            return concept;
        }

        #endregion
    }
}
