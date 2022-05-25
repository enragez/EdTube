using EdTube.Data;
using EdTube.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EdTube.Pages.Categories;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [BindProperty]
    public VideoCategoryEditModel CategoryModel { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var videoCategory = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
        
        if (videoCategory == null)
        {
            return NotFound();
        }

        CategoryModel = new VideoCategoryEditModel
        {
            Id = videoCategory.Id,
            ExistingPoster = videoCategory.Poster,
            Name = videoCategory.Name
        };
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var ms = new MemoryStream();
        await CategoryModel.File.CopyToAsync(ms);

        var category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == CategoryModel.Id);
        
        category.Name = CategoryModel.Name;
        category.Poster = ms.ToArray();

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}