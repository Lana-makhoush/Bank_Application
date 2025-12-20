using System.ComponentModel.DataAnnotations;

public class FeatureCreateDto
{
    [Required(ErrorMessage = "حقل الاسم مطلوب")]
    public string Name { get; set; }
    [Required(ErrorMessage = "حقل الوصف مطلوب")]

    public string? Description { get; set; }
    [Required(ErrorMessage = "حقل الكلفة مطلوب")]

    [Range(0, double.MaxValue, ErrorMessage = "المبلغ يجب أن يكون رقمًا موجبًا")]
    public decimal Cost { get; set; }
}
