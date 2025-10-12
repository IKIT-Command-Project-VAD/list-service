using ListService.Domain.Entities;
using ListService.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ListService.Controllers;

public class ListItemsController(ListsDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ListItem>>> GetListItems(Guid listId)
    {
        return await context.ListItems
            .Where(li => li.ListId == listId)
            .Include(li => li.Category)
            .ToListAsync();
    }

    [HttpGet("{itemId}")]
    public async Task<ActionResult<ListItem>> GetListItem(Guid listId, Guid itemId)
    {
        var listItem = await context.ListItems
            .Include(li => li.Category)
            .FirstOrDefaultAsync(li => li.ListId == listId && li.ItemId == itemId);

        if (listItem == null)
            return NotFound();

        return listItem;
    }

    [HttpPost]
    public async Task<ActionResult<ListItem>> CreateListItem(Guid listId, ListItem listItem)
    {
        listItem.ItemId = Guid.NewGuid();
        listItem.ListId = listId;
        listItem.CreatedAt = DateTimeOffset.UtcNow;
        listItem.UpdatedAt = DateTimeOffset.UtcNow;
        listItem.IsDeleted = false;

        context.ListItems.Add(listItem);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetListItem), 
            new { listId = listId, itemId = listItem.ItemId }, listItem);
    }

    [HttpPut("{itemId}")]
    public async Task<IActionResult> UpdateListItem(Guid listId, Guid itemId, ListItem listItem)
    {
        if (itemId != listItem.ItemId || listId != listItem.ListId)
            return BadRequest();

        listItem.UpdatedAt = DateTimeOffset.UtcNow;
        context.Entry(listItem).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ListItemExists(listId, itemId))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{itemId}")]
    public async Task<IActionResult> DeleteListItem(Guid listId, Guid itemId)
    {
        var listItem = await context.ListItems
            .FirstOrDefaultAsync(li => li.ListId == listId && li.ItemId == itemId);

        if (listItem == null)
            return NotFound();

        // Soft delete
        listItem.IsDeleted = true;
        listItem.UpdatedAt = DateTimeOffset.UtcNow;
        await context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ListItemExists(Guid listId, Guid itemId)
    {
        return await context.ListItems
            .AnyAsync(e => e.ListId == listId && e.ItemId == itemId);
    }
}