using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories;

public class TagRepository : ITagRepository
{
    private readonly BloggieDbContext _dbContext;
    public TagRepository(BloggieDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Tag> AddAsync(Tag tag)
    {
        await _dbContext.Tags.AddAsync(tag);
        await _dbContext.SaveChangesAsync();
        return tag;
    }

    public async Task<Tag?> DeleteAsync(Guid id)
    {
        var existingTag = await _dbContext.Tags.FindAsync(id);
        if(existingTag != null)
        {
            _dbContext.Tags.Remove(existingTag);
            _dbContext.SaveChanges();
            return existingTag;
        }
        return null;
    }

    public async Task<IEnumerable<Tag>> GetAllAsync()
    {
        return await _dbContext.Tags.ToListAsync();
    }

    public async Task<Tag?> GetAsync(Guid id)
    {
       return await _dbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Tag?> UpdateAsync(Tag tag)
    {
        var existingTag = await _dbContext.Tags.FindAsync(tag.Id);

        if (existingTag != null)
        {
            existingTag.Name = tag.Name;
            existingTag.DisplayName = tag.DisplayName;
            await _dbContext.SaveChangesAsync();
            return existingTag;
        }
        return null;
    }
}
