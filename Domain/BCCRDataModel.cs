using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoApiBCCR.Domain
{
    internal class BCCRDataModel
    {
        public DateTime fechaConsulta { get; set; }
        public string tipoCambioCompra { get; set; }
        public string tipoCambioVenta { get; set; }
    }
}
