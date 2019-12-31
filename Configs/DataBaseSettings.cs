namespace API.Mowizz2.EHH.Configs
{
    public class DataBaseSettings : IDataBaseSettings
    {
        public string BankAccountsCollectionName { get; set; }
        public string CompaniesCollectionName { get; set; }
        public string ConceptsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string CostCentresCollectionName { get; set; }
        public string TransactionsCollectionName { get; set; }
        public string DatabaseName { get; set; }
        public string UsersCollectionName { get; set; }
    }
}
