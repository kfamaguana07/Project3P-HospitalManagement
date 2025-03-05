window.onload = function () {
    const roles = userRoles.split(',');
    const hasAccess = roles.includes('Admin') || roles.includes('Staff');
    if (hasAccess) {
        ListarPacientes();
    }

    ListarFacturacion();

}

let objFacturacion;

async function ListarFacturacion() {
    const roles = userRoles.split(',');
    const hasAccess = roles.includes('Admin') || roles.includes('Staff');

    if (!hasAccess) {
        let btnNuevo = document.getElementById("btnNuevoFacturacion");
        btnNuevo.style.display = "none";
    }

    objFacturacion = {
        url: "Facturacion/ListarFacturacion",
        cabeceras: ["Id Facturacion", "Nombre", "Monto", "Metodo de Pago", "Fecha de Pago"],
        propiedades: ["idFacturacion", "nombreCompletoPaciente", "monto", "metodoPago", "fechaPago"],
        editar: hasAccess,
        eliminar: hasAccess,
        propiedadID: "idFacturacion"
    }

    if (roles.includes('Patient')) {
        objFacturacion.url = "Facturacion/ListarFacturacionPaciente";
        objFacturacion.cabeceras = ["Id Facturacion", "Monto", "Metodo de Pago", "Fecha de Pago"];
        objFacturacion.propiedades = ["idFacturacion", "monto", "metodoPago", "fechaPago"];
    }

    pintar(objFacturacion);
}

function ListarPacientes() {
    const roles = userRoles.split(',');
    let url = "Paciente/ListarPaciente";

    if (roles.includes('Doctor')) {
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

function LimpiarFacturacion() {
    LimpiarDatos("frmFacturacion");
    ListarFacturacion();
}

function ValidarFormulario() {
    let form = document.getElementById("frmFacturacion");
    if (!form) {
        console.error("El formulario no fue encontrado.");
        return false;
    }

    let isValid = true;

    // Validar campo idPaciente (select)
    let idPaciente = form.querySelector('#idPaciente');
    if (!idPaciente) {
        console.error("El campo idPaciente no fue encontrado.");
        return false;
    }
    if (idPaciente.value === '') {
        idPaciente.classList.add('is-invalid');
        isValid = false;
    } else {
        idPaciente.classList.remove('is-invalid');
    }

    // Validar campo monto (input number)
    let monto = form.querySelector('#monto');
    if (!monto) {
        console.error("El campo monto no fue encontrado.");
        return false;
    }
    if (monto.value === '' || parseFloat(monto.value) <= 0) {
        monto.classList.add('is-invalid');
        isValid = false;
    } else {
        monto.classList.remove('is-invalid');
    }

    // Validar campo metodoPago (select)
    let metodoPago = form.querySelector('#metodoPago');
    if (!metodoPago) {
        console.error("El campo metodoPago no fue encontrado.");
        return false;
    }
    if (metodoPago.value === '') {
        metodoPago.classList.add('is-invalid');
        isValid = false;
    } else {
        metodoPago.classList.remove('is-invalid');
    }

    // Validar campo fechaPago (input date)
    let fechaPago = form.querySelector('#fechaPago');
    if (!fechaPago) {
        console.error("El campo fechaPago no fue encontrado.");
        return false;
    }
    if (fechaPago.value === '') {
        fechaPago.classList.add('is-invalid');
        isValid = false;
    } else {
        fechaPago.classList.remove('is-invalid');
    }

    return isValid;
}


function GuardarFacturacion() {
    if (!ValidarFormulario()) {
        return;
    }
    console.log("Guardando Facturacion...");
    let form = document.getElementById("frmFacturacion");
    let frm = new FormData(form);
    fetchPost("Facturacion/GuardarFacturacion", "text", frm, function (res) {
        // Perform other operations first
        Exito("Registro Guardado con Exito");
        ListarFacturacion();

        var myModal = bootstrap.Modal.getInstance(document.getElementById('modalFacturacion'));
        myModal.hide();
    });
}

function MostrarModal() {
    LimpiarDatos("frmFacturacion");
    var myModal = new bootstrap.Modal(document.getElementById('modalFacturacion'));
    myModal.show();
}

function Editar(id) {
    fetchGet("Facturacion/RecuperarFacturacion/?idFacturacion=" + id, "json", function (data) {
        // Establecer valores en los campos del formulario
        setN("idFacturacion", data.idFacturacion);
        setN("idPaciente", data.idPaciente);
        setN("monto", data.monto);
        setN("fechaPago", data.fechaPago ? new Date(data.fechaPago).toISOString().split('T')[0] : '');

        // Establecer el valor del método de pago en el select
        let metodoPagoSelect = document.getElementById("metodoPago");
        if (metodoPagoSelect) {
            // Buscar la opción que coincida con el valor de data.metodoPago
            for (let option of metodoPagoSelect.options) {
                if (option.value === data.metodoPago) {
                    option.selected = true; // Seleccionar la opción correspondiente
                    break;
                }
            }
        }

        // Mostrar el modal
        var myModal = new bootstrap.Modal(document.getElementById('modalFacturacion'));
        myModal.show();
    });
}

function Eliminar(id) {
    fetchGet("Facturacion/RecuperarFacturacion/?idFacturacion=" + id, "json", function (data) {
        fetchGet("Paciente/RecuperarPaciente/?idPaciente=" + data.idPaciente, "json", function (res) {
            const fecha = new Date(data.fecha);
            const fechaFormateada = `${fecha.getDate()}/${fecha.getMonth() + 1}/${fecha.getFullYear()}`;

            Confirmar(
                undefined,
                `¿Desea eliminar la factura de ${res.nombre} ${res.apellido} con monto de  $${data.monto}?`,
                function () {
                    fetchGet("Facturacion/EliminarFacturacion/?idFacturacion=" + id, "text", function (r) {
                        Exito("Factura eliminada con éxito");
                        ListarFacturacion();
                    });
                }
            );
        });
    });
}
