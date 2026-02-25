using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActividadApp.Models;

public class Accion
{
    [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int TipoAccionId { get; set; }
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Required] 
    public int ProcesoId { get; set;} 

    [Required]
    public int ResponsableId { get; set;}

    [Required]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    public int UsuarioId { get; set; }

    [Required]
    public int OrigenId { get; set; }

    [Required]
    public int EntidadId { get; set; }

    [Required]
    public int SistemaGestionId { get; set; }

    [Required]
    public int OrganizacionId { get; set; }

    [Required]
    public int SitioId { get;set; }

    [Required]
    public int EstadoId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt {  get; set; }
    public DateTime? UltimaNotificacion { get; set; }

    // Relaciones

    public virtual Proceso Proceso { get; set; }
    public virtual Usuario Usuario { get; set; }
    public virtual Solucion Solucion { get; set; }
    public virtual ConfirmacionPlanAccion ConfirmacionPlanAccion { get; set; }
    public virtual Eficacia Eficacia { get; set; }
    public virtual List<SeguimientoSgi> SeguimientosSgi { get; set; }

    public virtual Estado Estado { get; set; }
}

