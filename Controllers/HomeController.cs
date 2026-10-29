using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tarea4SignalR.Controllers;

public sealed class HomeController : Controller
{
    [Authorize]
    public IActionResult Index() => View();

    [Authorize, HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Salir()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults
                .AuthenticationScheme);

        return RedirectToPage("/Login");
    }

    public IActionResult Error() =>
        Content("Ha ocurrido un error.");
}