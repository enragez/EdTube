using System.ComponentModel.DataAnnotations;

namespace EdTube.Models;

public class VideoCategoryEditModel
{
    public int Id { get; set; }
    
    [Display(Name = "Название")]
    public string Name { get; set; }
    
    [Display(Name = "Обложка")]
    public IFormFile File { get; set; }
    
    public byte[]? ExistingPoster { get; set; }
}