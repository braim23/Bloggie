using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories;

public class BlogPostRepository : IBlogPostRepository
{
    private readonly BloggieDbContext _dbContext;

    public BlogPostRepository(BloggieDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BlogPost> AddAsync(BlogPost blogPost)
    {
        await _dbContext.AddAsync(blogPost);
        await _dbContext.SaveChangesAsync();
        return blogPost;
    }

    public async Task<BlogPost?> DeleteAsync(Guid id)
    {
      var existingBlog =  await _dbContext.BlogPosts.FindAsync(id);
        if (existingBlog != null)
        {
            _dbContext.BlogPosts.Remove(existingBlog);
            await _dbContext.SaveChangesAsync();
            return existingBlog;
        }
        return null;
    }

    public async Task<IEnumerable<BlogPost>> GetAllAsync()
    {
        return await _dbContext.BlogPosts.Include(x=>x.Tags).ToListAsync();
    }

    public async Task<BlogPost?> GetAsync(Guid id)
    {
        return await _dbContext.BlogPosts.Include(x=>x.Tags).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
    {
       return await _dbContext.BlogPosts.Include(x=>x.Tags)
            .FirstOrDefaultAsync(x=>x.UrlHandle == urlHandle);
    }

    public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
    {
        var existingBlog = await _dbContext.BlogPosts.Include(x=>x.Tags)
            .FirstOrDefaultAsync(x=>x.Id == blogPost.Id);

        if(existingBlog != null)
        {
            existingBlog.Id = blogPost.Id;
            existingBlog.Heading = blogPost.Heading;
            existingBlog.PageTitle = blogPost.PageTitle;
            existingBlog.Content = blogPost.Content;
            existingBlog.ShortDescription = blogPost.ShortDescription;
            existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
            existingBlog.UrlHandle = blogPost.UrlHandle;
            existingBlog.PublishedDate = blogPost.PublishedDate;
            existingBlog.Author = blogPost.Author;
            existingBlog.Visible = blogPost.Visible;
            existingBlog.Tags = blogPost.Tags;
            await _dbContext.SaveChangesAsync();
            return existingBlog;
        }
        return null;
    }
}
