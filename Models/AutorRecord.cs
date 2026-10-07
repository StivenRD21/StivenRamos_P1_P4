namespace EditorialApi.Models;

public record AutorRecord
{
    public int IdAutor { get; init; }
    public string Nombres { get; init; } = string.Empty;
    public string Nacionalidad { get; init; } = string.Empty;
    public DateTime FechaNacimiento { get; init; }
    public decimal Sueldo { get; init; }
}