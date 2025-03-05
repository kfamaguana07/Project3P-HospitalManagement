function get(idControl) {
    return document.getElementById(idControl).value;
}

function set(nameControl, valor) {
    document.getElementByName(nameControl)[0].value = valor;
}

function setN(idControl, valor) {
    document.getElementById(idControl).value = valor;
}
function getN(idControl) {
    return document.getElementById(idControl).value;
}

function LimpiarDatos(idFormulario) {
    let elementsName = document.querySelectorAll('#' + idFormulario + " [name]");

    elementsName.forEach(element => {
        element.value = "";
    });
}

async function fetchGet(url, tipoRespuesta, callback) {
    try {
        let urlCompleta = window.location.protocol + "//" + window.location.host + "/" + url;
        let res = await fetch(urlCompleta)

        if (tipoRespuesta == "json")
            res = await res.json();
        else if (tipoRespuesta == "text")
            res = await res.text();

        callback(res)

    } catch (e) {
        alert("ERROR! Ocurre un problema.");
    }
}

async function fetchPost(url, tipoRespuesta, frm, callback) {
    try {
        let urlCompleta = window.location.protocol + "//" + window.location.host + "/" + url;

        let res = await fetch(urlCompleta, {
            method: "POST",
            body: frm
        });

        if (tipoRespuesta == "json")
            res = await res.json();
        else if (tipoRespuesta == "text")
            res = await res.text();

        callback(res)

    } catch (e){
        alert("Ocurre un problema en POST.");
    }
}

let objConfiguracionGlobal;

//{url: "", cabeceras:[], propiedades:[]}
function pintar(objConfiguracion) {
    objConfiguracionGlobal = objConfiguracion;

    if (objConfiguracionGlobal.divContenedorTabla == undefined)
        objConfiguracionGlobal.divContenedorTabla = "divContenedorTabla";
    if (objConfiguracionGlobal.editar == undefined)
        objConfiguracionGlobal.editar = false;
    if (objConfiguracionGlobal.eliminar == undefined)
        objConfiguracionGlobal.eliminar = false;
    if (objConfiguracionGlobal.propiedadID == undefined)
        objConfiguracionGlobal.propiedadID = "";

    fetchGet(objConfiguracion.url, "json", function (res) {
        let contenido = "";

        contenido += "<div id='divContenedor'>";

        contenido += generarTabla(res); 

        contenido += "</div>";

        document.getElementById(objConfiguracionGlobal.divContenedorTabla).innerHTML = contenido;

        new DataTable('#myTable');
    });
}

function generarTabla(res) {
    let contenido = "";

    // ["Id tipo Medicamento", "Nombre", "Descripcion", "Stock"]
    let cabeceras = objConfiguracionGlobal.cabeceras;
    let propiedades = objConfiguracionGlobal.propiedades;

    contenido += "<table id='myTable' class='table table-striped'>";
    contenido += "<thead>";
    contenido += "<tr>";

    for (let i = 0; i < cabeceras.length; i++) {
        contenido += "<th>" + cabeceras[i] + "</th>";
    }

    if (objConfiguracionGlobal.editar === true || objConfiguracionGlobal.eliminar === true) {
        contenido += "<th>Operaciones</th>";
    }

    contenido += "</tr>";
    contenido += "</thead>";

    let nroRegistros = res.length;
    let obj;
    let propiedadActual;

    contenido += "<tbody>";

    for (let i = 0; i < nroRegistros; i++) {
        obj = res[i];
        contenido += "<tr>";
        for (let j = 0; j < propiedades.length; j++) {
            propiedadActual = propiedades[j];
            contenido += "<td>" + obj[propiedadActual] + "</td>";
        }

        if (objConfiguracionGlobal.editar === true || objConfiguracionGlobal.eliminar === true) {
            let propiedadID = objConfiguracionGlobal.propiedadID;
            contenido += "<td>";
            if (objConfiguracionGlobal.editar === true) {
                contenido += `<i onclick="Editar(${obj[propiedadID]})" class="btn btn-primary">
                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16">
                                    <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"/>
                                    <path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z"/>
                                </svg>
                            </i>`;
            }
            contenido += " ";
            if (objConfiguracionGlobal.eliminar === true) {
                contenido += `<i onclick="Eliminar(${obj[propiedadID]})" class="btn btn-danger">
                          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
                          <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z"/>
                          <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z"/>
                        </svg></i>`;
            }
            contenido += "</td>";
        }

        contenido += "</tr>";
    }

    contenido += "</tbody>";
    contenido += "</table>";

    return contenido;
}


function pintarDatosGenerico(objDatos, propiedades, contenedorId) {
    let contenido = "<div class='infoGenerica'>";

    propiedades.forEach(prop => {
        if (objDatos[prop]) {
            contenido += "<p><strong>" + prop.charAt(0).toUpperCase() + prop.slice(1) + ":</strong> " + objDatos[prop] + "</p>";
        }
    });

    contenido += "</div>";
    document.getElementById(contenedorId).innerHTML = contenido;
}
function Confirmar(titulo ="Eliminar Registro", texto, callback) {
    return Swal.fire({
        title: titulo,
        text: texto,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, Eliminar Registro"
    }).then((result) => {
        if (result.isConfirmed) {
            callback();
        }
    });
}

function Exito(titulo = "Operacion realizado con Exito") {
    Swal.fire({
        title: titulo ,
        icon: "success",
        showConfirmButton: false,
        timer: 1500
    });
}

function Fallo(titulo = "Ha ocurrido un problema con la Operacion",
                mensaje = "Por favor, inténtelo de nuevo más tarde.") {
    Swal.fire({
        title: titulo,
        text: mensaje,
        icon: "error",
        draggable: true
    });
    setTimeout(() => {
        swalInstance.close();
    }, 5000);
}

function AlertaCopiar(textoACopiar = "Copiado al portapapeles.",
    _title = "Asegurate de Guardar tu informacion",
    _text= "Puedes copiar el texto o simplemente cerrar esta ventana.") {

    Swal.fire({
        title: _title,
        text: _text,
        icon: "info",
        showCancelButton: true, // Mostrar botón de cancelar (Copiar)
        confirmButtonText: "OK", // Texto del botón de confirmación
        cancelButtonText: "Copiar", // Texto del botón de cancelación (Copiar)
        reverseButtons: true, // Invertir el orden de los botones (Copiar a la izquierda)
    }).then((result) => {
        if (result.dismiss === Swal.DismissReason.cancel) {
            // Si el usuario hace clic en "Copiar"
            navigator.clipboard.writeText(textoACopiar) // Copiar al portapapeles
                .then(() => {
                    Swal.fire({
                        title: "¡Copiado!",
                        text: "Informacion en el portapapeles.",
                        icon: "success",
                        timer: 1500, // Cerrar automáticamente después de 1.5 segundos
                        showConfirmButton: false, // No mostrar el botón "OK"
                    });
                })
                .catch(() => {
                    Swal.fire({
                        title: "Error",
                        text: "No se pudo copiar el texto al portapapeles.",
                        icon: "error",
                    });
                });
        }
    });
}