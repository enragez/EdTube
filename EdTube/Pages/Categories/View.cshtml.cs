using EdTube.Data;
using EdTube.Data.Entities;
using EdTube.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EdTube.Pages.Categories;

public class ViewModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ViewModel(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public List<VideoModel> Videos { get; set; }
    
    public VideoCategoryModel CategoryModel { get; set; }

    public async Task OnGetAsync(int? id)
    {
        var videoCategory = await _context.Categories.FirstOrDefaultAsync(f => f.Id == id);

        CategoryModel = new VideoCategoryModel
        {
            Id = videoCategory.Id,
            Name = videoCategory.Name,
            Poster = videoCategory.Poster
        };
        
        Videos = await _context.Videos
            .Where(fs => fs.Category.Id == id.Value)
            .Select(u => new VideoModel
            {
                Id = u.Id,
                Name = u.Name,
                Content = u.Content
            })
            .ToListAsync();
    }
}