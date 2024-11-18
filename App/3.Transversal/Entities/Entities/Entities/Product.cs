using Entities.Colmena.Base;
using System.ComponentModel;

namespace Entities.Colmena.NuGet
{
    [Description("Tabla que indica el producto de aseguradora")]
    public class Product : Audit
    {
        /// <summary>
        /// ID principal de la tabla
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// ID principal de la tabla de líneas de los bienes
        /// </summary>
        public int LineId { get; set; }

        /// <summary>   
        /// Línea
        /// Tomado de la Entidad Line
        /// </summary>
        public Dictionary<string, object?>? Line { get; set; }

        /// <summary>   
        /// Nombre del producto
        /// </summary>
        public string? Name { get; set; }

        /// <summary>   
        /// Detalles del bien
        /// </summary>
        public string? Details { get; set; }

        /// <summary>   
        /// Precio base del producto
        /// </summary>
        public decimal? BasePrice { get; set; }
    }
}
