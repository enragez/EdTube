using EdTube.Pages;
using EdTube.Data;
using EdTube.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EdTube.Pages.Users;

public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public EditModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
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
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == UserModel.Id);

        await _userManager.SetEmailAsync(user, UserModel.Email);
        await _userManager.SetPhoneNumberAsync(user, UserModel.PhoneNumber);
        await _userManager.SetUserNameAsync(user, UserModel.Name);

        if (!string.IsNullOrEmpty(UserModel.NewPassword))
        {
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, UserModel.NewPassword);
        }

        switch (UserModel.IsAdmin)
        {
            case true when !await _userManager.IsInRoleAsync(user, "Администратор"):
                await _userManager.AddToRoleAsync(user, "Администратор");
                break;
            case false when await _userManager.IsInRoleAsync(user, "Администратор") && user.UserName != "Главный администратор":
                await _userManager.RemoveFromRoleAsync(user, "Администратор");
                break;
        }
            
        switch (UserModel.IsContentCreator)
        {
            case true when !await _userManager.IsInRoleAsync(user, "Автор"):
                await _userManager.AddToRoleAsync(user, "Автор");
                break;
            case false when await _userManager.IsInRoleAsync(user, "Автор"):
                await _userManager.RemoveFromRoleAsync(user, "Автор");
                break;
        }
            
        switch (UserModel.IsViewer)
        {
            case true when !await _userManager.IsInRoleAsync(user, "Зритель"):
                await _userManager.AddToRoleAsync(user, "Зритель");
                break;
            case false when await _userManager.IsInRoleAsync(user, "Зритель"):
                await _userManager.RemoveFromRoleAsync(user, "Зритель");
                break;
        }

        return RedirectToPage("./Index");
    }
}