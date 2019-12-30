namespace API.Mowizz2.EHH.Configs
{
    public interface IDataBaseSettings
    {
        string BankAccountsCollectionName { get; set; }
        string CompaniesCollectionName { get; set; }
        string ConceptsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string CostCentresCollectionName { get; set; }
        string DatabaseName { get; set; }
        string UsersCollectionName { get; set; }
    }
}
