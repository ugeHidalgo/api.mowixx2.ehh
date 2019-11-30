using System;

namespace API.Mowizz2.EHH
{
    public class HealthData
    {
        public DateTime CheckDate { get; set; }

        public string ConnectionString { get; set; }

        public string DataBaseName { get; set; }

        public BankAccountsHealthStatus BankAccountsHealthStatus { get; set; }        
    }

    public class BankAccountsHealthStatus
    {
        public bool Connected { get; set; }
        public string CollectionName { get; set; }
    }
}
