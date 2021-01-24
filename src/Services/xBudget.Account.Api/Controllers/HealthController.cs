using Microsoft.AspNetCore.Mvc;

namespace xBudget.Account.Api.Controllers
{
    [Route("api/health")]
    [ApiController]
    public class HealthController : ControllerBase
    {

        [HttpGet("status")]
        public IActionResult Status()
        {
            return Ok($"{ System.Reflection.Assembly.GetEntryAssembly().GetName().Name } is running");
        }
    }
}
