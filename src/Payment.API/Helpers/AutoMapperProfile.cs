using AutoMapper;
using Microsoft.Extensions.Logging;
using Payment.API.DTO;
using Payment.API.Models;

namespace Payment.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AccountDTO, Account>();
            CreateMap<AccountTransactionDTO, AccountTransaction>();
            CreateMap<Account, AccountResponseDTO>();
        }
    }
}
