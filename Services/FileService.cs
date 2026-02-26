namespace ActividadApp.Services;

public interface IFileService
{
    Task<(string rutaArchivo, string nombreArchivo)> GuardarArchivo(Stream stream, string nombreOriginal, int accionId, int actividadId);
    void EliminarArchivo(string rutaArchivo);
    string ObtenerRutaCompleta(string rutaRelativa);
}

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;
    private const string CarpetaEvidencias = "evidencias";

    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<(string rutaArchivo, string nombreArchivo)> GuardarArchivo(Stream stream, string nombreOriginal, int accionId, int actividadId)
    {
        var carpeta = Path.Combine(_env.WebRootPath, CarpetaEvidencias, $"accion-{accionId}");
        Directory.CreateDirectory(carpeta);

        // Sanitizar extension para evitar path traversal
        var extension = Path.GetExtension(nombreOriginal);
        if (string.IsNullOrEmpty(extension) || extension.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            extension = ".bin";
        }
        var nombreArchivo = $"act-{actividadId}_{DateTime.UtcNow:yyyyMMddHHmmss}{extension}";
        var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

        // Verificar que la ruta final esta dentro de wwwroot
        var rutaNormalizada = Path.GetFullPath(rutaCompleta);
        var webRootNormalizado = Path.GetFullPath(_env.WebRootPath);
        if (!rutaNormalizada.StartsWith(webRootNormalizado, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Ruta de archivo invalida.");
        }

        using var fileStream = new FileStream(rutaCompleta, FileMode.Create);
        await stream.CopyToAsync(fileStream);

        var rutaRelativa = Path.Combine(CarpetaEvidencias, $"accion-{accionId}", nombreArchivo);
        return (rutaRelativa, nombreOriginal);
    }

    public void EliminarArchivo(string rutaArchivo)
    {
        var rutaCompleta = ObtenerRutaCompleta(rutaArchivo);
        if (File.Exists(rutaCompleta))
        {
            File.Delete(rutaCompleta);
        }
    }

    public string ObtenerRutaCompleta(string rutaRelativa)
    {
        return Path.Combine(_env.WebRootPath, rutaRelativa);
    }
}
