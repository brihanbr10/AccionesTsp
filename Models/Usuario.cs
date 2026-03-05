using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActividadApp.Models;


public class Usuario
{
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int OrganizacionId { get; set; }

    [Required] 
    public int Cedula { get; set; }

    [Required]
    public string Nombre { get; set; }

    [Required]
    public int CargoId { get; set; }

    [Required]
    public int AgenciaId { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public int RolId { get; set; }

    public bool Activo { get; set; } = false;


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relaciones
    public virtual Rol Rol { get; set; }
    public virtual List<Accion> Acciones { get ; set; } 
    public virtual Maestro Organizacion { get; set; }
    public virtual Maestro Cargo { get; set; }
    public virtual Maestro Agencia { get; set; }

}
