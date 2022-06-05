using EdTube.Data;
using EdTube.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EdTube.Pages.Author;

public class DeclineRequest : PageModel
{
    private readonly ApplicationDbContext _context;

    public DeclineRequest(ApplicationDbContext context)
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
            UserId = request.User.Id,
            UserName = request.User.UserName
        };

        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        var request = await _context.BecomeAuthorRequests.FirstAsync(m => m.Id == AuthorRequestModel.Id);

        request.Approved = false;
        request.Declined = true;
        
        await _context.SaveChangesAsync();

        return RedirectToPage("./Requests");
    }
}