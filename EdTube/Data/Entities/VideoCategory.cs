namespace EdTube.Data.Entities;

public class VideoCategory
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public byte[]? Poster { get; set; }
}