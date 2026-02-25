using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActividadApp.Models
{
    public class ConfirmacionPlanAccion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int AccionId { get; set; }

        [Required]
        public string DetallesCumplimiento { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public int ResponsableId { get; set; }

        [Required]
        public bool CumplioPlan { get; set; }

        // Relaciones
        public virtual Usuario Responsable { get; set; }
    }
}
