using System.Diagnostics;
using EdTube.Data;
using Microsoft.AspNetCore.Mvc;
using EdTube.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EdTube.Controllers;

public class HomeController : Controller
{
    private static bool _defaultsInitialized = false;
    
    
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        await CreateDefaultUsersAndRoles();
        
        return View(new HomeViewModel
        {
            Categories = await _context.Categories
                .Select(u => new VideoCategoryModel
                {
                    Name = u.Name,
                    Id = u.Id,
                    Poster = u.Poster,
                })
                .ToListAsync()
        });
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    private async ValueTask CreateDefaultUsersAndRoles()
    {
        if (_defaultsInitialized)
        {
            return;
        }
        
        string[] roleNames = { "Администратор", "Зритель" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var superUser = await _userManager.FindByEmailAsync("root@root.ru");

        //await _userManager.DeleteAsync(superUser);
        
        if (superUser == null)
        {
            var superUserIdentity = new IdentityUser
            {
                Email = "root@root.ru",
                UserName = "Главный администратор",
                PhoneNumber = "123456789"
            };
            
            var createSuperUserResult = await _userManager.CreateAsync(superUserIdentity, "123");

            if (createSuperUserResult.Succeeded)
            {
                await _userManager.AddToRolesAsync(superUserIdentity, roleNames);
            }
        }

        _defaultsInitialized = true;
    }
}