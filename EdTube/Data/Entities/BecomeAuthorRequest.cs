using Microsoft.AspNetCore.Identity;

namespace EdTube.Data.Entities;

public class BecomeAuthorRequest
{
    public int Id { get; set; }
    
    public IdentityUser User { get; set; }
    
    public string ChannelName { get; set; }
    
    public string Category { get; set; }
    
    public bool Approved { get; set; }
    
    public bool Declined { get; set; }
    
    public byte[] Poster { get; set; }
}