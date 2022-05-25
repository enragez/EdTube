using System.ComponentModel.DataAnnotations;

namespace EdTube.Models;

public class VideoCategoryCreateModel
{
    public int Id { get; set; }
    
    [Display(Name = "Название")]
    public string Name { get; set; }
    
    [Display(Name = "Обложка")]
    public IFormFile File { get; set; }
}