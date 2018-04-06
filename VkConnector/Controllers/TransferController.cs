using Microsoft.AspNetCore.Mvc;
using VkConnector.Model;

namespace VkConnector.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class TransferController : Controller
    {
        /// <summary>
        ///     Отправка сообщения от имени пользователя
        /// </summary>
        /// <response code="200">Успешная отправка сообщения</response>
        /// <response code="400">Отправка сообщения не удалась</response>
        [HttpPost]
        public IActionResult TransferMessage([FromBody] TransmittedMessage transmittedMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseResult
                {
                    Ok = false,
                    Description = "Не передано сообщение или данные для авторизации"
                });
            }

            return BadRequest(new ResponseResult {Ok = false, Description = "Метод еще не реализован"});
        }
    }
}