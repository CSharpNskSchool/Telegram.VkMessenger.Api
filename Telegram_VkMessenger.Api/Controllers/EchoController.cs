using Microsoft.AspNetCore.Mvc;

namespace Telegram_VkMessenger.Api.Controllers
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

            var size = message.Length;
            var result = new char[size];

            for (int i = 0; i < size; i++)
            {
                var symbol = message[i];
                var changedSymbol = char.IsUpper(symbol) ? char.ToLower(symbol) : char.ToUpper(symbol);
                result[i] = changedSymbol;
            }

            var answer = new string(result);
            return new ObjectResult(answer);
        }
    }
}


