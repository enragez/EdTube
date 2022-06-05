using System.Security.Claims;
using EdTube.Data;
using EdTube.Data.Entities;
using EdTube.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EdTube.Pages.Author;

public class Become : PageModel
{
    private readonly ApplicationDbContext _context;

    public Become(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [BindProperty]
    public BecomeAuthorRequestModel Model { get; set; }
    
    public async Task<IActionResult> OnGetAsync()
    {
        Model = new BecomeAuthorRequestModel
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

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == Model.UserId);
        
        var newRequest = new BecomeAuthorRequest
        {
            User = user,
            Category = !string.IsNullOrEmpty(Model.NewCategory) ? Model.NewCategory : Model.SelectedCategory,
            Approved = false,
            Declined = false
        };
        
        await _context.BecomeAuthorRequests.AddAsync(newRequest);

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}