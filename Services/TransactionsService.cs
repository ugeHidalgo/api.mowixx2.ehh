using API.Mowizz2.EHH.Configs;
using API.Mowizz2.EHH.Models;
using AutoMapper;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace API.Mowizz2.EHH.Services
{
    public class TransactionsService
    {
        private readonly IMongoCollection<Transaction> _transactions;        
        private readonly IMapper _mapper;
        private readonly IDataBaseSettings _settings;
        private MongoClient _client;
        private IMongoDatabase _dataBase;

        public TransactionsService(IDataBaseSettings settings, IMapper mapper)
        {
            _mapper = mapper;
            _settings = settings;
            _client = new MongoClient(settings.ConnectionString);
            _dataBase = _client.GetDatabase(settings.DatabaseName);

            _transactions = _dataBase.GetCollection<Transaction>(settings.TransactionsCollectionName);            
        }

        public HealthStatusData Check()
        {
            return new HealthStatusData
            {
                Connected = _transactions != null,
                CollectionName = _transactions.CollectionNamespace.CollectionName
            };
        }

        public async Task<Transaction> GetById(string company, string id)
        {
            var transactions = await _transactions.FindAsync(transaction => transaction.Company == company && transaction.Id == id);
            return transactions.FirstOrDefault();
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
                transaction.Account = GetBankAccount(transactionToImport.AccountName);
                transaction.CostCentre = GetCostCentre(transactionToImport.CostCentreName);
                transactions.Add(transaction);
            }
            if (transactions.Count > 0)
            {
                await _transactions.InsertManyAsync(transactions);
            }
            return transactions;
        }

        public async Task<Transaction> CreateOrUpdateTransaction(Transaction transaction)
        {                        
            var accounts = _dataBase.GetCollection<Account>(_settings.AccountsCollectionName);

            if (transaction.Id == null)
            {
                return await CreateNewTransaction(transaction, accounts);
            }
            return await UpdateTransaction(transaction, accounts);
        }

        public async Task<bool> DeleteById(string company, string id)
        {
            var accounts = _dataBase.GetCollection<Account>(_settings.AccountsCollectionName);
            return await DeleteTransaction(company, id, accounts);
        }

        #region Private Methods

        private Account GetBankAccount(string bankAccountName)
        {
            AccountsService _bankAccountService = new AccountsService(_settings);
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

        private async Task<Transaction> CreateNewTransaction(Transaction transaction, IMongoCollection<Account> accounts)
        {
            using (var session = await _client.StartSessionAsync())
            {
                //session.StartTransaction(); //No disponible en mi version de mongoDb
                try
                {
                    await _transactions.InsertOneAsync(transaction);

                    var result = await accounts.FindOneAndUpdateAsync(
                            Builders<Account>.Filter.Eq("Id", transaction.Account.Id),
                            Builders<Account>.Update.Inc("Amount", transaction.Amount)
                        );

                    //await session.CommitTransactionAsync(); //No disponible en mi version de mongoDb

                    return transaction;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error creating new transaction: " + e.Message);
                    //await session.AbortTransactionAsync(); //No disponible en mi version de mongoDb
                    return null;
                }
            }
        }

        private async Task<Transaction> UpdateTransaction(Transaction transaction, IMongoCollection<Account> accounts)
        {
            using (var session = await _client.StartSessionAsync())
            {
                //session.StartTransaction(); //No disponible en mi version de mongoDb
                try
                {
                    //Find the old transaction to take old amount
                    Expression<Func<Transaction, bool>> transactionFilter = t => t.Company == transaction.Company && t.Id == transaction.Id;
                    var oldTransaction = await _transactions.FindAsync<Transaction>(transactionFilter);
                    double oldAmount = oldTransaction.FirstOrDefault().Amount;

                    //Find and update the transaction
                    var transactionUpdate = Builders<Transaction>.Update
                        .Set(t => t.AccountAmount, transaction.AccountAmount)
                        .Set(t => t.Amount, transaction.Amount)
                        .Set(t => t.TransactionType, transaction.TransactionType)
                        .Set(t => t.Concept, transaction.Concept)
                        .Set(t => t.CostCentre, transaction.CostCentre)
                        .Set(t => t.Account, transaction.Account)
                        .Set(t => t.Comments, transaction.Comments)
                        .Set(t => t.Date, transaction.Date)
                        .Set(t => t.Company, transaction.Company);

                    var transactionOptions = new FindOneAndUpdateOptions<Transaction> { ReturnDocument = ReturnDocument.After };
                    var updatedTransaction = await _transactions.FindOneAndUpdateAsync(transactionFilter, transactionUpdate, transactionOptions);

                    //Updates the account amount using the old and new amount value.
                    Expression<Func<Account, bool>> accountFilter = account => account.Company == updatedTransaction.Company && account.Id == updatedTransaction.Account.Id;
                    var accountUpdate = Builders<Account>.Update.Inc("Amount", updatedTransaction.Amount - oldAmount);
                    await accounts.FindOneAndUpdateAsync(accountFilter, accountUpdate);

                    //await session.CommitTransactionAsync(); //No disponible en mi version de mongoDb

                    return updatedTransaction;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error updating transaction: " + e.Message);
                    //await session.AbortTransactionAsync(); //No disponible en mi version de mongoDb
                    return null;
                }
            }
        }

        private async Task<bool> DeleteTransaction(string company, string id, IMongoCollection<Account> accounts)
        {
            using (var session = await _client.StartSessionAsync())
            {
                //session.StartTransaction(); //No disponible en mi version de mongoDb
                try
                {
                    Expression<Func<Transaction, bool>> transactionFilter = transaction => transaction.Company == company && transaction.Id == id;

                    //Find the old transaction to take old amount                    
                    var oldTransactions = await _transactions.FindAsync(transactionFilter);
                    var oldTransaction = oldTransactions.FirstOrDefault();
                    if (oldTransaction == null) return false;
                    double oldAmount = oldTransaction.Amount;

                    //Find and delete the transaction                    
                    await _transactions.DeleteOneAsync(transactionFilter);

                    //Updates the account amount using the old amount value.                    
                    Expression<Func<Account, bool>> accountFilter = account => account.Company == oldTransaction.Company && account.Id == oldTransaction.Account.Id;
                    var accountUpdate = Builders<Account>.Update.Inc("Amount", oldAmount*-1);
                    await accounts.FindOneAndUpdateAsync(accountFilter, accountUpdate);

                    //await session.CommitTransactionAsync(); //No disponible en mi version de mongoDb

                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error deleting transaction: " + e.Message);
                    //await session.AbortTransactionAsync(); //No disponible en mi version de mongoDb
                    return false;
                }
            }
        }

        #endregion
    }
}
