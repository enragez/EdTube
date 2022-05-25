using System.ComponentModel.DataAnnotations;

namespace EdTube.Models;

public class UserModel
{
    public string Id { get; set; }
    
    [Display(Name = "Имя")]
    public string Name { get; set; }
    
    [Display(Name = "Email")]
    public string Email { get; set; }
    
    [Display(Name = "Номер телефона")]
    public string PhoneNumber { get; set; }
    
    [Display(Name = "Роли")]
    public string Roles { get; set; }
    
    [Display(Name = "Администратор")]
    public bool IsAdmin { get; set; }
    
    [Display(Name = "Автор")]
    public bool IsContentCreator { get; set; }
    
    [Display(Name = "Зритель")]
    public bool IsViewer { get; set; }
}