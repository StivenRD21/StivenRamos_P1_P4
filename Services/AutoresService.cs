using Dapper;
using Microsoft.Data.Sqlite;
using EditorialApi.Models; 

namespace EditorialApi.Services; 

public class AutoresServices
{
    private readonly string _connectionString;

    public AutoresServices(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<IEnumerable<AutorRecord>> GetAllAsync() 
    {
        using var connection = new SqliteConnection(_connectionString);
        return await connection.QueryAsync<AutorRecord>("SELECT * FROM Autores");
    }

    public async Task<AutorRecord?> GetByIdAsync(int id) 
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "SELECT * FROM Autores WHERE IdAutor = @Id";
        return await connection.QueryFirstOrDefaultAsync<AutorRecord>(sql, new { Id = id });
    }

    public async Task<int> InsertAsync(AutorRecord autor) 
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"INSERT INTO Autores (Nombres, Nacionalidad, FechaNacimiento, Sueldo) 
                    VALUES (@Nombres, @Nacionalidad, @FechaNacimiento, @Sueldo);
                    SELECT last_insert_rowid();";
        return await connection.ExecuteScalarAsync<int>(sql, autor);
    }

    public async Task<bool> UpdateAsync(int id, AutorRecord autor) 
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = @"UPDATE Autores 
                    SET Nombres = @Nombres, 
                        Nacionalidad = @Nacionalidad, 
                        FechaNacimiento = @FechaNacimiento, 
                        Sueldo = @Sueldo 
                    WHERE IdAutor = @Id";
        
        var rowsAffected = await connection.ExecuteAsync(sql, new { 
            Id = id, 
            autor.Nombres, 
            autor.Nacionalidad, 
            autor.FechaNacimiento, 
            autor.Sueldo 
        });
        
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sql = "DELETE FROM Autores WHERE IdAutor = @Id";
        var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
        
        return rowsAffected > 0;
    }
}