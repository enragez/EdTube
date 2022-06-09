using Microsoft.AspNetCore.Identity;

namespace EdTube.Data.Entities;

public class VideoChannel
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public IdentityUser User { get; set; }
    
    public VideoCategory Category { get; set; }
    
    public byte[] Poster { get; set; }
}