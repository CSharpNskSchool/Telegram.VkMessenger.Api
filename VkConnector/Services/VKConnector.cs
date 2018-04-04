using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model.RequestParams;

namespace NVKConnector
{
    /// <summary>
    /// Коннектор к соц сети Вконтакте.
    /// Там много захардкоденой дичи, которая
    /// требуется для взаимодействия с сервером ВК.
    /// 
    /// TODO: Может вынести ее в отдельный класс?
    /// </summary>
    public class VKConnector : IConnector
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


        public void CloseUserSession()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(User user, Message message)
        {
            throw new NotImplementedException();
        }

        public void SetUser(User user)
        {
            connector.Authorize(new ApiAuthParams() { AccessToken = user.Token, Settings = Settings.Messages });
            UpdateLongpollServerInfo();
        }

        /// <summary>
        /// Получает обновления сообщений, используя лонгпол сервер ВК.
        /// Там своя специфика, лонгполл каждые ~20sec. возвращает
        /// "Апдейты" в массиве размером 0 (как будто они были), хотя их и не было.
        /// 
        /// Пока возвращаются только сообщения
        /// </summary>
        /// <returns></returns>
        public async Task<List<Message>> GetUserUpdatesAsync()
        {
            var updateResponse = await client.GetAsync($"https://{server}?act=a_check&key={key}&ts={ts}&wait={wait}&mode={mode}&version={version}");
            var jsoned = updateResponse.Content.ReadAsStringAsync().Result;
            var updates = (JObject)JsonConvert.DeserializeObject(jsoned);

            var failedCode = updates["failed"];
            if (failedCode != null)
            {
                HandleVKServerErrors(failedCode.ToObject<int>(), updates["ts"].ToObject<ulong>());
                return new List<Message>() { new Message() { Text = "InternalError" } };
            }
            else
            {
                var result = new List<Message>();
                var messageUpdates = connector.Messages.GetLongPollHistory(new MessagesGetLongPollHistoryParams() { Ts = this.ts, Pts = this.pts });
                
                foreach (var msg in messageUpdates.Messages)
                {
                    result.Add(new Message() { Text = msg.Body });
                }

                return result;
            }
        }

        private void HandleVKServerErrors(int error, ulong ts)
        {
            switch(error)
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
