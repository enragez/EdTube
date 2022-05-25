using EdTube.Pages;
using EdTube.Data;
using EdTube.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EdTube.Pages.Users;

public class DeleteModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public DeleteModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty]
    public UserEditModel UserModel { get; set; }

    public async Task<IActionResult> OnGetAsync(string? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
        
        if (user == null)
        {
            return NotFound();
        }
        
        var userRoles = _context.UserRoles.Where(ur => ur.UserId == user.Id)
            .Select(ur => ur.RoleId);

        var roleNames = _context.Roles.Where(r => userRoles.Contains(r.Id))
            .Select(r => r.Name)
            .ToList();
        
        UserModel = new UserEditModel
        {
            Email = user.Email,
            Id = user.Id,
            Name = user.UserName,
            PhoneNumber = user.PhoneNumber,
            IsAdmin = roleNames.Any(r => r == "Администратор"),
            IsContentCreator = roleNames.Any(r => r == "Автор"),
            IsViewer = roleNames.Any(r => r == "Зритель")
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (UserModel.Name == "Главный администратор")
        {
            return Page();
        }
        
        var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == UserModel.Id);
        
        await _userManager.DeleteAsync(user);

        return RedirectToPage("./Index");
    }
}