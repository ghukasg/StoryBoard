using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using StoryBoard.Models;

namespace StoryBoard.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("StoryBoardConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Story> Stories { get; set; }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}