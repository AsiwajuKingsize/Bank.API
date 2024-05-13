using Payment.API.Models;

namespace Payment.API.ServiceInterfaces
{
    public interface IAccountTransactionServices
    {
        public ResponseModel CreditAccount(string debitAccountId, string creditAccountId, double amount);

    }
}
