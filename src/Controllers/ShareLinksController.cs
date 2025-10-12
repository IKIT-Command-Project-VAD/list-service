using ListService.Domain.Entities;
using ListService.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ListService.Controllers;

public class ShareLinksController(ListsDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShareLink>>> GetShareLinks(Guid listId)
    {
        return await context.ShareLinks
            .Where(sl => sl.ListId == listId)
            .ToListAsync();
    }

    [HttpGet("{shareId}")]
    public async Task<ActionResult<ShareLink>> GetShareLink(Guid listId, Guid shareId)
    {
        var shareLink = await context.ShareLinks
            .FirstOrDefaultAsync(sl => sl.ListId == listId && sl.ShareId == shareId);

        if (shareLink == null)
            return NotFound();

        return shareLink;
    }

    [HttpPost]
    public async Task<ActionResult<ShareLink>> CreateShareLink(Guid listId, ShareLink shareLink)
    {
        shareLink.ShareId = Guid.NewGuid();
        shareLink.ListId = listId;
        shareLink.ShareToken = Guid.NewGuid().ToString("N")[..16]; // Generate a random token
        shareLink.CreatedAt = DateTimeOffset.UtcNow;
        shareLink.IsActive = true;

        context.ShareLinks.Add(shareLink);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetShareLink), 
            new { listId = listId, shareId = shareLink.ShareId }, shareLink);
    }

    [HttpPut("{shareId}")]
    public async Task<IActionResult> UpdateShareLink(Guid listId, Guid shareId, ShareLink shareLink)
    {
        if (shareId != shareLink.ShareId || listId != shareLink.ListId)
            return BadRequest();

        context.Entry(shareLink).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ShareLinkExists(listId, shareId))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{shareId}")]
    public async Task<IActionResult> DeleteShareLink(Guid listId, Guid shareId)
    {
        var shareLink = await context.ShareLinks
            .FirstOrDefaultAsync(sl => sl.ListId == listId && sl.ShareId == shareId);

        if (shareLink == null)
            return NotFound();

        context.ShareLinks.Remove(shareLink);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ShareLinkExists(Guid listId, Guid shareId)
    {
        return await context.ShareLinks
            .AnyAsync(e => e.ListId == listId && e.ShareId == shareId);
    }
}