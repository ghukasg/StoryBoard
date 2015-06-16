using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StoryBoard.DAL.Repositories;
using StoryBoard.Models;
using StoryBoard.ViewModels;
using PagedList;
using PagedList.Mvc;

namespace StoryBoard.Controllers
{
    [Authorize]
    public class MyStoriesController : Controller
    {
        readonly StoryRepository _repository = new StoryRepository();

        private MultiSelectList GetGroupsList(string[] selectedValues)
        {
            var userId = User.Identity.GetUserId();

            var result = _repository.GetGroups(userId);
             
            return new MultiSelectList(result, "Id", "Name", selectedValues);
        }

        // GET: MyStories
        public ActionResult Index(int page = 1, int pageSize = 9)
        {
            var userId = User.Identity.GetUserId();
            var stories = _repository.GetStories(userId);

            var model = new PagedList<StoryViewModel>(stories, page, pageSize);

            return View(model);
        }

        public ActionResult Create()
        {
            ViewBag.MultiSelectGroups = GetGroupsList(null);

            return View();
        }

        [HttpPost]
        public ActionResult Create(StoryViewModel story)
        {
            story.UserId = User.Identity.GetUserId();

            _repository.InsertStory(story);

            return RedirectToAction("Index");
        }

        public ActionResult Details(int storyId)
        {
            var result = _repository.GetStoryById(storyId);

            return View(result);
        }

        public ActionResult Edit(int storyId)
        {
            ViewBag.MultiSelectGroups = GetGroupsList(null);

            var result = _repository.GetStoryById(storyId);

            return View(result);
        }

        [HttpPost]
        public ActionResult Edit(StoryViewModel story)
        {
            _repository.EditStory(story);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int storyId)
        {
            _repository.DeleteStory(storyId);

            return RedirectToAction("Index");
        }
    }
}