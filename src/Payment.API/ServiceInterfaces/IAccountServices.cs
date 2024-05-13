namespace Payment.API.ServiceInterfaces
{
    public interface IAccountServices
    {
        public bool isAccountValid(string accountId);
        public bool isAccountBalanceSufficient(string accountId, double amount);
        public bool isAccountTransactionPINValid(string accountId, string PIN);
    }
}
