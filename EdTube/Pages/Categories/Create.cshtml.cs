using EdTube.Data;
using EdTube.Data.Entities;
using EdTube.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EdTube.Pages.Categories;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [BindProperty]
    public VideoCategoryCreateModel CategoryModel { get; set; }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var ms = new MemoryStream();
        await CategoryModel.File.CopyToAsync(ms);

        var videoCategory = new VideoCategory
        {
            Name = CategoryModel.Name,
            Poster = ms.ToArray()
        };

        await _context.Categories.AddAsync(videoCategory);

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}