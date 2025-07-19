using API.Commons;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("errors/{code}")]
    public class ErrorController : ApiControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error(int code)
        {
            return new ObjectResult(ResponseFactory.Fail(code, "An error occurred."));
        }
    }
}
