using System.ComponentModel.DataAnnotations;

namespace Bank_Application.Models
{
    public class Feature
    {
        [Key]
        public int? FeatureId { get; set; }

        [Required(ErrorMessage = "اسم الميزة مطلوب")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "اسم الميزة يجب أن يكون بين 2 و 50 حرف")]
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z]+$",
            ErrorMessage = "اسم الميزة يجب أن يحتوي على أحرف عربية أو إنجليزية فقط")]
        public string? FeatureName { get; set; }

        [StringLength(200, MinimumLength = 5, ErrorMessage = "الوصف يجب أن يكون بين 5 و 200 حرف")]
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z0-9\s.,\-]*$",
            ErrorMessage = "الوصف يمكن أن يحتوي على أحرف عربية، إنجليزية، أرقام، ومسافات، وعلامات ترقيم . , -")]
        public string? Description { get; set; }

        public ICollection<AccountTypeFeature>? AccountTypeFeatures { get; set; }


    }
}
