using System.Security.Claims;
using EdTube.Data;
using EdTube.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EdTube.Pages.Video;

public class Upload : PageModel
{
    private readonly ApplicationDbContext _context;

    public Upload(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [BindProperty]
    public UploadVideoModel UploadModel { get; set; }
    
    public async Task<IActionResult> OnGetAsync()
    {
        UploadModel = new UploadVideoModel
        {
            UserId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value,
            Categories = new SelectList(await _context.Categories.Select(c => c.Name).ToListAsync())
        };
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == UploadModel.SelectedCategory);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == UploadModel.UserId);
        var ms = new MemoryStream();
        await UploadModel.Video.CopyToAsync(ms);

        var newVideo = new Data.Entities.Video
        {
            Category = category,
            Name = UploadModel.Name,
            User = user,
            Content = ms.ToArray()
        };
        
        await _context.Videos.AddAsync(newVideo);

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}