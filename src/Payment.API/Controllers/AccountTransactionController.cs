using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.API.DTO;
using Payment.API.Helpers;
using Payment.API.Models;
using Payment.API.ServiceInterfaces;
using Payment.API.Validators;

namespace Payment.API.Controllers
{
    /// <summary>
    /// Account Transaction Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTransactionController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private IServiceRepository<AccountTransaction> _serviceRepository;
        private IAccountServices _accountService;
        private IAccountTransactionServices _accountTransactionService;
        private readonly IMapper _mapper;
        public AccountTransactionController(IMapper mapper, IServiceRepository<AccountTransaction> serviceRepository, IAccountServices accountService, IAccountTransactionServices accountTransactionService)
        {
            _serviceRepository = serviceRepository;
            _accountService = accountService;
            _accountTransactionService = accountTransactionService;
            _mapper = mapper;
        }
        /// <summary>
        /// Returns all Account Transaction
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAllAccountTransactions")]
        public IEnumerable<AccountTransaction> GetAccountTransactions()
        {
            return _serviceRepository.GetAll().OrderByDescending(x=> x.createdDate);
        }

        /// <summary>
        /// Credits a given account
        /// </summary>
        /// <param name="accountTransactionDTO"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /CreditAccount
        ///     {
        ///        "amount": "150000",
        ///        "debitAccountId": "1234567890",
        ///        "creditAccountId": "0987654321",
        ///        "transactionPin": 1768
        ///     }
        ///
        /// </remarks>
        [HttpPost(Name = "CreditAccount")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreditAccount(AccountTransactionDTO accountTransactionDTO)
        {
            var validator = new AccountTransactionValidator();
            var validRes = validator.Validate(accountTransactionDTO);
            if (!validRes.IsValid)
            {
                return BadRequest(validRes.Errors.FirstOrDefault());
            }
            else
            {
                var accountTransactiondata = _mapper.Map<AccountTransaction>(accountTransactionDTO);
                //Validate both account to Debit and credit                
                if (!_accountService.isAccountValid(accountTransactiondata.debitAccountId))
                {
                    return BadRequest("Debit account does not exist");
                }               
                if (!_accountService.isAccountValid(accountTransactiondata.creditAccountId))
                {
                    return BadRequest("Credit account does not exist");
                }
                //Validate Transaction PIN
                if (!_accountService.isAccountTransactionPINValid(accountTransactiondata.debitAccountId, accountTransactionDTO.transactionPin.ToString()))
                {
                    return BadRequest("Invalid transaction PIN");
                }
                if (!_accountService.isAccountBalanceSufficient(accountTransactiondata.debitAccountId, accountTransactiondata.amount))
                {
                    return BadRequest("Insufficient Funds in the Debit Account");
                }
                var resp = _accountTransactionService.CreditAccount(accountTransactiondata.debitAccountId, accountTransactiondata.creditAccountId, accountTransactiondata.amount);
                if (resp.responseCode.Equals("00"))
                {
                    return CreatedAtRoute("GetAllAccountTransactions", new { id = accountTransactiondata.accountTransactionId }, accountTransactiondata);
                }
                else
                {
                    return BadRequest(resp);
                }
                
            }


        }
    }
}
