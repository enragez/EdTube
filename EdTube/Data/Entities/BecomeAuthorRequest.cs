namespace EdTube.Data.Entities;

public class BecomeAuthorRequest
{
    public int Id { get; set; }
    
    public string UserId { get; set; }
    
    public string Category { get; set; }
    
    public bool Approved { get; set; }
    
    public bool Declined { get; set; }
}