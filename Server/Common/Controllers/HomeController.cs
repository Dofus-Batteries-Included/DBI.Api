using Microsoft.AspNetCore.Mvc;

namespace Server.Common.Controllers;

/// <summary>
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    ///     Index page
    /// </summary>
    public IActionResult Index() => View();
}
