using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers;

public class BlogsController : Controller
{
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly IBlogPostLikeRepository _likeRepository;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IBlogPostCommentRepository _commentRepository;

    public BlogsController( IBlogPostRepository blogPostRepository, 
                            IBlogPostLikeRepository likeRepository,
                            SignInManager<IdentityUser> signInManager,
                            UserManager<IdentityUser> userManager,
                            IBlogPostCommentRepository commentRepository)
    {
        _blogPostRepository = blogPostRepository;
        _likeRepository = likeRepository;
        _signInManager = signInManager;
        _userManager = userManager;
        _commentRepository = commentRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string urlHandle)
    {
        var liked = false;
        var blogPost = await _blogPostRepository.GetByUrlHandleAsync(urlHandle);
        var blogPostLikeViewModel = new BlogDetailsViewModel();

        if (blogPost != null)
        {
            var totalLikes = await _likeRepository.GetTotalLikes(blogPost.Id);
            if (_signInManager.IsSignedIn(User))
            {
                // Get the like of this user to this post
                var likeForBlog = await _likeRepository.GetLikesForBlog(blogPost.Id);
                var userId = _userManager.GetUserId(User);
                if(userId != null)
                {
                   var likeFromUser = likeForBlog.FirstOrDefault(x=>x.UserId == Guid.Parse(userId));
                   // Switch to true if the user is not null
                   liked = likeFromUser != null;

                }
            }

            // Get comments for blog post
            var blogCommentDomainModel = await _commentRepository.GetCommentByBlogIdAsync(blogPost.Id);
            var blogCommentsForView = new List<BlogComment>();
            foreach (var blogComment in blogCommentDomainModel)
            {
                blogCommentsForView.Add(new BlogComment
                {
                    Text = blogComment.Text,
                    DateTimeAdded = blogComment.DateTimeAdded,
                    Username = (await _userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName
                });
            }


            blogPostLikeViewModel = new BlogDetailsViewModel
            {
                Id = blogPost.Id,
                Content = blogPost.Content,
                PageTitle = blogPost.PageTitle,
                Author = blogPost.Author,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                Heading = blogPost.Heading,
                PublishedDate = blogPost.PublishedDate,
                ShortDescription = blogPost.ShortDescription,
                UrlHandle = blogPost.UrlHandle,
                Visible = blogPost.Visible,
                Tags = blogPost.Tags,
                TotalLikes = totalLikes,
                Liked = liked,
                Comments = blogCommentsForView

            };
        }
        return View(blogPostLikeViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Index(BlogDetailsViewModel blogDetailsViewModel)
    {
        if (_signInManager.IsSignedIn(User))
        {
            var commentDomainModel = new BlogPostComment
            {
                BlogPostId = blogDetailsViewModel.Id,
                Text = blogDetailsViewModel.CommentInput,
                UserId = Guid.Parse(_userManager.GetUserId(User)),
                DateTimeAdded = DateTime.Now
            };
            await _commentRepository.AddAsync(commentDomainModel);
            return RedirectToAction("Index", "Blogs", new {urlHandle = blogDetailsViewModel.UrlHandle});
        }
        return View();


    }
}
