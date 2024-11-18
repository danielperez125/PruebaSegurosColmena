using System.ComponentModel;

namespace Entities.Colmena.Base
{
    [Description("Entidad transversal para los campos de auditoría de cada tabla")]
    public class Audit : State
    {
        /// <summary>
        /// Usuario que crea el registro
        /// </summary>
        public long UserAdd { get; set; }

        /// <summary>
        /// Fecha en la que se crea el registro
        /// </summary>
        public DateTime DateAdd { get; set; }

        /// <summary>
        /// Usuario que modifica el registro
        /// </summary>
        public long? UserMod { get; set; }

        /// <summary>
        /// Fecha en la que se modifica el registro
        /// </summary>
        public DateTime? DateMod { get; set; }
    }
}
