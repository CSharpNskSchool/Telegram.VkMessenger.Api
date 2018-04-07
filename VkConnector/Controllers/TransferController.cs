using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VkConnector.Extensions;
using VkConnector.Model;
using VkConnector.Model.Messages;

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
        public async Task<IActionResult> TransferMessage([FromBody] TransmittedMessage transmittedMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseResult
                {
                    IsOk = false,
                    Description = "Не передано сообщение или данные для авторизации"
                });
            }

            try
            {
                await transmittedMessage.Transfer();
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseResult
                {
                    IsOk = false,
                    Description = $"Ошибка отправки сообщения: {e.Message}"
                });
            }

            return Ok(new ResponseResult {IsOk = false, Description = "Сообщение отправлено"});
        }
    }
}