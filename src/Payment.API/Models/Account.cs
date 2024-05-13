namespace Payment.API.Models
{
    public class Account : BaseModel
    {
        public string accountId { get; set; }
        public string surname { get; set; }
        public string firstName { get; set; }

        public double accountBalance { get; set; } = 0.00;    

        public string transactionPin { get; set; }
    }
}
