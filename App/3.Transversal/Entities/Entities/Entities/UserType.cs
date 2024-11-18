using Entities.Colmena.Base;
using System.ComponentModel;

namespace Entities.Colmena.NuGet
{
    [Description("Tabla que indica el tipo de usuario (Super Admin, Admin, Jugador, Árbitro...)")]
    public class UserType : Audit
    {
        /// <summary>
        /// ID principal de la tabla
        /// </summary>
        public int UserTypeId { get; set; }

        /// <summary>
        /// Nombre del tipo de usuario
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Detalles del tipo de usuario
        /// </summary>
        public string? Details { get; set; }
    }
}
