﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EdTube.Models;

public class BecomeAuthorRequestModel
{
    public string UserId { get; set; }

    [Display(Name = "Выбрать категорию")]
    public string SelectedCategory { get; set; }
    
    [Display(Name = "Создать новую категорию")]
    public string? NewCategory { get; set; }
    
    public SelectList? Categories { get; set; }
}