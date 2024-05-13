using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Payment.API.DTO;
using Payment.API.Helpers;
using Payment.API.Models;
using Payment.API.ServiceInterfaces;
using Payment.API.Validators;
using System;

namespace Payment.API.Controllers
{
    /// <summary>
    /// Account Transaction Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IServiceRepository<Account> _serviceRepository ;
        private readonly IMapper _mapper;
        /// <summary>
        /// Account Controller
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="serviceRepository"></param>
        public AccountController(IMapper mapper, IServiceRepository<Account> serviceRepository)
        {
            _serviceRepository = serviceRepository;
            _mapper = mapper;
            if (!_serviceRepository.GetAll().Any())
            {
                _serviceRepository.Insert(new Account
                { 
                    accountId = "1234567890", 
                    surname = "Master" ,
                    firstName="Ledger",
                    transactionPin= SecurePayment.ComputeSha256Hash(Convert.ToString("1469")),
                    accountBalance = 1000000
                } 
                    );
                _serviceRepository.Save();
            }
        }
        /// <summary>
        /// Returns all accounts
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAllAccounts")]
        public IEnumerable<AccountResponseDTO> GetAllAccounts()
        {
            var accounts = _serviceRepository.GetAll();
            return _mapper.Map<IEnumerable<AccountResponseDTO>>(accounts).OrderByDescending(x => x.createdDate); ;
        }

        /// <summary>
        /// Creates new Account 
        /// </summary>
        /// <param name="accountDTO"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /AddAccount
        ///     {
        ///        "surname": "Joromi",
        ///        "firstName": "Adekunle",
        ///        "transactionPin": 1004
        ///     }
        ///
        /// </remarks>
        [HttpPost(Name = "CreateAccount")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateAccount(AccountDTO accountDTO)
        {
            var validator = new AccountValidator();
            var validRes = validator.Validate(accountDTO);
            if (!validRes.IsValid)
            {
                return BadRequest(validRes.Errors.FirstOrDefault());
            }
            else
            {
                var accountdata = _mapper.Map<Account>(accountDTO);  
                accountdata.accountId = DateTime.Now.Ticks.ToString();
                accountdata.transactionPin = SecurePayment.ComputeSha256Hash(Convert.ToString(accountdata.transactionPin));
                _serviceRepository.Insert(accountdata);
                _serviceRepository.Save();
                return CreatedAtRoute("GetAllAccounts", new { id = accountdata.accountId }, _mapper.Map<AccountResponseDTO>(accountdata));
            }
                
           
        }
    }
}
