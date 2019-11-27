﻿namespace API.Mowizz2.EHH.Configs
{
    public class DataBaseSettings : IDataBaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

        public string BankAccountsCollectionName { get; set; }
        public string UsersCollectionName { get; set; }

    }
}
