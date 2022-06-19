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
        var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var channels = await  _context.Channels.Where(c => c.User.Id == userId).Select(c => c.Name).ToListAsync();
        UploadModel = new UploadVideoModel
        {
            UserId = userId,
            Channels = new SelectList(channels)
        };
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var channel = await _context.Channels.FirstOrDefaultAsync(c => c.Name == UploadModel.SelectedChannel);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == UploadModel.UserId);
        var ms = new MemoryStream();
        await UploadModel.Video.CopyToAsync(ms);

        var newVideo = new Data.Entities.Video
        {
            Name = UploadModel.Name,
            User = user,
            Channel = channel,
            Content = ms.ToArray(),
            FileName = UploadModel.Video.FileName
        };
        
        await _context.Videos.AddAsync(newVideo);

        await _context.SaveChangesAsync();
        
        return UploadModel.AttachTest 
            ? RedirectToPage("../Test/AddQuestion", new { videoId = newVideo.Id, index = 0 }) 
            : RedirectToPage("./Index");
    }
}