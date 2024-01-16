namespace Bloggie.Web.Models.ViewModels;

public class LikeRequest
{
    public Guid BlogPostId { get; set; }
    public Guid UserId { get; set; }
}
