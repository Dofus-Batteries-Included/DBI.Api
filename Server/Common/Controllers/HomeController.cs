using Microsoft.AspNetCore.Mvc;

namespace Server.Common.Controllers;

/// <summary>
/// </summary>
public class HomeController : Controller
{
    public IActionResult Index() => View();
}
