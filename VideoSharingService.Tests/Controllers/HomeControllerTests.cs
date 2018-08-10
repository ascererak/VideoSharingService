using Microsoft.VisualStudio.TestTools.UnitTesting;
using VideoSharingService.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace VideoSharingService.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        [TestMethod()]
        public void IndexTest()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void ChangeCultureTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SearchTest()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.Search("jon") as ViewResult;

            Assert.AreEqual("Search", result.ViewName);
        }

        [TestMethod()]
        public void AboutSystemTest()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.AboutSystem() as ViewResult;

            Assert.AreEqual("", result.ViewName);
        }

        [TestMethod()]
        public void TermsOfServiceTest()
        {
           HomeController controller = new HomeController();

            ViewResult result = controller.TermsOfService() as ViewResult;

            Assert.AreEqual("", result.ViewName); 
        }

        [TestMethod()]
        public async Task DetailsTest()
        {
            HomeController controller = new HomeController();

            ViewResult result = await controller.Details(1) as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void NotFoundTest()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.NotFound() as ViewResult;

            Assert.AreEqual("", result.ViewName); 

        }

        [TestMethod()]
        public void ErrorTest()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.Error() as ViewResult;

            Assert.AreEqual("", result.ViewName); 

        }
    }
}