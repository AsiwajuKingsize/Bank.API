namespace Payment.API.Models
{
    public class BaseModel
    {
        public DateTime createdDate { get; set; } = DateTime.UtcNow;
    }
}
