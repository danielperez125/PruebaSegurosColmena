using Entities.Colmena.Base;
using System.ComponentModel;

namespace Entities.Colmena.NuGet
{
    [Description("Tabla que permite la cotización de un producto")]
    public class Quote : Audit
    {
        /// <summary>
        /// ID principal de la tabla
        /// </summary>
        public int QuoteId { get; set; }

        /// <summary>
        /// ID principal de la tabla de productos
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>   
        /// Producto de aseguradora
        /// Tomado de la Entidad Product
        /// </summary>
        public Dictionary<string, object?>? Product { get; set; }

        /// <summary>
        /// ID principal de la tabla de usuarios
        /// </summary>
        public int UserId { get; set; }

        /// <summary>   
        /// Usuario que realiza la cotización
        /// Tomado de la Entidad Product
        /// </summary>
        public Dictionary<string, object?>? User { get; set; }

        /// <summary>
        /// ID principal de la tabla de usuarios
        /// </summary>
        public DateTime QuoteDate { get; set; }

        /// <summary>
        /// Total de la cotización
        /// </summary>
        public decimal Total { get; set; }
    }
}
