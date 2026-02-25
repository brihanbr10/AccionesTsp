using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActividadApp.Models
{
    public class Proceso
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Descripcion { get; set; } = null!;

        public int ResponsableId { get; set; }

        // Relaciones

        public virtual ResponsableProceso Responsable { get; set; }
    }
}
