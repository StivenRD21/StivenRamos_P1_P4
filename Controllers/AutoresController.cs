using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite; 
using Dapper;           
using EditorialApi.Models;

namespace EditorialApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly string _connectionString;

        public AutoresController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            InicializarBaseDeDatos();
        }

        private void InicializarBaseDeDatos()
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                CREATE TABLE IF NOT EXISTS Autores (
                    IdAutor INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombres TEXT NOT NULL,
                    Nacionalidad TEXT NOT NULL,
                    FechaNacimiento TEXT NOT NULL,
                    Sueldo REAL NOT NULL
                )";
            connection.Execute(sql);
        }

        [HttpGet]
        public IActionResult Get()
        {
            using var connection = new SqliteConnection(_connectionString);
            var autores = connection.Query<Autor>("SELECT * FROM Autores").ToList();
            return Ok(autores);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            var autor = connection.QueryFirstOrDefault<Autor>(
                "SELECT * FROM Autores WHERE IdAutor = @Id", new { Id = id });

            if (autor == null)
            {
                return NotFound();
            }
            
            return Ok(autor);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Autor autor)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                INSERT INTO Autores (Nombres, Nacionalidad, FechaNacimiento, Sueldo) 
                VALUES (@Nombres, @Nacionalidad, @FechaNacimiento, @Sueldo);
                SELECT last_insert_rowid();";
            
            var idGenerado = connection.ExecuteScalar<int>(sql, autor);
            autor.IdAutor = idGenerado;

            return CreatedAtAction(nameof(Get), new { id = autor.IdAutor }, autor);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Autor autor)
        {
            using var connection = new SqliteConnection(_connectionString);
            var sql = @"
                UPDATE Autores 
                SET Nombres = @Nombres, Nacionalidad = @Nacionalidad, 
                    FechaNacimiento = @FechaNacimiento, Sueldo = @Sueldo 
                WHERE IdAutor = @Id";
            
            autor.IdAutor = id; 
            var filasAfectadas = connection.Execute(sql, autor);

            if (filasAfectadas == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            var filasAfectadas = connection.Execute(
                "DELETE FROM Autores WHERE IdAutor = @Id", new { Id = id });

            if (filasAfectadas == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}