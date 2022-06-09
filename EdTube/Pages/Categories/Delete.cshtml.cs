using EdTube.Data;
using EdTube.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EdTube.Pages.Categories;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context)
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
            Name = videoCategory.Name
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == CategoryModel.Id);

        var channels = await _context.Channels.Where(c => c.Category.Id == category.Id).ToListAsync();
        
        _context.Categories.Remove(category);
        _context.Channels.RemoveRange(channels);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}