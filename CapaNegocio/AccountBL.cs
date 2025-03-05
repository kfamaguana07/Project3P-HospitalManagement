namespace CapaNegocio
{
    public class AccountBL
    {
        // Método para generar la contraseña del paciente
        public static string GenerarContrasena(string nombre, string apellido, DateTime fechaNacimiento)
        {          
            var primeraLetraNombre = nombre.Substring(0, 1).ToUpper();
            var apellidoMinuscula = apellido.ToLower();
            var anoNacimiento = fechaNacimiento.Year.ToString();
            return primeraLetraNombre + apellidoMinuscula + anoNacimiento + "!";
        }
    }
}
