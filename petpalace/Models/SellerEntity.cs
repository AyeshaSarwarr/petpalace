namespace petpalace.Models
{
    public class SellerEntity : UserEntity
    {
        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }
        public string ContactNumber { get; set; }
        public string Website { get; set; }
    }
}
