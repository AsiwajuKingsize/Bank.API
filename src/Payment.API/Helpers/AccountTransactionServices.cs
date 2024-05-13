using Payment.API.DTO;
using Payment.API.Models;
using Payment.API.ServiceInterfaces;

namespace Payment.API.Helpers
{
    public class AccountTransactionServices : IAccountTransactionServices
    {
        private readonly AppDbContext _context;
        private readonly IAccountServices _accountService;
        private Object _accountBalanceLock = new Object();
        public AccountTransactionServices(AppDbContext context, IAccountServices accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        public ResponseModel CreditAccount(string debitAccountId, string creditAccountId, double amount)
        {
            ResponseModel resp = new ResponseModel();            

            if (!_accountService.isAccountValid(debitAccountId))
            {
                resp.responseCode = "01";
                resp.responseMessage = "invalid Debit Account";
            }
            if (!_accountService.isAccountValid(creditAccountId))
            {
                resp.responseCode = "01";
                resp.responseMessage = "invalid Credit Account";
            }
            if (!_accountService.isAccountBalanceSufficient(debitAccountId,amount))
            {
                resp.responseCode = "01";
                resp.responseMessage = "insufficient funds in the Credit Account";
            }
            lock (_accountBalanceLock)
            {
                Account debitAccount = _context.Accounts.Where(x => x.accountId == debitAccountId).FirstOrDefault();
                Account creditAccount = _context.Accounts.Where(x => x.accountId == creditAccountId).FirstOrDefault();
                debitAccount.accountBalance -= amount;
                creditAccount.accountBalance += amount;
                _context.AccountTransactions.Add(new AccountTransaction()
                {
                    amount = amount,
                    debitAccountId = debitAccountId,
                    creditAccountId = creditAccountId
                });
                _context.SaveChanges();
            }
            return resp;
        }
    }
}
