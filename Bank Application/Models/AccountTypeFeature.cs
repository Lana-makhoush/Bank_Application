using System.ComponentModel.DataAnnotations;

namespace Bank_Application.Models
{
    public class AccountTypeFeature
    {
        [Key]
        public int Id { get; set; }

        public int? AccountTypeId { get; set; }
        public AccountType? AccountType { get; set; }

        public int? FeatureId { get; set; }
        public Feature? Feature { get; set; }
    }
}
