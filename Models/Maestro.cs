using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActividadApp.Models
{
    public class Maestro
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Descripcion { get; set; }
        [Required]
        public TipoMaestro Tipo { get; set; }
    }

    public enum TipoMaestro 
    {
        ORIGEN=1, SISGESTION=2, ORGANIZACION=3, ENTIDAD=4, CARGO=5, SITIO=6
    }
}
