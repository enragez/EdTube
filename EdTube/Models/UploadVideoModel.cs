using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EdTube.Models;

public class UploadVideoModel
{
    public string UserId { get; set; }
    
    [Display(Name = "Название")]
    public string Name { get; set; }

    [Display(Name = "Выбрать канал")]
    public string SelectedChannel { get; set; }
    
    public SelectList? Channels { get; set; }
    
    [Display(Name = "Видео")]
    public IFormFile Video { get; set; }
    
    [Display(Name = "Создать проверочный материал")]
    public bool AttachTest { get; set; }
}