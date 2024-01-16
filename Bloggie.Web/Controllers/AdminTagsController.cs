using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers;
[Authorize(Roles = "admin")]
public class AdminTagsController : Controller
{
    private readonly ITagRepository _tagRepository;
    public AdminTagsController(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddTagViewModel tagViewModel)
    {
        //ValidateAddTagRequest(tagViewModel);

        if (!ModelState.IsValid)
        {
            return View();
        }
        //map tag view model to the tag domain model
        var tag = new Tag
        {
            Name = tagViewModel.Name,
            DisplayName = tagViewModel.DisplayName,
        };

        await _tagRepository.AddAsync(tag);

        return RedirectToAction("List");
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var tags = await _tagRepository.GetAllAsync();
        return View(tags);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var tag = await _tagRepository.GetAsync(id);
        if (tag != null)
        {
            var editTagRequest = new EditTagViewModel
            {
                Id = tag.Id,
                Name = tag.Name,
                DisplayName = tag.DisplayName
            };
            return View(editTagRequest);
        }

        return View(null);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditTagViewModel editTagViewModel)
    {
        var tag = new Tag
        {
            Id = editTagViewModel.Id,
            Name = editTagViewModel.Name,
            DisplayName = editTagViewModel.DisplayName,
        };

        //update here

        var updatedTag = await _tagRepository.UpdateAsync(tag);
        if(updatedTag != null)
        {
            //TODO -- Show success notification
        }
        else
        {
            // TODO -- Show error notification
        }

        return RedirectToAction("Edit", new {id = editTagViewModel.Id});
    }

    [HttpPost]
    public async Task<IActionResult> Delete(EditTagViewModel editTagViewModel)
    {
        var deletedTag = await _tagRepository.DeleteAsync(editTagViewModel.Id);
        if (deletedTag != null)
        {
            //TODO -- Show success notification
            return RedirectToAction("List");
            
        }
        // TODO: Show error notification
        return RedirectToAction("Edit", new { id = editTagViewModel.Id });

    }

    [HttpGet]
    public async Task<IActionResult> Deletee(Guid id)
    {
        var deletedTag = await _tagRepository.DeleteAsync(id);
        if (deletedTag != null)
        {
            //TODO -- Show success notification
            return RedirectToAction("List");

        }
        // TODO: Show error notification
        return RedirectToAction("Edit", new { id = id });
    }
    //private void ValidateAddTagRequest(AddTagViewModel tagViewModel)
    //{
    //    if(tagViewModel.Name != null && tagViewModel.DisplayName != null)
    //    {
    //        if(tagViewModel.Name.Length<= 1)
    //        {
    //            ModelState.AddModelError("Name", "meow meow");
    //        }
    //    }
    //}
}
