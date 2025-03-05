window.onload = function () {
    ListarTratamiento();
    ListarPacientes();
}

let objTratamiento;

async function ListarTratamiento() {
    const roles = userRoles.split(',');
    const hasAccess = roles.includes('Admin') || roles.includes('Staff') || roles.includes('Doctor');

    if (roles.includes('Doctor')) {
        document.getElementById("btnNuevoTratamiento").style.display = "none";
    }

    objTratamiento = {
        url: "Tratamiento/ListarTratamiento",
        cabeceras: ["Id Tratamiento", "Nombre", "Descripcion", "Fecha", "Costo"],
        propiedades: ["idTratamiento", "nombreCompletoPaciente","descripcion", "fecha", "costo"],
        editar: hasAccess,
        eliminar: hasAccess,
        propiedadID: "idTratamiento"
    }


    if (roles.includes('Doctor')) {
        objTratamiento.url = "Tratamiento/ListarTratamientoMedico";
    }

    pintar(objTratamiento);
}

function ListarPacientes() {
    const roles = userRoles.split(','); 
    let url = "Paciente/ListarPaciente"; 

    if (roles.includes('Doctor') ) {
        url = "Paciente/ListarPacientesAsignados"; 
    }

    fetchGet(url, "json", function (data) {
        let selectPaciente = document.getElementById("idPaciente");
        selectPaciente.innerHTML = ""; 
        data.forEach((paciente) => {
            let option = document.createElement("option");
            option.value = paciente.idPaciente;
            option.text = paciente.nombre + " " + paciente.apellido;
            selectPaciente.appendChild(option);
        });
    });
}

function LimpiarTratamiento() {
    LimpiarDatos("frmTratamiento");
    ListarTratamiento();
}

function ValidarFormulario() {
    let form = document.getElementById("frmTratamiento");
    let isValid = true;

    // Validar campos de texto
    let camposTexto = ['descripcion', 'fecha', 'costo'];
    camposTexto.forEach(function (campo) {
        let input = form.querySelector('#' + campo);
        if (input.value.trim() === '') {
            input.classList.add('is-invalid');
            isValid = false;
        } else {
            input.classList.remove('is-invalid');
        }
    });

    // Validar el campo de selección (Paciente)
    let selectPaciente = form.querySelector('#idPaciente');
    if (selectPaciente.value === '') {
        selectPaciente.classList.add('is-invalid');
        isValid = false;
    } else {
        selectPaciente.classList.remove('is-invalid');
    }

    // Validar el campo de fecha
    let fechaInput = form.querySelector('#fecha');
    if (fechaInput.value === '') {
        fechaInput.classList.add('is-invalid');
        isValid = false;
    } else {
        fechaInput.classList.remove('is-invalid');
    }

    // Validar el campo de costo (permite decimales)
    let costoInput = form.querySelector('#costo');
    let costoValue = parseFloat(costoInput.value.replace(',', '.')); // Acepta tanto ',' como '.' como separador decimal
    if (isNaN(costoValue) || costoValue <= 0) {
        costoInput.classList.add('is-invalid');
        isValid = false;
    } else {
        costoInput.classList.remove('is-invalid');
    }


    return isValid;
}

function GuardarTratamiento() {

    if (!ValidarFormulario()) {
        console.log("Formulario inválido. Revise los campos.");
        return;
    }
    let form = document.getElementById("frmTratamiento");
    let frm = new FormData(form);
    fetchPost("Tratamiento/GuardarTratamiento", "text", frm, function (res) {
        Exito("Registro guardado con éxito");
        LimpiarTratamiento();
        ListarTratamiento();

        var modalElement = document.getElementById('modalTratamiento');
        var myModal = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
        if (myModal) myModal.hide();
    });
}
function MostrarModal() {
    LimpiarDatos("frmTratamiento");
    var myModal = new bootstrap.Modal(document.getElementById('modalTratamiento'));
    myModal.show();
}

function Editar(id) {
    fetchGet("Tratamiento/RecuperarTratamiento/?idTratamiento=" + id, "json", function (data) {
        setN("idTratamiento", data.idTratamiento);
        setN("idPaciente", data.idPaciente);
        setN("descripcion", data.descripcion);
        setN("fecha", data.fecha ? new Date(data.fecha).toISOString().split('T')[0] : '');
        setN("costo", data.costo);

        // Show the modal
        var myModal = new bootstrap.Modal(document.getElementById('modalTratamiento'));
        myModal.show();
    });
}

function Eliminar(id) {
    fetchGet("Tratamiento/RecuperarTratamiento/?idTratamiento=" + id, "json", function (data) {
        fetchGet("Paciente/RecuperarPaciente/?idPaciente=" + data.idPaciente, "json", function (res) {
            const fecha = new Date(data.fecha);
            const fechaFormateada = `${fecha.getDate()}/${fecha.getMonth() + 1}/${fecha.getFullYear()}`;

            Confirmar(
                undefined, 
                `¿Desea eliminar el tratamiento de ${res.nombre} ${res.apellido} con fecha ${fechaFormateada}?`, 
                function () {
                    fetchGet("Tratamiento/EliminarTratamiento/?idTratamiento=" + id, "text", function (r) {
                        Exito("Tratamiento eliminado con éxito"); 
                        ListarTratamiento(); 
                    });
                }
            );
        });
    });
}