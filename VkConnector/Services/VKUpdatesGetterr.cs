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

        private const int wait = 20;
        private const int mode = 2;
        private const int version = 2;

        private string server;
        private string key;
        private ulong ts;
        private ulong pts;

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

            var updateResponse = client.GetAsync($"https://{server}?act=a_check&key={key}&ts={ts}&wait={wait}&mode={mode}&version={version}").Result;
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
            var messages = connector.Messages.GetLongPollHistory(new MessagesGetLongPollHistoryParams() { Ts = ts, Pts = pts }).
            Messages;
            foreach (var message in messages)
            {
                var sender = new ExternalUser((message.FromId ?? -1).ToString());
                var text = new MessageBody(message.Body);
                var recMsg = new RecievedMessage(sender, text);
                yield return recMsg;
            }
        }

        private void HandleVKServerErrors(int error, ulong ts)
        {
            switch (error)
            {
                case 1:
                    {
                        this.ts = ts;
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
            server = data.Server;
            key = data.Key;
            ts = data.Ts;
            pts = data.Pts ?? 0;
        }
    }
}
