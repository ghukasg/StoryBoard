using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Reflection;
using System.Web;
using log4net;
using log4net.Repository.Hierarchy;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StoryBoard.DAL.IRepositories;
using StoryBoard.Logger;
using StoryBoard.Models;
using StoryBoard.ViewModels;

namespace StoryBoard.DAL.Repositories
{
    public class GroupRepository : BaseLogger, IGroupRepository
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public List<GroupViewModel> GetGroups(string userId)
        {
            var user =
                    new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context)).FindById(userId);

            List<Group> groups = new List<Group>();

            try
            {
                groups = _context.Groups.ToList();
            }
            catch (EntitySqlException ex)
            {
                Logger.Error("An exception was thrown while doing GetGroups operation", ex);
            }

            return groups.Select(group => new GroupViewModel
            {
                Name = group.Name,
                Description = group.Description,
                NumberOfMembers = group.Users.Count,
                NumberOfStories = group.Stories.Count,
                GroupId = group.GroupId,
                IsMember = group.Users.Contains(user)
            }).ToList();
        }

        public GroupViewModel GetGroupById(string userId, int groupId)
        {
            var user =
                    new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context)).FindById(userId);

            var group = new Group();

            try
            {
                group = _context.Groups.FirstOrDefault(g => g.GroupId == groupId);

            }
            catch (Exception ex)
            {
                Logger.Error("An exception was thrown while doing GetGroupById operation", ex);
            }

            return new GroupViewModel
            {
                Name = group.Name,
                Description = group.Description,
                NumberOfMembers = group.Users.Count,
                NumberOfStories = group.Stories.Count,
                GroupId = group.GroupId,
                IsMember = group.Users.Contains(user)
            };
        }

        public IEnumerable<StoryViewModel> GetStoriesByGroupId(string userId, int groupId)
        {
            var stories = new List<Story>();

            try
            {
                stories = _context.Stories.Where(s => s.Groups.Any(g => g.GroupId == groupId)).ToList();

            }
            catch (Exception ex)
            {
                Logger.Error("An exception was thrown while doing GetStoriesByGroupId operation", ex);
            }

            return stories.Select(story => new StoryViewModel
            {
                Title = story.Title,
                Description = story.Description,
                Content = story.Content,
                PostedOn = story.PostedOn,
                StoryId = story.StoryId,
                //UserId = story.User.Id
            }).ToList();
        }

        public void InsertGroup(GroupViewModel group)
        {
            try
            {
                var user =
                   new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context)).FindById(group.OwnerId);

                var item = new Group
                {
                    Name = group.Name,
                    Description = group.Description,
                    Users = new HashSet<ApplicationUser> { user }
                };

                _context.Groups.Add(item);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Error("An exception was thrown while doing InsertGroup operation", ex);
            }
        }

        public void JoinToGroup(string userId, int groupId)
        {
            var user =
                    new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context)).FindById(userId);

            try
            {
                var group = _context.Groups.FirstOrDefault(g => g.GroupId == groupId);

                if (group == null)
                {
                    return;
                }

                group.Users.Add(user);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.Error("An exception was thrown while doing InsertGroup operation", ex);
            }
        }

        #region Dispose

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}