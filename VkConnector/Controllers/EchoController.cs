using Microsoft.AspNetCore.Mvc;

namespace VkConnector.Controllers
{
    [Route("api/[controller]")]
    public class EchoController : Controller
    {
        [HttpPost]
        public IActionResult GetReverseRegister([FromBody] string message)
        {
            if (message == null)
            {
                return BadRequest();
            }
            
            return new ObjectResult(message.ToUpper());
        }
    }
}


