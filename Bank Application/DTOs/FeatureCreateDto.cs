using System.ComponentModel.DataAnnotations;

public class FeatureCreateDto
{
    [Required(ErrorMessage = "حقل الاسم مطلوب")]
    public string Name { get; set; }
    [Required(ErrorMessage = "حقل الوصف مطلوب")]

    public string? Description { get; set; }
}
