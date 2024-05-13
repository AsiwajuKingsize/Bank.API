namespace Payment.API.DTO
{
    public class AccountTransactionDTO
    {
        public double amount { get; set; }

        public string debitAccountId { get; set; }

        public string creditAccountId { get; set; }

        public int transactionPin { get; set; }

    }
}
