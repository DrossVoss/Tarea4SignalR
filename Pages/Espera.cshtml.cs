using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tarea4SignalR.Servicios;

namespace Tarea4SignalR.Pages;

public sealed class EsperaModel : PageModel
{
    private readonly VerificacionStore _store;

    public EsperaModel(VerificacionStore store)
    {
        _store = store;
    }

    public SolicitudVerificacion Solicitud
        { get; private set; } = null!;

    public string EnlaceVerificacion
        { get; private set; } = "";

    public IActionResult OnGet(string id)
    {
        if (!_store.TieneAcceso(
            id, Request.Cookies["Tarea4.Espera"]))
            return RedirectToPage("/Login");

        Solicitud = _store.Obtener(id)!;

        EnlaceVerificacion = Url.Action(
            "Index", "Confirmar",
            new { token = Solicitud.Token },
            Request.Scheme)!;

        return Page();
    }

    public async Task<IActionResult>
        OnPostFinalizarAsync(string id)
    {
        if (!_store.TieneAcceso(
            id, Request.Cookies["Tarea4.Espera"]))
            return RedirectToPage("/Login");

        var solicitud = _store.Obtener(id)!;

        if (!solicitud.Verificada)
            return RedirectToPage(
                "/Espera", new { id });

        var claims = new[]
        {
            new Claim(
                ClaimTypes.Name, solicitud.Nombre),
            new Claim(
                ClaimTypes.Email, solicitud.Email)
        };

        var identidad = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults
                .AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults
                .AuthenticationScheme,
            new ClaimsPrincipal(identidad));

        Response.Cookies.Delete("Tarea4.Espera");
        _store.Eliminar(id);

        return RedirectToAction("Index", "Home");
    }
}