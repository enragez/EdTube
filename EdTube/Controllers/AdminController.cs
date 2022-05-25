using Microsoft.AspNetCore.Mvc;

namespace EdTube.Controllers;

public class AdminController : Controller
{
    public IActionResult Index()
    {        
        return View();
    }
}