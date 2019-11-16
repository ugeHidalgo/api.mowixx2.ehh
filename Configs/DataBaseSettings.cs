namespace API.Mowizz2.EHH.Configs
{
    public class DataBaseSettings : IDataBaseSettings
    {
        public string BankAccountsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
