namespace Payment.API.Models
{
    public class AccountTransaction : BaseModel
    {
        public Guid accountTransactionId {get;set;} = System.Guid.NewGuid();

        public double amount { get; set; }

        public string debitAccountId { get; set; }

        public string creditAccountId { get; set; }       


    }
}
