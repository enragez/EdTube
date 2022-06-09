using System.ComponentModel.DataAnnotations;

namespace EdTube.Models;

public class VideoModel
{
    public int Id { get; set; }
    
    [Display(Name = "Название")]
    public string Name { get; set; }
    
    public string FilePath { get; set; }
    
    public string ThumbnailFilePath { get; set; }
    
    public VideoChannelModel ChannelModel { get; set; }
}