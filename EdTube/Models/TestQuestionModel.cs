using System.ComponentModel.DataAnnotations;

namespace EdTube.Models;

public class TestQuestionModel
{
    public int Index { get; set; }
    
    public int VideoId { get; set; }
    
    [Display(Name = "Вопрос")]
    public string Question { get; set; }
    
    [Display(Name = "1 вариант ответа")]
    public string FirstOption { get; set; }
    
    [Display(Name = "2 вариант ответа")]
    public string SecondOption { get; set; }
    
    [Display(Name = "3 вариант ответа")]
    public string ThirdOption { get; set; }
    
    [Display(Name = "4 вариант ответа")]
    public string FourthOption { get; set; }
    
    [Display(Name = "Правильный вариант ответа")]
    public string RightOption { get; set; }
}