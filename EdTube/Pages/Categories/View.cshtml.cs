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
    
    public List<VideoChannelModel> Channels { get; set; }
    
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

        Channels = await _context.Channels
            .Include(c => c.User)
            .Where(c => c.Category.Id == id.Value)
            .Select(c => new VideoChannelModel
            {
                Id = c.Id,
                Name = c.Name,
                Category = CategoryModel,
                UserId = c.User.Id,
                Poster = c.Poster
            })
            .ToListAsync();
    }
}