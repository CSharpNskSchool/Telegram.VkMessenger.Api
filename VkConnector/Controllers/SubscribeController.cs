using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using VkConnector.Extensions;
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
        public async Task<IActionResult> Subscribe([FromBody] Subscription subscription)
        {
        //{
        //    var body = new StreamReader(Request.Body).ReadToEnd();
        //    var subscribeModel = JsonConvert.DeserializeObject<Subscription>(body);

        //    var test = new Subscription() { Url = }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseResult
                {
                    IsOk = false,
                    Description = "Не передан url или данные для авторизации"
                });
            }

            await subscription.StartGettingUpdates();

            return BadRequest(new ResponseResult {IsOk = false, Description = "Метод еще не реализован"});
        }
    }
}