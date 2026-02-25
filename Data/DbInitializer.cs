using ActividadApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace ActividadApp.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        await SeedRoles(context);
        await SeedMaestros(context);
        await SeedResponsablesProceso(context);
        await SeedProcesos(context);
        await SeedEstados(context);
    }

    private static async Task SeedRoles(AppDbContext context)
    {
        if (await context.Roles.AnyAsync())
            return;

        context.Roles.AddRange(
            new Rol { Descripcion = "Usuario" },
            new Rol { Descripcion = "Administrador" }
        );

        await context.SaveChangesAsync();
    }

    private static async Task SeedMaestros(AppDbContext context)
    {
        if (await context.Maestros.AnyAsync())
            return;

        var maestros = new List<Maestro>
        {
            // ORGANIZACION
            new() { Descripcion = "TSP", Tipo = TipoMaestro.ORGANIZACION },
            new() { Descripcion = "LDSP", Tipo = TipoMaestro.ORGANIZACION },
            new() { Descripcion = "TSP-LDSP", Tipo = TipoMaestro.ORGANIZACION },
            new() { Descripcion = "TSP-LDSP-ISP", Tipo = TipoMaestro.ORGANIZACION },
            new() { Descripcion = "TSP ECU", Tipo = TipoMaestro.ORGANIZACION },
            new() { Descripcion = "TSP PER", Tipo = TipoMaestro.ORGANIZACION },
            new() { Descripcion = "TSP VEN", Tipo = TipoMaestro.ORGANIZACION },

            // SITIO
            new() { Descripcion = "Oficina Principal", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Barranquilla", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Bogota", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Cali", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Medellin", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Pereira", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Bucaramanga", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Cartagena", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Manizales", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Buenaventura", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Santa Marta", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Albania", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Ibague", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Tocancipa", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Ipiales", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Valledupar", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "La Loma", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Cerrejon", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Drummond", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Tenjo", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Puerto Tejada", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Cucuta", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "La Dorada", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Paraguachon", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Barbosa", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Guarero", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "E Guayaquil", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "E Tulcan", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "E Quito", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Peru", Tipo = TipoMaestro.SITIO },
            new() { Descripcion = "Venezuela", Tipo = TipoMaestro.SITIO },

            // CARGO
            new() { Descripcion = "Analista Administrativo", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Analista Base De Datos", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Analista De Compras", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Analista De Control Operativo", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Analista De Equipos Propios (Cerrados)", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Analista De Facturacion Nacional", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Analista De Gestion Humana", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Analista De Indicadores y Servicio Al Cliente", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Analista De Seguridad Vial", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Analista De Sistemas De Gestion", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Analista De Sistemas De Gestion MAA", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Analista De Trafico", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Analista Juridico", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Analista SSTA", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Auxiliar Administrativo", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Auxiliar De Activacion De Proveedores De Transporte", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Auxiliar De Cumplidos y Facturacion", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Auxiliar De Facturacion", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Auxiliar De Inventarios Nacional", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Auxiliar De Mantenimiento", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Auxiliar De Soporte Tecnico", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Auxiliar Operativo", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Coordinador Administrativo", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Coordinador Administrativo Comercial", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Coordinador Comercial", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Coordinador De Desarrollo SI", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Coordinador De Despachos", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Coordinador De Flota", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Coordinador De Gestion Humana", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Coordinador De Implementacion OTM", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Coordinador De Infraestructura", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Coordinador De Infraestructura y Soporte Tecnico", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Coordinador De Inventarios", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Coordinador De Mejora Continua", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Coordinador De PSF", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Coordinador De Trafico", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Coordinador De Transporte", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Coordinador Nacional De Inventario", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Coordinador Operativo", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Coordinador SSTA", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Gerente Comercial", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Gerente De Agencia", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Gerente De Asuntos Corporativos", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Gerente De Calidad Y Trafico", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Gerente De Costos Y Auditoria", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Gerente De Datos Maestros", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Gerente De Planeacion De La Operacion Y Flota", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Gerente De Planeacion y Presupuesto", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Gerente De Proyectos y Mejora Continua", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Gerente De Tecnologia E Innovacion", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Gerente General", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Gerente Nacional De Operaciones", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Gerente Operativo", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Ingeniero De Proyectos", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Jefe De Flota", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Jefe De Mejora Continua", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Jefe De Negocios", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Jefe De Seguridad Nacional", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Jefe De Sistemas De Gestion", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Jefe Operativo", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Presidente", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Supervisor De Equipos", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Supervisor De Operaciones", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Supervisor De Trafico y SAC", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Supervisor Logistico", Tipo = TipoMaestro.CARGO },
            new() { Descripcion = "Vicepresidente De Operaciones", Tipo = TipoMaestro.CARGO },

            // ENTIDAD
            new() { Descripcion = "SGI", Tipo = TipoMaestro.ENTIDAD },
            new() { Descripcion = "ICONTEC", Tipo = TipoMaestro.ENTIDAD },
            new() { Descripcion = "CCS", Tipo = TipoMaestro.ENTIDAD },
            new() { Descripcion = "BASC", Tipo = TipoMaestro.ENTIDAD },
            new() { Descripcion = "SMETA", Tipo = TipoMaestro.ENTIDAD },
            new() { Descripcion = "Cliente", Tipo = TipoMaestro.ENTIDAD },
            new() { Descripcion = "Proveedor", Tipo = TipoMaestro.ENTIDAD },
            new() { Descripcion = "Operativa", Tipo = TipoMaestro.ENTIDAD },
            new() { Descripcion = "COCOLA", Tipo = TipoMaestro.ENTIDAD },
            new() { Descripcion = "COPASST", Tipo = TipoMaestro.ENTIDAD },
            new() { Descripcion = "CSV", Tipo = TipoMaestro.ENTIDAD },
            new() { Descripcion = "Rev Interna", Tipo = TipoMaestro.ENTIDAD },

            // ORIGEN
            new() { Descripcion = "Hallazgo de Auditoria Interna", Tipo = TipoMaestro.ORIGEN },
            new() { Descripcion = "Hallazgo de Auditoria Externa", Tipo = TipoMaestro.ORIGEN },
            new() { Descripcion = "Hallazgo de Auditoria Cliente", Tipo = TipoMaestro.ORIGEN },
            new() { Descripcion = "Revision de proceso", Tipo = TipoMaestro.ORIGEN },
            new() { Descripcion = "Incumplimiento de Indicadores", Tipo = TipoMaestro.ORIGEN },
            new() { Descripcion = "Gestion del Cambio", Tipo = TipoMaestro.ORIGEN },
            new() { Descripcion = "Condicion y/o Acto a Riesgo", Tipo = TipoMaestro.ORIGEN },
            new() { Descripcion = "Inspecciones de Seguridad", Tipo = TipoMaestro.ORIGEN },
            new() { Descripcion = "Incumplimiento Requisito Legal", Tipo = TipoMaestro.ORIGEN },
            new() { Descripcion = "Inspecciones Preoperacionales", Tipo = TipoMaestro.ORIGEN },
            new() { Descripcion = "Accidentalidad", Tipo = TipoMaestro.ORIGEN },
            new() { Descripcion = "Incumplimiento Procedimientos", Tipo = TipoMaestro.ORIGEN },

            // SISGESTION
            new() { Descripcion = "SGI", Tipo = TipoMaestro.SISGESTION },
            new() { Descripcion = "SGC", Tipo = TipoMaestro.SISGESTION },
            new() { Descripcion = "SGSST", Tipo = TipoMaestro.SISGESTION },
            new() { Descripcion = "SGA", Tipo = TipoMaestro.SISGESTION },
            new() { Descripcion = "SGCS", Tipo = TipoMaestro.SISGESTION },
            new() { Descripcion = "RUC", Tipo = TipoMaestro.SISGESTION },
            new() { Descripcion = "RS", Tipo = TipoMaestro.SISGESTION },
            new() { Descripcion = "PESV", Tipo = TipoMaestro.SISGESTION },
            new() { Descripcion = "SGSST-RUC", Tipo = TipoMaestro.SISGESTION },
            new() { Descripcion = "SGCS-PESV", Tipo = TipoMaestro.SISGESTION },
        };

        context.Maestros.AddRange(maestros);
        await context.SaveChangesAsync();
    }

    private static async Task SeedResponsablesProceso(AppDbContext context)
    {
        if (await context.ResponsablesProceso.AnyAsync())
            return;

        context.ResponsablesProceso.AddRange(
            new ResponsableProceso { Nombre = "Cano Montoya Carlos Mario", Email = "cano.montoya@empresa.com" },
            new ResponsableProceso { Nombre = "Baez Diaz Tania", Email = "baez.diaz@empresa.com" },
            new ResponsableProceso { Nombre = "Guette Hernandez Isaac", Email = "guette.hernandez@empresa.com" },
            new ResponsableProceso { Nombre = "Mendoza Ojeda Dairo", Email = "mendoza.ojeda@empresa.com" },
            new ResponsableProceso { Nombre = "Chica Jadys Maria", Email = "chica.jadys@empresa.com" },
            new ResponsableProceso { Nombre = "Rojas Bustamante Jose", Email = "rojas.bustamante@empresa.com" },
            new ResponsableProceso { Nombre = "Zapata Lavado Clara", Email = "zapata.lavado@empresa.com" },
            new ResponsableProceso { Nombre = "Fajardo Tatis Melissa", Email = "fajardo.tatis@empresa.com" },
            new ResponsableProceso { Nombre = "Polo Leiva Alberto", Email = "polo.leiva@empresa.com" },
            new ResponsableProceso { Nombre = "Mattos Gaitan Maicol", Email = "mattos.gaitan@empresa.com" }
        );

        await context.SaveChangesAsync();
    }

    private static async Task SeedProcesos(AppDbContext context)
    {
        if (await context.Procesos.AnyAsync())
            return;

        var responsables = await context.ResponsablesProceso.ToListAsync();

        int IdDe(string nombre) => responsables.First(r => r.Nombre == nombre).Id;

        context.Procesos.AddRange(
            new Proceso { Descripcion = "Gestion Gerencial", ResponsableId = IdDe("Cano Montoya Carlos Mario") },
            new Proceso { Descripcion = "Gestion Comercial & Proyectos", ResponsableId = IdDe("Cano Montoya Carlos Mario") },
            new Proceso { Descripcion = "Planeacion de la Operacion", ResponsableId = IdDe("Baez Diaz Tania") },
            new Proceso { Descripcion = "Gestion Almacenamiento", ResponsableId = IdDe("Guette Hernandez Isaac") },
            new Proceso { Descripcion = "Cargue y Despacho", ResponsableId = IdDe("Baez Diaz Tania") },
            new Proceso { Descripcion = "Transito y Seguimiento", ResponsableId = IdDe("Mendoza Ojeda Dairo") },
            new Proceso { Descripcion = "Entrega, Cumplido y Facturacion", ResponsableId = IdDe("Baez Diaz Tania") },
            new Proceso { Descripcion = "Seguridad, Salud en el Trabajo y Ambiente", ResponsableId = IdDe("Chica Jadys Maria") },
            new Proceso { Descripcion = "Proteccion y Seguridad Fisica", ResponsableId = IdDe("Rojas Bustamante Jose") },
            new Proceso { Descripcion = "Compras y Contratacion de Servicios", ResponsableId = IdDe("Zapata Lavado Clara") },
            new Proceso { Descripcion = "Gestion Humana", ResponsableId = IdDe("Fajardo Tatis Melissa") },
            new Proceso { Descripcion = "Administracion de los Sistemas de Gestion", ResponsableId = IdDe("Chica Jadys Maria") },
            new Proceso { Descripcion = "Gestion de Tecnologia e Informacion", ResponsableId = IdDe("Polo Leiva Alberto") },
            new Proceso { Descripcion = "Gestion Juridica", ResponsableId = IdDe("Mattos Gaitan Maicol") },
            new Proceso { Descripcion = "Gestion Financiera", ResponsableId = IdDe("Zapata Lavado Clara") }
        );

        await context.SaveChangesAsync();
    }

    private static async Task SeedEstados(AppDbContext context)
    {
        if (await context.Estados.AnyAsync())
            return;


        context.Estados.AddRange(
            new Estado { Descripcion = "Abierta sin Plan" },
            new Estado { Descripcion = "Abierta con Solución" },
            new Estado { Descripcion = "Abierta por Confirmación" },
            new Estado { Descripcion = "Pendiente por Eficacia" },
            new Estado { Descripcion = "Cerrada" }
        );

        await context.SaveChangesAsync();
    }
}
