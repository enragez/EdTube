using Microsoft.AspNetCore.Identity;

namespace EdTube.Data.Entities;

public class Video
{
    public int Id { get; set; }
    
    public IdentityUser User { get; set; }
    
    public string Name { get; set; }
    
    public byte[] Content { get; set; }
    
    public VideoChannel Channel { get; set; }
    
    public string FileName { get; set; }
}