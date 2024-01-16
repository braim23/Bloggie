using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Repositories;

public interface IBlogPostLikeRepository
{
   Task<int> GetTotalLikes(Guid blogPostId);
    
   Task<IEnumerable<BlogPostLike>> GetLikesForBlog(Guid blogPostId);

   Task<BlogPostLike> AddLike(BlogPostLike blogPostLike);
   Task<BlogPostLike> RemoveLike(BlogPostLike blogPostLike);
}
