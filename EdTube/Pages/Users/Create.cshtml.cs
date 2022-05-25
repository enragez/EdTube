using EdTube.Data;
using EdTube.Pages;
using EdTube.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EdTube.Pages.Users;

public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public CreateModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    
    [BindProperty]
    public UserCreateModel UserModel { get; set; }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = new IdentityUser
        {
            Email = UserModel.Email,
            PhoneNumber = UserModel.PhoneNumber,
            UserName = UserModel.Name
        };
        
        var createResult = await _userManager.CreateAsync(user, UserModel.Password);
        if (createResult.Succeeded)
        {
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
        }
        

        return RedirectToPage("./Index");
    }
}