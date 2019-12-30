using System;

namespace API.Mowizz2.EHH
{
    public class HealthData
    {
        public DateTime CheckDate { get; set; }

        public string ServerName { get; set; }

        public string DataBaseName { get; set; }

        public string DataBaseUserName { get; set; }        

        public HealthStatusData BankAccountsHealthStatus { get; set; }
        public HealthStatusData CompaniesHealthStatus { get; set; }
        public HealthStatusData ConceptsHealthStatus { get; set; }
        public HealthStatusData CostCentresHealthStatus { get; set; }        
        public HealthStatusData UsersHealthStatus { get; set; }
        
    }

    public class HealthStatusData
    {
        public bool Connected { get; set; }
        public string CollectionName { get; set; }
    }
}
