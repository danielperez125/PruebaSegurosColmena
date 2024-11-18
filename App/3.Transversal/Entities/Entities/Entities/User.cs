using Entities.Colmena.Base;
using System.ComponentModel;

namespace Entities.Colmena.NuGet
{
    [Description("Tabla principal de usuarios del sistema")]
    public class User : Audit
    {
        /// <summary>
        /// ID principal de la tabla
        /// </summary>
        public int UserId { get; set; }

        /// <summary>   
        /// ID principal de la tabla de Tipos de Usuario
        /// </summary>
        public int UserTypeId { get; set; }

        /// <summary>
        /// Tipo de usuario: (Super Admin, Admin, Jugador, Árbitro...)
        /// Tomado de la Entidad UserType
        /// </summary>
        public Dictionary<string, object?>? UserType { get; set; }

        /// <summary>   
        /// Nombres del usuario
        /// </summary>
        public string? Names { get; set; }

        /// <summary>   
        /// Apellidos del usuario
        /// </summary>
        public string? Lastnames { get; set; }

        /// <summary>   
        /// Correo electrónico del usuario
        /// </summary>
        public string? Email { get; set; }

        /// <summary>   
        /// Contraseña de acceso del usuario
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Entorno habilitado para el usuario
        /// </summary>
        public string? Environment { get; set; }
    }
}
