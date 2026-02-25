using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActividadApp.Models
{
    public class Rol
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Descripcion { get; set; } = string.Empty;

        // Relaciones

        public virtual List<Usuario> Usuarios { get; set; }
    }
}
