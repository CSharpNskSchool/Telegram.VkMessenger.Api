using Microsoft.AspNetCore.Mvc;
using VkConnector.Model;

namespace VkConnector.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SubscribeController : Controller
    {
        /// <summary>
        ///     Установка WebHook для получения обновлений пользователя
        /// </summary>
        /// <response code="200">WebHook успешно установлен</response>
        /// <response code="400">Установка WebHook не удалась</response>
        [HttpPost]
        public IActionResult Subscribe([FromBody] SubscribeModel subscribeModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseResult
                {
                    Ok = false,
                    Description = "Не передан url или данные для авторизации"
                });
            }

            return BadRequest(new ResponseResult {Ok = false, Description = "Метод еще не реализован"});
        }
    }
}