using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class FacturacionBL
    {
        private readonly FacturacionDAL _facturacionDAL;


        public FacturacionBL(FacturacionDAL facturacionDAL)
        {
            _facturacionDAL = facturacionDAL;
        }

        public List<FacturacionViewCLS> ListarFacturacion()
        {
            return _facturacionDAL.ListarFacturacion();
        }

        public List<FacturacionCLS> ListarFacturacionPaciente(int idPaciente)
        {
            return _facturacionDAL.ListarFacturacionPaciente(idPaciente);
        }

        public FacturacionCLS? RecuperarFacturacion(int idFacturacion)
        {
            FacturacionCLS? factura = _facturacionDAL.RecuperarFacturacion(idFacturacion);
            return factura;
        }

        public void EliminarFacturacion(int idFacturacion)
        {
            _facturacionDAL.EliminarFacturacion(idFacturacion);

        }

        public void GuardarFacturacion(FacturacionCLS facturacion)
        {
            _facturacionDAL.GuardarFacturacion(facturacion);
        }
    }
}
