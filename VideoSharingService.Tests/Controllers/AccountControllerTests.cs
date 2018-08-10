using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace VideoSharingService.Controllers.Tests
{
    [TestClass()]
    public class AccountControllerTests
    {
        [TestMethod()]
        public void AccountControllerTest()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index(2) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void AccountControllerTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LoginTest()
        {
            AccountController controller = new AccountController();
        }

        [TestMethod()]
        public void LoginPostTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RegisterTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RegisterTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ConfirmEmailTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ForgotPasswordTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ForgotPasswordTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ForgotPasswordConfirmationTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ResetPasswordTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ResetPasswordTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ResetPasswordConfirmationTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LogOffTest()
        {
            Assert.Fail();
        }
    }
}