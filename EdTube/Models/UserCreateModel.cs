using System.ComponentModel.DataAnnotations;

namespace EdTube.Models;

public class UserCreateModel
{
    [Display(Name = "Имя")]
    public string Name { get; set; }
    
    [Display(Name = "Email")]
    public string Email { get; set; }
    
    [Display(Name = "Номер телефона")]
    public string PhoneNumber { get; set; }
    
    [Display(Name = "Пароль")]
    public string Password { get; set; }
    
    [Display(Name = "Администратор")]
    public bool IsAdmin { get; set; }
    
    [Display(Name = "Автор")]
    public bool IsContentCreator { get; set; }
    
    [Display(Name = "Зритель")]
    public bool IsViewer { get; set; }
}