using AccountAPI.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Controllers
{
    public class TestableBaseController : BaseController
    {
        public TestableBaseController(ILogger logger) : base(logger) { }

        public virtual async Task<ActionResult> HandleOperationAsync(Func<Task<ActionResult>> operation)
        {
            return await base.HandleOperationAsync(operation);
        }

        public string GetUsernameTest()
        {
            return this.GetUsername();
        }

        public string GetUserIdTest()
        {
            return this.GetUserId();
        }
    }

    public class BaseControllerTests
    {
        private readonly Mock<ILogger> _mockLogger;
        private readonly TestableBaseController _testableBaseController;

        public BaseControllerTests()
        {
            _mockLogger = new Mock<ILogger>();
            _testableBaseController = new TestableBaseController(_mockLogger.Object);
        }

        [Fact]
        public async Task ManusearOperacaoAsync_RetornaCodigoStatus500_QuandoExcecaoGenericaLancada()
        {
            // Arrange
            Func<Task<ActionResult>> operation = () => throw new Exception("Test exception");

            // Act
            var result = await _testableBaseController.HandleOperationAsync(operation);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }

        [Fact]
        public async Task ManusearOperacaoAsync_RetornaOk_QuandoOperacaoBemSucedida()
        {
            // Arrange
            Func<Task<ActionResult>> operation = async () => await Task.FromResult(new OkResult());

            // Act
            var result = await _testableBaseController.HandleOperationAsync(operation);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void GetUsername_QuandoExcecaoLancada_RetornaNull()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            var identity = new Mock<ClaimsIdentity>();
            mockHttpContext.Setup(context => context.User.Identity).Returns(identity.Object);
            identity.Setup(id => id.Claims).Throws(new Exception());


            var controller = new TestableBaseController(_mockLogger.Object) { ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object } };

            // Act
            var result = controller.GetUsernameTest();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetUserId_QuandoExcecaoLancada_RetornaNull()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            var identity = new Mock<ClaimsIdentity>();
            mockHttpContext.Setup(context => context.User.Identity).Returns(identity.Object);
            identity.Setup(id => id.Claims).Throws(new Exception());


            var controller = new TestableBaseController(_mockLogger.Object) { ControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object } };

            // Act
            var result = controller.GetUserIdTest();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetUsername_QuandoNameIdentifierPresente_RetornaUserName()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "user123"),
                new Claim("custom-claim-type", "custom-value")
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var user = new ClaimsPrincipal(identity);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.User).Returns(user);

            var controller = new TestableBaseController(_mockLogger.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            // Act
            var result = controller.GetUsernameTest();

            // Assert
            Assert.Equal("user123", result);
        }

        [Fact]
        public void GetUserId_QuandoNameIdentifierPresente_RetornaUserId()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),  // Corrigido para usar ClaimTypes.NameIdentifier
                new Claim("custom-claim-type", "custom-value")
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var user = new ClaimsPrincipal(identity);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.User).Returns(user);

            var controller = new TestableBaseController(_mockLogger.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
                }
            };

            // Act
            var result = controller.GetUserIdTest();

            // Assert
            Assert.Equal("123", result);
        }

    }


}
