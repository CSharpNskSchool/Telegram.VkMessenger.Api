using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VkConnector.Model.Messages;
using VkConnector.Model.Users;
using VkNet;
using VkNet.Model.RequestParams;

namespace VkConnector.Services
{
    public class VKUpdatesGetter
    {
        private readonly VkApi connector = new VkApi(null, null, null);
        private readonly HttpClient client = new HttpClient();


        private const int longPoolWait = 20;
        private const int longPoolMode = 2;
        private const int longPoolVersion = 2;

        private string longPoolServer;
        private string longPoolKey;
        private ulong longPoolTs;
        private ulong longPoolPts;


        /// <summary>
        ///     Класс с ответственностью возвращать обновления пользователя.
        ///     Хранит так же всякую дичь для связи с сервером ВК.
        /// </summary>
        /// 
        /// <param name="user">
        ///     Пользователь, который прошел процедуру залогинивания и
        ///     получил ключ-токен для совершения действий с API.
        /// </param>
        public VKUpdatesGetter(AuthorizedUser user)
        {
            connector.Authorize(new ApiAuthParams() { AccessToken = user.AccessToken });
            UpdateLongpollServerInfo();
        }

        /// <summary>
        ///     Блокирует поток до появления обновлений у пользователя,
        ///     либо до истечения времени существования longpoll соединения
        /// </summary>
        /// 
        /// <returns>
        ///     Обновления пользователя.
        ///     (на 11.04.18 - возвращает обновления в текстовых сообщениях)
        /// </returns>
        public IEnumerable<RecievedMessage> GetUserUpdates()
        {
            var result = new List<RecievedMessage>();


            var updateResponse = client.GetAsync($"https://{longPoolServer}?act=a_check&key={longPoolKey}&ts={longPoolTs}&wait={longPoolWait}&mode={longPoolMode}&version={longPoolVersion}").Result;

            var jsoned = updateResponse.Content.ReadAsStringAsync().Result;
            var updates = (JObject)JsonConvert.DeserializeObject(jsoned);


            var failedCode = updates["failed"];
            if (failedCode != null)
            {
                HandleVKServerErrors(failedCode.ToObject<int>(), updates["ts"].ToObject<ulong>());
                return result;
            }
            else
            {
                return GetTextMessagesUpdates();
            }
        }

        private IEnumerable<RecievedMessage> GetTextMessagesUpdates()
        {

            var messages = connector.Messages.GetLongPollHistory(new MessagesGetLongPollHistoryParams() { Ts = longPoolTs, Pts = longPoolPts }).

            Messages;
            foreach (var message in messages)
            {
                var sender = new ExternalUser((message.FromId ?? -1).ToString());
                var text = new MessageBody(message.Body);

                var recievedMessage = new RecievedMessage(sender, text);
                yield return recievedMessage;

            }
        }

        private void HandleVKServerErrors(int error, ulong ts)
        {
            switch (error)
            {
                case 1:
                    {

                        this.longPoolTs = ts;

                        break;
                    }
                case 2:
                case 3:
                    {
                        UpdateLongpollServerInfo();
                        break;
                    }
                case 4:
                    {
                        break;
                    }

            }
        }

        private void UpdateLongpollServerInfo()
        {
            var data = connector.Messages.GetLongPollServer(true);

            longPoolServer = data.Server;
            longPoolKey = data.Key;
            longPoolTs = data.Ts;
            longPoolPts = data.Pts ?? 0;

        }
    }
}
