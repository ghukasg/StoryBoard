using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoryBoard.Controllers;
using StoryBoard.DAL.Repositories;
using StoryBoard.ViewModels;

namespace StoryBoard.Tests.Controllers
{
    [TestClass]
    public class GroupsControllerTest
    {
        readonly GroupRepository _repository = new GroupRepository();

        [TestMethod]
        public void Index(int page = 1, int pageSize = 10)
        {
            // Arrange
            GroupsController controller = new GroupsController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Create()
        {
            GroupsController controller = new GroupsController();

            ViewResult result = controller.Create() as ViewResult;

            if (result == null)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ViewStories(int groupId, int page = 1, int pageSize = 9)
        {

        }

        [TestMethod]
        public void Join(int groupId)
        {

        }
    }
}