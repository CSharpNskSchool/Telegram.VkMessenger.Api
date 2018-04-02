using Microsoft.AspNetCore.Mvc;

namespace VkConnector.Controllers
{
    [Route("api/[controller]")]
    public class EchoController : Controller
    {
        /// <summary>
        /// Перевести сообщение в верхний регистр
        /// </summary>
        /// <param name="message"> Сообщение, которое необходимо перевести в верхний регистр </param>
        /// <response code="200"> Сообщение <paramref name="message"/> в верхнем регистре </response>
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
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


