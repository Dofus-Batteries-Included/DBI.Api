using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

/// <summary>
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    /// </summary>
    public IActionResult Index() => View();
}
