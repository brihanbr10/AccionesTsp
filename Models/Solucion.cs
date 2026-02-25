using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActividadApp.Models
{
    public class Solucion
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int AccionId { get; set; }

        [Required]
        public string CorreccionPropuesta { get; set; }

        [Required]
        public string AnalisisCausa {  get; set; }

        [Required]
        public int UsuarioInvestigaId { get; set; }

        [Required]
        public DateTime FechaInvestigacion { get; set; }

        [Required]
        public int UsuarioCoordinaId { get; set; }

        [Required]
        public DateTime FechaCompromiso {  get; set; }
        

        // Relaciones
        public virtual List<Actividad> Actividades { get; set; }
        public virtual Usuario UsuarioInvestiga { get; set; }
        public virtual Usuario UsuarioCoordina { get; set; }

    }
}
