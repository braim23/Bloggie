﻿using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories;

public class BlogPostCommentRepository : IBlogPostCommentRepository
{
    private readonly BloggieDbContext _bloggieDbContext;

    public BlogPostCommentRepository(BloggieDbContext bloggieDbContext)
    {
        _bloggieDbContext = bloggieDbContext;
    }
    public async Task<BlogPostComment> AddAsync(BlogPostComment blogPostComment)
    {
        await _bloggieDbContext.Comments.AddAsync(blogPostComment);
        await _bloggieDbContext.SaveChangesAsync();
        return blogPostComment;
    }

    public async Task<IEnumerable<BlogPostComment>> GetCommentByBlogIdAsync(Guid blogPostId)
    {
        return await _bloggieDbContext.Comments.Where(x=> x.BlogPostId == blogPostId).ToListAsync();

    }
}
