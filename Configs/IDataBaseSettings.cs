namespace API.Mowizz2.EHH.Configs
{
    public interface IDataBaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string BankAccountsCollectionName { get; set; }        
        string UsersCollectionName { get; set; }
        string CompaniesCollectionName { get; set; }
        string ConceptsCollectionName { get; set; }
    }
}
