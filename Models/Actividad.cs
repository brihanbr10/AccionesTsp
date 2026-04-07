using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActividadApp.Models
{
    public class Actividad
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int SolucionId { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public int ResponsableId { get; set; }

        [Required]
        public string Agencia { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;


        // Seguimiento
        public bool Ejecutada { get; set; } = false;
        public DateTime? FechaEjecucion { get; set; }
        public string? Observaciones { get; set; }
        public string? RutaEvidencia { get; set; }
        public string? NombreArchivoEvidencia { get; set; }

        // Relaciones
        public virtual Usuario Responsable { get; set; }
        public virtual ICollection<SeguimientoActividad> Seguimientos { get; set; } = new List<SeguimientoActividad>();

    }
}