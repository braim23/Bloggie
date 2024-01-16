using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bloggie.Web.Models.Domain;
using Microsoft.AspNetCore.Authorization;

namespace Bloggie.Web.Controllers;
[Authorize(Roles = "admin")]
public class AdminBlogPostsController : Controller
{
    private readonly ITagRepository _tagRepository;
    private readonly IBlogPostRepository _blogPostRepository;

    public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
    {
        _tagRepository = tagRepository;
        _blogPostRepository = blogPostRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        //talk to repository - get tags from it
        var tags = await _tagRepository.GetAllAsync();

        var model = new AddBlogPostViewModel
        {
            Tags = tags.Select(x => new SelectListItem
            {
                Text = x.DisplayName,
                Value = x.Id.ToString()
            })
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync(AddBlogPostViewModel model)
    {
        // Map view model to domain model
        var blogPostDomainModel = new BlogPost
        {
            Heading = model.Heading,
            PageTitle = model.PageTitle,
            Content = model.Content,
            ShortDescription = model.ShortDescription,
            FeaturedImageUrl = model.FeaturedImageUrl,
            UrlHandle = model.UrlHandle,
            PublishedDate = model.PublishedDate,
            Author = model.Author,
            Visible = model.Visible,
        };

        // Map tags from selected tags
        var selectedTags = new List<Tag>();
        foreach (var selectedTagId in model.SelectedTags)
        {
            var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
            var existingTag = await _tagRepository.GetAsync(selectedTagIdAsGuid);
            if (existingTag != null)
            {
                selectedTags.Add(existingTag);
            }
        };
        // Mapping tags back to domain model
        blogPostDomainModel.Tags = selectedTags;

        await _blogPostRepository.AddAsync(blogPostDomainModel);
        return RedirectToAction("Add");
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        // Call repo
        var blogPosts = await _blogPostRepository.GetAllAsync();

        return View(blogPosts);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        // Retrieve the result ffrom the repo
        var blogPost = await _blogPostRepository.GetAsync(id);
        var tagsDomainModel = await _tagRepository.GetAllAsync();
        if (blogPost != null)
        {
            // Map the domain model into the view model
            var model = new EditBlogPostVewModel
            {
                Id = blogPost.Id,
                Heading = blogPost.Heading,
                PageTitle = blogPost.PageTitle,
                Content = blogPost.Content,
                Author = blogPost.Author,
                FeaturedImageUrl = blogPost.FeaturedImageUrl,
                UrlHandle = blogPost.UrlHandle,
                ShortDescription = blogPost.ShortDescription,
                PublishedDate = blogPost.PublishedDate,
                Visible = blogPost.Visible,
                Tags = tagsDomainModel.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }),
                SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
            };
            // Pass data to view
            return View(model);
        }
        return View(null);
    }


    [HttpPost]
    public async Task<IActionResult> Edit(EditBlogPostVewModel model)
    {
        // map view model back to domain model
        var blogPostDomainModel = new BlogPost
        {
            Id = model.Id,
            Heading = model.Heading,
            PageTitle = model.PageTitle,
            Content = model.Content,
            ShortDescription = model.ShortDescription,
            FeaturedImageUrl = model.FeaturedImageUrl,
            UrlHandle = model.UrlHandle,
            PublishedDate = model.PublishedDate,
            Author = model.Author,
            Visible = model.Visible,
        };

        // map tags into domain model

        var selectedTags = new List<Tag>();
        foreach(var selectedTag in model.SelectedTags)
        {
            if(Guid.TryParse(selectedTag, out var tag))
            {
                var foundTag = await _tagRepository.GetAsync(tag);
                if(foundTag != null)
                {
                    selectedTags.Add(foundTag);
                }

            }
        }
        blogPostDomainModel.Tags = selectedTags;
        // submit information to repository to update
        var updatedBlogpost = await _blogPostRepository.UpdateAsync(blogPostDomainModel);
        if(updatedBlogpost != null)
        {
            // TODO: Show success notification
            return RedirectToAction("Edit");
        }
        // TODO: Show success notification
        return RedirectToAction("Edit");


    }


    [HttpPost]
    public async Task<IActionResult> Delete(EditBlogPostVewModel model)
    {
        // Talk to repo to delete
        var deletedBlogPost = await _blogPostRepository.DeleteAsync(model.Id);
        if(deletedBlogPost != null)
        {
            // TODO: show duccess notofication
            return RedirectToAction("List");
        }
        // TODO: show error notification
        return RedirectToAction("Edit", new { id = model.Id });
    }

    public async Task<IActionResult> Deletee(Guid id)
    {
        var deletedTag = await _blogPostRepository.DeleteAsync(id);
        if (deletedTag != null)
        {
            //TODO -- Show success notification
            return RedirectToAction("List");

        }
        // TODO: Show error notification
        return RedirectToAction("Edit", new { id = id });
    }


}
