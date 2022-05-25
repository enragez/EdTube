using EdTube.Data;
using EdTube.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EdTube.Pages.Categories;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public List<VideoCategoryModel> Categories { get; set; }

    public async Task OnGetAsync()
    {
        Categories = await _context.Categories
            .Select(videoCategory => new VideoCategoryModel
            {
                Name = videoCategory.Name,
                Id = videoCategory.Id,
                Poster = videoCategory.Poster
            })
            .ToListAsync();
    }
}