using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AccountAPI.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly ILogger _logger;

        protected BaseController(ILogger logger)
        {
            _logger = logger;
        }

        protected async Task<ActionResult> HandleOperationAsync(Func<Task<ActionResult>> operation)
        {
            try
            {
                return await operation();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }



        protected ActionResult HandleValidationErrors()
        {
            var errors = ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return BadRequest(new { Errors = errors });
        }

        protected string GetUserId()
        {
            try
            {
                string userId = "";

                ClaimsIdentity userContext = (ClaimsIdentity)HttpContext.User.Identity;

                if (userContext.Claims.Any() && userContext.Claims.Any(x => x.Type.Contains("nameidentifier")))
                    userId = userContext.Claims.FirstOrDefault(x => x.Type.Contains("nameidentifier")).Value;

                return userId;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected string GetUsername()
        {
            try
            {
                string userName = "";

                ClaimsIdentity userContext = (ClaimsIdentity)HttpContext.User.Identity;

                if (userContext.Claims.Any() && userContext.Claims.Any(x => x.Type.Contains("name")))
                    userName = userContext.Claims.FirstOrDefault(x => x.Type.Contains("name")).Value;

                return userName;
            }
            catch (Exception)
            {
                return null;
            };
        }

        

    }
}
