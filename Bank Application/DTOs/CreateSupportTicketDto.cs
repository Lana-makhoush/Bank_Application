using System.ComponentModel.DataAnnotations;

namespace Bank_Application.DTOs
{
    public class CreateSupportTicketDto
    {
        [Required(ErrorMessage = "حقل الموضوع اجباري")]

        [StringLength(100, MinimumLength = 3, ErrorMessage = "الموضوع يجب أن يكون بين 3 و 100 حرف")]
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z0-9\s.,!?-]+$",
            ErrorMessage = "الوصف يجب أن يكون نصًا عربيًا أو إنجليزيًا فقط")]

        public string? Subject { get; set; }
        [Required(ErrorMessage = "حقل الوصف اجباري")]
        [StringLength(1000, ErrorMessage = "الوصف طويل جدًا")]
        [RegularExpression(@"^[\u0621-\u064A a-zA-Z0-9\s]+$",
    ErrorMessage = "الموضوع يجب أن يحتوي على أحرف وأرقام فقط")]

        public string? Description { get; set; }


    }
}