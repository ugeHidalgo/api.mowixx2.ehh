namespace API.Mowizz2.EHH.Configs
{
    public interface IDataBaseSettings
    {
        string BankAccountsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
