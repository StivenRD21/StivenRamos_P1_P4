using Microsoft.AspNetCore.Mvc;
using EditorialApi.Models; 
using EditorialApi.Services; 

namespace EditorialApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutoresController : ControllerBase
{
    private readonly AutoresServices _autoresService;

    public AutoresController(AutoresServices autoresService)
    {
        _autoresService = autoresService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var autores = await _autoresService.GetAllAsync();
        return Ok(autores);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var autor = await _autoresService.GetByIdAsync(id);
        if (autor == null)
        {
            return NotFound();
        }
        return Ok(autor);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AutorRecord autor)
    {
        var newId = await _autoresService.InsertAsync(autor);
        return CreatedAtAction(nameof(GetById), new { id = newId }, autor with { IdAutor = newId });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AutorRecord autor)
    {
        var updated = await _autoresService.UpdateAsync(id, autor);
        if (!updated)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _autoresService.DeleteAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return NoContent();
    }
}