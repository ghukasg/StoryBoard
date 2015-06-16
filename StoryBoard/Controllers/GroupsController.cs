using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using StoryBoard.DAL.Repositories;
using StoryBoard.ViewModels;
using PagedList;

namespace StoryBoard.Controllers
{
    [Authorize]
    public class GroupsController : Controller
    {
        readonly GroupRepository _repository = new GroupRepository();

        [AllowAnonymous]
        // GET: Group
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            var userId = User.Identity.GetUserId();

            var result = _repository.GetGroups(userId);

            var model = new PagedList<GroupViewModel>(result, page, pageSize);

            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(GroupViewModel group)
        {
            group.OwnerId = User.Identity.GetUserId();

            _repository.InsertGroup(group);

            return RedirectToAction("Index");
        }

        public ActionResult ViewStories(int groupId, int page = 1, int pageSize = 9)
        {
            var userId = User.Identity.GetUserId();

            var result = _repository.GetStoriesByGroupId(userId, groupId);

            var model = new PagedList<StoryViewModel>(result, page, pageSize);

            return View(model);
        }

        public ActionResult Join(int groupId)
        {
            var userId = User.Identity.GetUserId();

            _repository.JoinToGroup(userId, groupId);

            return RedirectToAction("Index");
        }
    }
}