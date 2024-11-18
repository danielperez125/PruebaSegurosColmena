using Entities.Colmena.Base;
using System.ComponentModel;

namespace Entities.Colmena.NuGet
{
    [Description("Tabla que indica el tipo de bien a asegurar")]
    public class Line : Audit
    {
        /// <summary>
        /// ID principal de la tabla
        /// </summary>
        public int LineId { get; set; }

        /// <summary>   
        /// Nombre del bien
        /// </summary>
        public string Name { get; set; } = String.Empty;

        /// <summary>   
        /// Detalles del bien
        /// </summary>
        public string Details { get; set; } = String.Empty;
    }
}
