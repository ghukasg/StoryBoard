using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StoryBoard.DAL.IRepositories;
using StoryBoard.Logger;
using StoryBoard.Models;
using StoryBoard.ViewModels;

namespace StoryBoard.DAL.Repositories
{
    public class StoryRepository : BaseLogger, IStoryRepository
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public void InsertStory(StoryViewModel story)
        {
            try
            {
                var user =
                    new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context)).FindById(story.UserId);

                var groups =
                    story.Groups.Select(groupId => _context.Groups.FirstOrDefault(g => g.GroupId == groupId)).ToList();

                _context.Stories.Add(new Story
                {
                    Content = story.Content,
                    Description = story.Description,
                    PostedOn = DateTime.Now,
                    Title = story.Title,
                    User = user,
                    Groups = groups
                });

                _context.SaveChanges();

            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("An exception was thrown while doing InsertStory operation", ex);
            }
        }

        public List<StoryViewModel> GetStories(string userId)
        {
            var stories = new List<Story>();

            try
            {
                stories = _context.Stories.Where(s => s.User.Id == userId).ToList();

            }
            catch (Exception ex)
            {
                Logger.Error("An exception was thrown while doing GetStories operation", ex);
            }

            return stories.Select(story => new StoryViewModel
            {
                Title = story.Title,
                Description = story.Description,
                Content = story.Content,
                PostedOn = story.PostedOn,
                StoryId = story.StoryId,
                UserId = userId
            }).ToList();
        }

        public StoryViewModel GetStoryById(int storyId)
        {
            var story = new Story();

            try
            {
                story = _context.Stories.FirstOrDefault(s => s.StoryId == storyId);
            }
            catch (Exception ex)
            {
                Logger.Error("An exception was thrown while doing GetStoryById operation", ex);
            }

            if (story == null)
            {
                return null;
            }

            var ids = story.Groups.Select(g => g.GroupId).ToList();

            return new StoryViewModel
            {
                Title = story.Title,
                Description = story.Description,
                Content = story.Content,
                PostedOn = story.PostedOn,
                Groups = ids.ToArray()
            };
        }

        public void EditStory(StoryViewModel story)
        {
            try
            {
                var user =
                    new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context)).FindById(story.UserId);

                var result = _context.Stories.FirstOrDefault(s => s.StoryId == story.StoryId);

                var groups =
                    story.Groups.Select(groupId => _context.Groups.FirstOrDefault(g => g.GroupId == groupId)).ToList();

                if (result == null)
                {
                    return;
                }

                result.Content = story.Content;
                result.Description = story.Description;
                result.PostedOn = DateTime.Now;
                result.Title = story.Title;
                result.Groups = groups;

                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("An exception was thrown while doing EditStory operation", ex);
            }
        }

        public void DeleteStory(int storyId)
        {
            try
            {
                var result = _context.Stories.FirstOrDefault(s => s.StoryId == storyId);

                if (result == null)
                {
                    return;
                }

                _context.Stories.Remove(result);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                Logger.Error("An exception was thrown while doing DeleteStory operation", ex);
            }
        }

        public List<GroupMultiselectViewModel> GetGroups(string userId)
        {
            var user =
                   new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context)).FindById(userId);

            var groups = new List<Group>();

            try
            {
                groups = _context.Groups.ToList();
            }
            catch (Exception ex)
            {
                Logger.Error("An exception was thrown while doing DeleteStory operation", ex);
            }

            var userGroups = groups.Where(g => g.Users.Contains(user)).ToList();

            return userGroups.Select(group => new GroupMultiselectViewModel
            {
                Id = group.GroupId,
                Name = group.Name
            }).ToList();
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
    }

        #endregion
}