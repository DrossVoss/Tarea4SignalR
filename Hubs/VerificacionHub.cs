using Microsoft.AspNetCore.SignalR;
using Tarea4SignalR.Servicios;

namespace Tarea4SignalR.Hubs;

public sealed class VerificacionHub : Hub
{
    private readonly VerificacionStore _store;

    public VerificacionHub(VerificacionStore store)
    {
        _store = store;
    }

    private void ComprobarAcceso(string id)
    {
        string? clave = Context.GetHttpContext()?
            .Request.Cookies["Tarea4.Espera"];

        if (!_store.TieneAcceso(id, clave))
            throw new HubException(
                "No tenés acceso a esta verificación.");
    }

    public Task Unirse(string id)
    {
        ComprobarAcceso(id);

        return Groups.AddToGroupAsync(
            Context.ConnectionId, $"espera-{id}");
    }

    public Task<bool> EstaVerificada(string id)
    {
        ComprobarAcceso(id);

        return Task.FromResult(
            _store.Obtener(id)?.Verificada == true);
    }
}