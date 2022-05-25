using EdTube.Pages;
using EdTube.Data;
using EdTube.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EdTube.Pages.Users;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public List<UserModel> Users { get; set; }

    public void OnGet()
    {
        var users = _context.Users
            .Select(u => new UserModel
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.UserName,
                PhoneNumber = u.PhoneNumber
            })
            .ToList();

        foreach (var user in users)
        {
            var userRoles = _context.UserRoles.Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.RoleId);

            var roleNames = _context.Roles.Where(r => userRoles.Contains(r.Id))
                .Select(r => r.Name)
                .ToList();

            user.Roles = string.Join(',', roleNames);
        }

        Users = users;
    }
}