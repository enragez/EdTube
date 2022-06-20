namespace EdTube.Models;

public class HomeViewModel
{
    public List<VideoCategoryModel> Categories { get; set; }
    
    public List<VideoChannelModel> ChannelModels { get; set; }
    
    public List<VideoModel> VideoModels { get; set; }
    
    public bool WithSearchResult { get; set; }
    
    public string SearchString { get; set; }
}