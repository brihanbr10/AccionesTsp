using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActividadApp.Models
{
    public class SeguimientoActividad
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ActividadId { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [Required]
        public string Comentario { get; set; } = string.Empty;

        // Relaciones
        public virtual Actividad Actividad { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
