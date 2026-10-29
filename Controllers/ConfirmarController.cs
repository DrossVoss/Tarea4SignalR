using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Tarea4SignalR.Hubs;
using Tarea4SignalR.Servicios;

namespace Tarea4SignalR.Controllers;

public sealed class ConfirmarController : Controller
{
    private readonly VerificacionStore _store;
    private readonly IHubContext<VerificacionHub> _hub;

    public ConfirmarController(
        VerificacionStore store,
        IHubContext<VerificacionHub> hub)
    {
        _store = store;
        _hub = hub;
    }

    [HttpGet("/confirmar")]
    public async Task<IActionResult> Index(
        string? token)
    {
        var solicitud = _store.Confirmar(token);

        if (solicitud == null)
            return BadRequest(
                "El enlace no existe o ya no es válido.");

        await _hub.Clients
            .Group($"espera-{solicitud.Id}")
            .SendAsync("VerificacionCompletada");

        return Content(
            "Correo verificado. Volvé a la pestaña " +
            "original: se actualizará sola.",
            "text/plain; charset=utf-8");
    }
}