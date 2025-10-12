using ListService.Domain.Entities;
using ListService.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ListService.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CategoriesController(ListsDbContext context) : ControllerBase
{
    [HttpGet]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await context.Categories.ToListAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(Guid id)
    {
        var category = await context.Categories.FindAsync(id);
        if (category == null)
            return NotFound();

        return category;
    }

    [HttpPost]
    public async Task<ActionResult<Category>> CreateCategory(Category category)
    {
        category.CategoryId = Guid.NewGuid();
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, Category category)
    {
        if (id != category.CategoryId)
            return BadRequest();

        context.Entry(category).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CategoryExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var category = await context.Categories.FindAsync(id);
        if (category == null)
            return NotFound();

        context.Categories.Remove(category);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> CategoryExists(Guid id)
    {
        return await context.Categories.AnyAsync(e => e.CategoryId == id);
    }
}