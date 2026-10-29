const panel = document.getElementById("panel-espera");
const id = panel.dataset.id;
const estado = document.getElementById("estado");
const formulario = document.getElementById("form-finalizar");

let finalizando = false;

const conexion = new signalR.HubConnectionBuilder()
    .withUrl("/verificacionHub")
    .withAutomaticReconnect()
    .build();

function finalizar() {
    if (finalizando) return;

    finalizando = true;
    estado.textContent = "¡Verificado! Iniciando sesión...";

    formulario.requestSubmit();
}

conexion.on("VerificacionCompletada", finalizar);

async function suscribirseYConsultar() {
    await conexion.invoke("Unirse", id);

    estado.textContent =
        "Conectado. Esperando que abras el enlace...";

    if (await conexion.invoke("EstaVerificada", id)) {
        finalizar();
    }
}

conexion.onreconnected(async () => {
    try {
        await suscribirseYConsultar();
    } catch (error) {
        console.error(error);
    }
});

async function iniciar() {
    try {
        await conexion.start();
        await suscribirseYConsultar();
    } catch (error) {
        estado.textContent =
            "No se pudo conectar. Reintentando...";

        console.error(error);
        setTimeout(iniciar, 3000);
    }
}

iniciar();