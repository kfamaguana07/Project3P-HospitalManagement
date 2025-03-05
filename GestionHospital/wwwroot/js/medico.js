window.onload = function () {
    const roles = userRoles.split(',');
    const hasAccess = roles.includes('Admin') || roles.includes('Staff');

    if (hasAccess) {
        ListarMedico();
    } else {
        PintarDatosMedico();
    }
}

let objMedico;

async function ListarMedico() {
    const roles = userRoles.split(',');
   
    if (!roles.includes('Admin')) {
        let btnNuevo = document.getElementById("btnNuevoMedico");
        btnNuevo.style.display = "none";
    }

    objMedico = {
        url: "Medico/ListarMedico",
        cabeceras: ["Id Médico", "Nombre", "Apellido", "Teléfono", "Email"],
        propiedades: ["idMedico", "nombre", "apellido", "telefono", "email"],
        editar: roles.includes('Admin'),
        eliminar: roles.includes('Admin'),
        propiedadID: "idMedico"
    }

    pintar(objMedico);
}

function PintarDatosMedico() {
    fetchGet("Medico/ObtenerIdMedicoActual", "json", function (id) {
        fetchGet("Medico/RecuperarMedico/?idMedico=" + id, "json", function (data) {
            if (data) {
                document.getElementById("nombre").textContent = data.nombre || "No disponible";
                document.getElementById("apellido").textContent = data.apellido || "No disponible";
                document.getElementById("telefono").textContent = data.telefono || "No disponible";
                document.getElementById("email").textContent = data.email || "No disponible";
            }
        });
    });
}

function LimpiarMedico() {
    LimpiarDatos("frmMedico");
    ListarMedico();
}

function ValidarFormulario() {
    let form = document.getElementById("frmMedico");
    let isValid = true;

    // Validar campos de texto
    let camposTexto = ['nombre', 'apellido', 'telefono', 'email'];
    camposTexto.forEach(function (campo) {
        let input = form.querySelector('#' + campo);
        if (input.value.trim() === '') {
            input.classList.add('is-invalid');
            isValid = false;
        } else {
            input.classList.remove('is-invalid');
        }
    });

    return isValid;
}

function GuardarMedico() {
    if (!ValidarFormulario()) {
        return;
    }

    let form = document.getElementById("frmMedico");
    let frm = new FormData(form);
    fetchPost("Medico/GuardarMedico", "text", frm, function (res) {
        // Perform other operations first
        Exito("Registro Guardado con Exito");
        let email = document.getElementById("email").value;
        let mensaje = "Usuario: " + email + " Contraseña: " + res;
        AlertaCopiar(mensaje, undefined, mensaje);
        ListarMedico();

        var myModal = bootstrap.Modal.getInstance(document.getElementById('modalMedico'));
        myModal.hide();
    });
}

function MostrarModal() {
    LimpiarDatos("frmMedico");
    var myModal = new bootstrap.Modal(document.getElementById('modalMedico'));
    myModal.show();
}

function Editar(id) {
    fetchGet("Medico/RecuperarMedico/?idMedico=" + id, "json", function (data) {
        setN("idMedico", data.idMedico);
        setN("nombre", data.nombre);
        setN("apellido", data.apellido);
        setN("telefono", data.telefono);
        setN("email", data.email);

        // Show the modal
        var myModal = new bootstrap.Modal(document.getElementById('modalMedico'));
        myModal.show();
    });
}

function Eliminar(id) {
    fetchGet("Medico/RecuperarMedico/?idMedico=" + id, "json", function (data) {
        Confirmar(undefined, "¿Desea eliminar el médico " + data.nombre + " " + data.apellido + "?", function () {
            fetchGet("Medico/EliminarMedico/?idMedico=" + id, "text", function (r) {
                Exito("Registro Eliminado con Exito");
                ListarMedico();
            });
        });
    });
}