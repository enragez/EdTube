namespace EdTube.Models;

public class AuthorRequestModel
{
    public int Id { get; set; }
    
    public string UserName { get; set; }
    
    public string UserId { get; set; }
    
    public string Category { get; set; }
    
    public bool Approved { get; set; }
}