# Taller .NET 2026 - Tarea 4: SignalR

## Objetivo
Mostrar una página de registro y una pantalla en espera. Al abrir un enlace de confirmación simulado en otra pestaña, el servidor notifica mediante SignalR a la pestaña original, que inicia una sesión por cookie y redirige automáticamente a la página principal.

## Requisitos
SDK .NET 8 y CLI LibMan. El proyecto no necesita base de datos ni email real.

## Instalar cliente JavaScript
```powershell
dotnet tool install --global Microsoft.Web.LibraryManager.Cli
libman install @microsoft/signalr@8.0.0 -p unpkg -d wwwroot/lib/signalr --files dist/browser/signalr.min.js
```
Si LibMan ya está instalado, omitir la primera línea.

## Ejecutar
```powershell
dotnet restore
dotnet run --urls http://localhost:5074
```
Entrar a http://localhost:5074/Login, ingresar nombre/email, dejar abierta la pantalla en espera, abrir el enlace de verificación en otra pestaña y volver a la original. Se debe redirigir sin recargar manualmente.

## Componentes
- `Pages/Login`: formulario Razor Pages.
- `Pages/Espera`: espera y finalización de autenticación.
- `Hubs/VerificacionHub`: conexión SignalR y grupos.
- `Controllers/ConfirmarController`: confirma token y notifica al grupo.
- `Controllers/HomeController`: página principal protegida.
- `Servicios/VerificacionStore`: estado en memoria.
- `wwwroot/js/espera.js`: recepción de evento y redirección.
- `diagrama.mmd`: diagrama de secuencia simple.

## Límites de la demo
Sin email real, contraseñas, base de datos ni persistencia tras reinicios. El enlace de prueba es visible en la pantalla para simplificar la verificación. El acceso a la pantalla en espera se limita usando una cookie temporal, y la página principal requiere autenticación por cookie. No usar este ejemplo como sistema de autenticación de producción; incorporar ASP.NET Core Identity, HTTPS, tokens con caducidad y almacenamiento persistente.
