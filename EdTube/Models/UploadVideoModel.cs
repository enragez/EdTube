using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EdTube.Models;

public class UploadVideoModel
{
    public string UserId { get; set; }
    
    [Display(Name = "Название")]
    public string Name { get; set; }

    [Display(Name = "Выбрать категорию")]
    public string SelectedCategory { get; set; }
    
    public SelectList? Categories { get; set; }
    
    [Display(Name = "Видео")]
    public IFormFile Video { get; set; }
}