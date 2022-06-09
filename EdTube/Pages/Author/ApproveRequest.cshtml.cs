using EdTube.Data;
using EdTube.Data.Entities;
using EdTube.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EdTube.Pages.Author;

public class ApproveRequest : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ApproveRequest(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    [BindProperty]
    public AuthorRequestModel AuthorRequestModel { get; set; }
    
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var request = await _context.BecomeAuthorRequests
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);
        
        if (request == null)
        {
            return NotFound();
        }

        AuthorRequestModel = new AuthorRequestModel
        {
            Id = request.Id,
            ChannelName = request.ChannelName,
            Approved = false,
            Category = request.Category,
            UserId = request.User.Id,
            UserName = request.User.UserName
        };

        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        var request = await _context.BecomeAuthorRequests
            .Include(r => r.User)
            .FirstAsync(r => r.Id == AuthorRequestModel.Id);

        request.Approved = true;
        request.Declined = false;

        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == request.Category);
        if (category == null)
        {
            category = new VideoCategory
            {
                Name = request.Category
            };

            _context.Categories.Add(category);
        }

        var videoChannel = new VideoChannel
        {
            Name = request.ChannelName,
            Category = category,
            User = request.User,
            Poster = request.Poster
        };

        _context.Channels.Add(videoChannel);
        
        var user = await _userManager.FindByIdAsync(AuthorRequestModel.UserId);

        if (await _userManager.IsInRoleAsync(user, "Автор"))
        {
            await _context.SaveChangesAsync();
        }
        else
        {
            await _userManager.AddToRoleAsync(user, "Автор");
        
            await _context.SaveChangesAsync();
        }
        
        return RedirectToPage("./Requests");
    }
}