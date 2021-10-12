using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace APICommunicator.Controllers
{
    [ApiController]
    [Route("[controller]/v1")]
    public class ConvertController : ControllerBase
    {
        private IConfiguration configuration;

        [HttpGet("JsonToXml")]
        public int JsonToXml([FromServices] IConfiguration configuration)
        {
            return 12;
        }
    }
}
