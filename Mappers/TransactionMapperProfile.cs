using API.Mowizz2.EHH.Configs;
using API.Mowizz2.EHH.Models;
using AutoMapper;

namespace API.Mowizz2.EHH.Mappers
{
    public class TransactionMapperProfile: Profile
    {                
        public TransactionMapperProfile()
        {
            CreateMap<ImportTransaction, Transaction>()
                .ForMember(
                    dest => dest.BankAccount,
                    opt => opt.Ignore());

            CreateMap<ImportTransaction, Transaction>()
                .ForMember(
                    dest => dest.CostCentre, 
                    opt => opt.Ignore());

            CreateMap<ImportTransaction, Transaction>()
                .ForMember(
                    dest => dest.Concept,
                    opt => opt.Ignore());
        }        
    }
}
