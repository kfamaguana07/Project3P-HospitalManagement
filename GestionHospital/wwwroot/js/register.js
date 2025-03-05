function validarContrasena(password) {
    const regexMayuscula = /[A-Z]/;
    const regexNumero = /[0-9]/;
    const regexEspecial = /[!@#$%^&*(),.?":{}|<>]/;

    if (password.length < 8) {
        Fallo("Contraseña inválida", "La contraseña debe tener al menos 8 caracteres.");
        return false;
    }
    if (!regexMayuscula.test(password)) {
        Fallo("Contraseña inválida", "La contraseña debe tener al menos una letra mayúscula.");
        return false;
    }
    if (!regexNumero.test(password)) {
        Fallo("Contraseña inválida", "La contraseña debe tener al menos un número.");
        return false;
    }
    if (!regexEspecial.test(password)) {
        Fallo("Contraseña inválida", "La contraseña debe tener al menos un carácter especial.");
        return false;
    }
    return true;
}


function validarFormulario() {
    const password = document.getElementById('Password').value;
    return validarContrasena(password);
}