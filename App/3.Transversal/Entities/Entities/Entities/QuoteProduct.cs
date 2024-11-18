using Entities.Colmena.Base;
using System.ComponentModel;

namespace Entities.Colmena.NuGet
{
    [Description("Tabla que relaciona varios productos a la cotización de un usuario")]
    public class QuoteProduct : Audit
    {
        /// <summary>
        /// ID principal de la tabla
        /// </summary>
        public int QuoteProductId { get; set; }

        /// <summary>
        /// ID principal de la tabla de cotizaciones
        /// </summary>
        public int QuoteId { get; set; }

        /// <summary>   
        /// Tabla de Cotizaciones
        /// Tomado de la Entidad Quote
        /// </summary>
        public Dictionary<string, object?>? Quote { get; set; }

        /// <summary>
        /// Total de la cotización
        /// </summary>
        public decimal Total { get; set; }
    }
}
