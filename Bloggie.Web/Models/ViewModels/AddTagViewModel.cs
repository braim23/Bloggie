using System.ComponentModel.DataAnnotations;

namespace Bloggie.Web.Models.ViewModels;

public class AddTagViewModel
{
    //request model
    [Required]
    public string Name { get; set; }
    [Required]
    public string DisplayName { get; set; }
}
