namespace EdTube.Data.Entities;

public class TestQuestion
{
    public int Id { get; set; }
    
    public int Index { get; set; }
    
    public Video Video { get; set; }
    
    public string Question { get; set; }
    
    public string FirstOption { get; set; }
    
    public string SecondOption { get; set; }
    
    public string ThirdOption { get; set; }
    
    public string FourthOption { get; set; }
    
    public string RightOption { get; set; }
}