using EdTube.Data;
using EdTube.Data.Entities;
using EdTube.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EdTube.Pages.Author;

public class ApproveRequest : PageModel
{
    private readonly ApplicationDbContext _context;

    public ApproveRequest(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [BindProperty]
    public AuthorRequestModel AuthorRequestModel { get; set; }
    
    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var request = await _context.BecomeAuthorRequests.FirstOrDefaultAsync(m => m.Id == id);
        
        if (request == null)
        {
            return NotFound();
        }
        
        AuthorRequestModel = new AuthorRequestModel
        {
            Id = request.Id,
            Approved = false,
            Category = request.Category,
            UserId = request.UserId,
            UserName = (await _context.Users.FirstAsync(u => request.UserId == u.Id)).UserName
        };

        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        var request = await _context.BecomeAuthorRequests.FirstAsync(m => m.Id == AuthorRequestModel.Id);

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
        
        await _context.SaveChangesAsync();

        return RedirectToPage("./Requests");
    }
}