using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VkConnector.Model;
using VkConnector.Model.Messages;
using VkConnector.Model.Users;
using VkNet;
using VkNet.Model.RequestParams;

namespace VkConnector.Extensions
{
    public static class SubscriptionModelExtention
    {
        // TODO: Саня доделает получение сообщений
        public static async Task SetWebHook(this SubscriptionModel subscriptionModelModel)
        {
            var api = new VkApi();
            await api.AuthorizeAsync(new ApiAuthParams
            {
                AccessToken = subscriptionModelModel.User.AccessToken
            });
            
            Task.Factory.StartNew(async () =>
            {
                foreach (var newMessage in api.GetNewMessages())
                {
                    await SendToWebHook(subscriptionModelModel.Url, newMessage);
                }
            });
        }
        
        private static IEnumerable<RecievedMessage> GetNewMessages(this VkApi vkApi)
        {
            const int longPoolWait = 20;
            const int longPoolMode = 2;
            const int longPoolVersion = 2;
            
            var client = new HttpClient();
            var longPollServer = vkApi.Messages.GetLongPollServer();
            ulong ts = longPollServer.Ts;
            
            while (true)
            {
                var updateResponse = client.GetAsync($"https://{longPollServer.Server}?act=a_check&key={longPollServer.Key}&ts={ts}&wait={longPoolWait}&mode={longPoolMode}&version={longPoolVersion}").Result;
                var jsoned = updateResponse.Content.ReadAsStringAsync().Result;
                var updates = JsonConvert.DeserializeObject<JObject>(jsoned);
                
                var longPollHistory = vkApi.Messages.GetLongPollHistoryAsync(new MessagesGetLongPollHistoryParams()
                {
                    Ts = ts
                }).Result;

                ts = updates["ts"].ToObject<ulong>();

                foreach (var message in longPollHistory.Messages)
                {
                    yield return new RecievedMessage(new ExternalUser(message.UserId.ToString()),
                        new MessageBody(message.Body));
                }          
            }
        }

        
        private static async Task SendToWebHook(Uri url, RecievedMessage message)
        {
            var client = new HttpClient();
            var toSend = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
            await client.PostAsync(url, toSend);
        }
    }
}