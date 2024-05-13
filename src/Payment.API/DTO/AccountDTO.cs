using Payment.API.Models;

namespace Payment.API.DTO
{
    public class AccountDTO
    {
        public string surname { get; set; }
        public string firstName { get; set; }
        public int transactionPin { get; set; }
    }

    public class AccountResponseDTO : BaseModel
    {
        public string accountId { get; set; }
        public string surname { get; set; }
        public string firstName { get; set; }

        public double accountBalance { get; set; } 

    }
}
