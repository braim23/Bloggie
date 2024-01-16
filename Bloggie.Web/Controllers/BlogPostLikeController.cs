using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogPostLikeController : ControllerBase
{
    private readonly IBlogPostLikeRepository _likeRepository;

    public BlogPostLikeController(IBlogPostLikeRepository likeRepository)
    {
        _likeRepository = likeRepository;
    }
    [HttpPost]
    [Route("Add")]
    public async Task<IActionResult> AddLike([FromBody] LikeRequest request)
    {
        var model = new BlogPostLike
        {
            BlogPostId = request.BlogPostId,
            UserId = request.UserId,
        };

        await _likeRepository.AddLike(model);
        return Ok();
    }

    [HttpPost]
    [Route("Remove")]
    public async Task<IActionResult> RemoveLike([FromBody] LikeRequest request)
    {
        var model = new BlogPostLike
        {
            BlogPostId = request.BlogPostId,
            UserId = request.UserId,
        };

        await _likeRepository.RemoveLike(model);
        return Ok();

    }

    [HttpGet]
    [Route("{blogPostId:Guid}/totalLikes")]
    public async Task<IActionResult> GetTotalLikesForBlog([FromRoute] Guid blogPostId)
    {
        var totalLikes = await _likeRepository.GetTotalLikes(blogPostId);
        return Ok(totalLikes);
    }
}
