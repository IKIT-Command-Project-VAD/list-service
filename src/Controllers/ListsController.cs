using ListService.Domain.Entities;
using ListService.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ListService.Controllers;

public class ListsController(ListsDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShoppingList>>> GetShoppingLists()
    {
        return await context.ShoppingLists
            .Include(sl => sl.Items)
            .Include(sl => sl.ShareLinks)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShoppingList>> GetShoppingList(Guid id)
    {
        var shoppingList = await context.ShoppingLists
            .Include(sl => sl.Items)
            .Include(sl => sl.ShareLinks)
            .FirstOrDefaultAsync(sl => sl.ListId == id);

        if (shoppingList == null)
            return NotFound();

        return shoppingList;
    }

    [HttpPost]
    public async Task<ActionResult<ShoppingList>> CreateShoppingList(ShoppingList shoppingList)
    {
        shoppingList.ListId = Guid.NewGuid();
        shoppingList.CreatedAt = DateTimeOffset.UtcNow;
        shoppingList.UpdatedAt = DateTimeOffset.UtcNow;
        shoppingList.Version = 1;
        shoppingList.IsDeleted = false;

        context.ShoppingLists.Add(shoppingList);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetShoppingList), new { id = shoppingList.ListId }, shoppingList);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShoppingList(Guid id, ShoppingList shoppingList)
    {
        if (id != shoppingList.ListId)
            return BadRequest();

        shoppingList.UpdatedAt = DateTimeOffset.UtcNow;
        shoppingList.Version++;
        
        context.Entry(shoppingList).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ShoppingListExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShoppingList(Guid id)
    {
        var shoppingList = await context.ShoppingLists.FindAsync(id);
        if (shoppingList == null)
            return NotFound();

        // Soft delete
        shoppingList.IsDeleted = true;
        shoppingList.UpdatedAt = DateTimeOffset.UtcNow;
        await context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ShoppingListExists(Guid id)
    {
        return await context.ShoppingLists.AnyAsync(e => e.ListId == id);
    }
}