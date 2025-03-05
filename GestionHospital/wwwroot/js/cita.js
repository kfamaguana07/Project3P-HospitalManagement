window.onload = function () {
    ListarCitas();
    const roles = userRoles.split(',');
    const hasAccess = roles.includes('Admin') || roles.includes('Staff') || roles.includes('Doctor');
    if (hasAccess) {
        ListarPacientes();
        ListarMedicos();
    } else {
        document.getElementById("btnNuevoCita").style.display = "none";
    }
};

let objCita;

async function ListarCitas() {
    const roles = userRoles.split(',');
    const hasAccess = roles.includes('Admin') || roles.includes('Staff') || roles.includes('Doctor');

    objCita = {
        url: "Cita/ListarCitas",
        cabeceras: ["Id Cita", "Paciente", "Médico", "Fecha y Hora", "Estado"],
        propiedades: ["idCita", "nombreCompletoPaciente", "nombreCompletoMedico", "fechaHora", "estado"],
        editar: hasAccess,  
        eliminar: hasAccess,  
        propiedadID: "idCita"
    };

    // Si es un Doctor, solo se muestran las citas del médico asignado
    if (roles.includes('Doctor')) {
        objCita.url = "Cita/ListarCitasMedico";
        objCita.cabeceras = ["Id Cita", "Paciente", "Fecha y Hora", "Estado"];
        objCita.propiedades = ["idCita", "nombreCompletoPaciente", "fechaHora", "estado"];
    }

    if (roles.includes('Patient')) {
        objCita.url = "Cita/ListarCitasPaciente";
        objCita.cabeceras = ["Id Cita", "Médico", "Fecha y Hora", "Estado"];
        objCita.propiedades = ["idCita", "nombreCompletoMedico", "fechaHora", "estado"];
    }

    pintar(objCita);
}

function ListarPacientes() {
    fetchGet("Paciente/ListarPaciente", "json", function (data) {
        let selectPaciente = document.getElementById("idPaciente");
        data.forEach((paciente) => {
            let option = document.createElement("option");
            option.value = paciente.idPaciente;
            option.text = paciente.nombre + " " + paciente.apellido;
            selectPaciente.appendChild(option);
        });
    });
}

function ListarMedicos() {
    const roles = userRoles.split(',');
    const selectMedico = document.getElementById("idMedico");

    // Si el usuario es Doctor, solo mostramos su información
    if (roles.includes('Doctor')) {
        selectMedico.innerHTML = ""; 

        fetchGet("Medico/ObtenerIdMedicoActual", "json", function (idMedico) {
            fetchGet("Medico/RecuperarMedico?idMedico=" + idMedico, "json", function (medico) {
                let option = document.createElement("option");
                option.value = medico.idMedico;
                option.text = medico.nombre + " " + medico.apellido;
                selectMedico.appendChild(option);
            });
        });
    } else {
        // Si no es Doctor, listar todos los médicos
        fetchGet("Medico/ListarMedico", "json", function (data) {
            selectMedico.innerHTML = ""; 
            data.forEach((medico) => {
                let option = document.createElement("option");
                option.value = medico.idMedico;
                option.text = medico.nombre + " " + medico.apellido;
                selectMedico.appendChild(option);
            });
        });
    }
}


    function LimpiarCita() {
        LimpiarDatos("frmCita");
        let form = document.getElementById("frmCita");
        let inputs = form.querySelectorAll('.form-control');
        inputs.forEach(input => input.classList.remove('is-invalid'));
    }

    function MostrarModal() {
        LimpiarCita();
        var myModal = new bootstrap.Modal(document.getElementById('modalCita'));
        myModal.show();
    }

    function ValidarFormularioCita() {
        let form = document.getElementById("frmCita");
        let isValid = true;

        let camposRequeridos = ['idPaciente', 'idMedico', 'fechaHora', 'estado'];
        camposRequeridos.forEach(campo => {
            let input = form.querySelector('#' + campo);
            if (!input.value.trim()) {
                input.classList.add('is-invalid');
                isValid = false;
            } else {
                input.classList.remove('is-invalid');
            }
        });

        return isValid;
    }

    function GuardarCita() {
        if (!ValidarFormularioCita()) {
            console.log("Formulario inválido. Revise los campos.");
            return;
        }

        let form = document.getElementById("frmCita");
        let frm = new FormData(form);
        fetchPost("Cita/GuardarCita", "text", frm, function (res) {
            LimpiarCita();
            Exito("Cita guardada con éxito");
            ListarCitas();

            var modalElement = document.getElementById('modalCita');
            var myModal = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
            if (myModal) myModal.hide();
        });
    }

    function Editar(id) {
        fetchGet("Cita/RecuperarCita?idCita=" + id, "json", function (data) {
            setN("idCita", data.idCita);
            setN("idPaciente", data.idPaciente);
            setN("idMedico", data.idMedico);
            setN("fechaHora", data.fechaHora);
            setN("estado", data.estado);

            let form = document.getElementById("frmCita");
            let inputs = form.querySelectorAll('.form-control');
            inputs.forEach(input => input.classList.remove('is-invalid'));

            var myModal = new bootstrap.Modal(document.getElementById('modalCita'));
            myModal.show();
        });
    }

    function Eliminar(id) {
        fetchGet("Cita/RecuperarCita?idCita=" + id, "json", function (data) {
            fetchGet("Paciente/RecuperarPaciente?idPaciente=" + data.idPaciente, "json", function (res) {

                const fecha = new Date(data.fechaHora);
                const fechaFormateada = `${fecha.getDate()}/${fecha.getMonth() + 1}/${fecha.getFullYear()} en el horario de las: ${fecha.getHours()}:${fecha.getMinutes().toString().padStart(2, '0')}`;

                Confirmar(undefined,
                    "¿Desea eliminar la cita de " + res.nombre + " " + res.apellido + " del día " + fechaFormateada + "?",
                    function () {
                        fetchGet("Cita/EliminarCita?idCita=" + id, "text", function (r) {
                            Exito("Cita eliminada con éxito");
                            ListarCitas();
                        });
                    });
            });
        });
    }
