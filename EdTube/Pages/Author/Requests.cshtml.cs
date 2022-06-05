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
        var requests = await _context.BecomeAuthorRequests.Where(r => !r.Approved && !r.Declined).ToListAsync();

        RequestsList = requests.Select(request => new AuthorRequestModel
            {
                Id = request.Id,
                UserName = request.User.UserName,
                UserId = request.User.Id,
                Approved = false,
                Category = request.Category
            })
            .ToList();
        
        return Page();
    }
}