using System.Collections.Concurrent;

namespace Tarea4SignalR.Servicios;

public sealed class SolicitudVerificacion
{
    public string Id { get; init; } =
        Guid.NewGuid().ToString("N");

    public string Token { get; init; } =
        Guid.NewGuid().ToString("N");

    public string ClaveNavegador { get; init; } =
        Guid.NewGuid().ToString("N");

    public string Nombre { get; init; } = "";
    public string Email { get; init; } = "";

    private int _verificada;

    public bool Verificada =>
        Volatile.Read(ref _verificada) == 1;

    public void MarcarVerificada() =>
        Interlocked.Exchange(ref _verificada, 1);
}

public sealed class VerificacionStore
{
    private readonly ConcurrentDictionary<string,
        SolicitudVerificacion> _porId = new();

    private readonly ConcurrentDictionary<string,
        string> _idPorToken = new();

    public SolicitudVerificacion Crear(
        string nombre, string email)
    {
        var solicitud = new SolicitudVerificacion
        {
            Nombre = nombre,
            Email = email
        };

        _porId[solicitud.Id] = solicitud;
        _idPorToken[solicitud.Token] = solicitud.Id;

        return solicitud;
    }

    public SolicitudVerificacion? Obtener(string? id) =>
        !string.IsNullOrWhiteSpace(id) &&
        _porId.TryGetValue(id, out var solicitud)
            ? solicitud : null;

    public bool TieneAcceso(
        string? id, string? claveNavegador)
    {
        var solicitud = Obtener(id);

        return solicitud != null &&
            !string.IsNullOrWhiteSpace(claveNavegador) &&
            solicitud.ClaveNavegador == claveNavegador;
    }

    public SolicitudVerificacion? Confirmar(
        string? token)
    {
        if (string.IsNullOrWhiteSpace(token) ||
            !_idPorToken.TryGetValue(token, out var id))
            return null;

        var solicitud = Obtener(id);
        solicitud?.MarcarVerificada();

        return solicitud;
    }

    public void Eliminar(string id)
    {
        if (_porId.TryRemove(id, out var solicitud))
            _idPorToken.TryRemove(
                solicitud.Token, out _);
    }
}