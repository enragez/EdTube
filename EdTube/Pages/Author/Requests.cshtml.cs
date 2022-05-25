using EdTube.Data;
using EdTube.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EdTube.Pages.Author;

public class Requests : PageModel
{
    private readonly ApplicationDbContext _context;

    public Requests(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [BindProperty]
    public List<AuthorRequestModel> RequestsList { get; set; }
    
    public async Task<IActionResult> OnGetAsync()
    {
        var result = new List<AuthorRequestModel>();

        var requests = _context.BecomeAuthorRequests.Where(r => !r.Approved && !r.Declined);

        foreach (var request in requests)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => request.UserId == u.Id);
            if (user == null)
            {
                continue;
            }
            
            result.Add(new AuthorRequestModel
            {
                Id = request.Id,
                UserName = user.UserName,
                UserId = user.Id,
                Approved = false,
                Category = request.Category
            });
        }
        
        RequestsList = result;
        
        return Page();
    }
}