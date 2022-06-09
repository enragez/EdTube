using System.ComponentModel.DataAnnotations;

namespace EdTube.Models;

public class VideoChannelModel
{
    public int Id { get; set; }
    
    public string UserId { get; set; }
    
    [Display(Name = "Название канала")]
    public string Name { get; set; }
    
    [Display(Name = "Категория")]
    public VideoCategoryModel Category { get; set; }
    
    [Display(Name = "Обложка")]
    public byte[]? Poster { get; set; }
}