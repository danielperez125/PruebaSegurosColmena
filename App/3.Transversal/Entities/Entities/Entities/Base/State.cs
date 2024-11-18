using System.ComponentModel;

namespace Entities.Colmena.Base
{
    [Description("Estados de una entidad en el sistema")]
    public class State
    {
        /// <summary>
        /// ID principal de la tabla
        /// </summary>
        public int StateId { get; set; }

        /// <summary>
        /// Nombre del estado
        /// </summary>
        public string? NameState { get; set; }

        /// <summary>
        /// Campo descriptivo que informa el detalle del estado
        /// </summary>
        public string? DetailsState { get; set; }
    }
}
