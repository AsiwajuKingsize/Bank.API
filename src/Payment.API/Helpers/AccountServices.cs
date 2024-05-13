using Microsoft.EntityFrameworkCore;
using Payment.API.ServiceInterfaces;

namespace Payment.API.Helpers
{
    public class AccountServices : IAccountServices
    {
        private readonly AppDbContext _context;
        public AccountServices(AppDbContext context)
        {
            _context = context;
        }
        public bool isAccountValid(string accountId)
        {
            return  _context.Accounts.Any(x => x.accountId == accountId);
        }

        public bool isAccountBalanceSufficient(string accountId, double amount)
        {
            bool resp = true;
            if (isAccountValid(accountId))
            {
                var _account = _context.Accounts.Where(x => x.accountId == accountId).FirstOrDefault();
                if (_account.accountBalance < amount)
                {
                    resp = false;
                }
            }
            else
            {
                return resp;
            }
            return resp;
        }

        public bool isAccountTransactionPINValid(string accountId, string PIN)
        {
            bool resp = true;
            if (!isAccountValid(accountId))
            {
                var _account = _context.Accounts.Where(x => x.accountId == accountId).FirstOrDefault();
                if (!SecurePayment.ComputeSha256Hash(PIN).Equals(_account.transactionPin))
                {
                    resp = false;
                }
            }
            return resp;
        }
    }
}
