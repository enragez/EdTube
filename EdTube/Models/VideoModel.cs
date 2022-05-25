using System.ComponentModel.DataAnnotations;

namespace EdTube.Models;

public class VideoModel
{
    public int Id { get; set; }
    
    [Display(Name = "Название")]
    public string Name { get; set; }
    
    [Display(Name = "Контент")]
    public byte[] Content { get; set; }
}