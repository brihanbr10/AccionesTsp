using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActividadApp.Models
{
    public class Eficacia
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int AccionId { get; set; }

        [Required]
        public int DiasParaVerificar { get; set; } = 90;

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public int ResponsableId { get; set; }

        [Required]
        public string Comentarios { get; set; }

        [Required]
        public bool Eficaz { get; set; }
    }
}
