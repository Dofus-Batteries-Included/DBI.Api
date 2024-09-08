using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

/// <summary>
/// </summary>
public class HomeController : Controller
{
    public IActionResult Index() => View();
}
