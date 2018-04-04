using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NVKConnector.Services;

namespace NVKConnector.Controllers
{
    /// <summary>
    /// Контроллер для подписывания/отписывания токенов к/от обновлениям (чтобы получать в телеграме сообщения из соц сетей)
    /// и передачи сообщений в социальные сети
    /// </summary>
    [Route("api/[controller]")]
    public class SpyonController : Controller
    {
        private string mainApiAdress;
        private HttpClient client;
        private IConnector connector;
        private ILogger<SpyonController> logger;

        /// <summary>
        /// Конструктор контроллера с сервисами - фабрика коннекторов к социальным сети, и логгер
        /// </summary>
        /// 
        /// <param name="factory"> API запрашивает для какого-либо токена слежку
        /// за его аккаунтом в социальной сети, и фабрика внутри реализует логику выдачи
        /// коннекторов для этой цели</param>
        /// 
        /// <param name="logger">Логгер</param>
        public SpyonController(IConnectorFactory factory, ILogger<SpyonController> logger)
        {
            this.client = new HttpClient();
            this.logger = logger;

            var gotConnector = factory.TryTake(out var connector);
            if (gotConnector)
            {
                this.connector = connector;
                logger.LogInformation("Created IConnector instance");
            }
            else
            {
                logger.LogError("Unable to create IConnector instanse from IConnectorFactory");
            }
        }

        /// <summary>
        /// Подписаться на обновления пользователя. В запросе POST прикреплен токен.
        /// </summary>
        [HttpPost]
        public void Subscribe()
        {
            User user;

            using (var streamReader = new StreamReader(Request.Body))
            {
                var body = streamReader.ReadToEnd();
                user = JsonConvert.DeserializeObject<User>(body);
            }
            
            connector.SetUser(user);

            while (true)
            {
                var messages = connector.GetUserUpdatesAsync().Result;
                var tasks = messages.Select(x => Task.Factory.StartNew(() => SendToMainAPIAsync(mainApiAdress, x))).ToArray();
                Task.WaitAll(tasks);
            }
        }

        private void SendToMainAPIAsync(string path, Message message)
        {
            var coded = JsonConvert.SerializeObject(message);
            //logger.LogInformation(coded);
            var values = new Dictionary<string, string>()
            {
                {"message", coded }
            };

            var toSend = new FormUrlEncodedContent(values);
            var result = client.PostAsync(path, toSend).Result;
        }
    }
}
