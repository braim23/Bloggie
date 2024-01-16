
using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories;

public class BlogPostLikeRepository : IBlogPostLikeRepository
{
    private readonly BloggieDbContext _dbContext;

    public BlogPostLikeRepository(BloggieDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BlogPostLike> AddLike(BlogPostLike blogPostLike)
    {
        await _dbContext.BlogPostLike.AddAsync(blogPostLike);
        await _dbContext.SaveChangesAsync();
        return blogPostLike;

    }
    public async Task<BlogPostLike> RemoveLike(BlogPostLike blogPostLike)
    {
        var like = await _dbContext.BlogPostLike.FirstAsync(x => x.UserId == blogPostLike.UserId);
        if (like != null)
        {
            _dbContext.BlogPostLike.Remove(blogPostLike);
            await _dbContext.SaveChangesAsync();
        }
        return blogPostLike;
    }
    public async Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId)
    {
        return await _dbContext.BlogPostLike.Where(x => x.BlogPostId == blogPostId).ToListAsync();
    }

    public async Task<int> GetTotalLikes(Guid blogPostId)
    {
        return await _dbContext.BlogPostLike.CountAsync(x => x.BlogPostId == blogPostId);
    }


}
