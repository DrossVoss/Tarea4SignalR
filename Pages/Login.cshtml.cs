using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tarea4SignalR.Servicios;

namespace Tarea4SignalR.Pages;

public sealed class LoginModel : PageModel
{
    private readonly VerificacionStore _store;

    public LoginModel(VerificacionStore store)
    {
        _store = store;
    }

    [BindProperty, Required]
    public string Nombre { get; set; } = "";

    [BindProperty, Required, EmailAddress]
    public string Email { get; set; } = "";

    public IActionResult OnGet() =>
        User.Identity?.IsAuthenticated == true
            ? RedirectToAction("Index", "Home")
            : Page();

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        var solicitud = _store.Crear(
            Nombre.Trim(), Email.Trim());

        Response.Cookies.Append(
            "Tarea4.Espera",
            solicitud.ClaveNavegador,
            new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Secure = Request.IsHttps,
                MaxAge = TimeSpan.FromMinutes(30)
            });

        return RedirectToPage(
            "/Espera", new { id = solicitud.Id });
    }
}