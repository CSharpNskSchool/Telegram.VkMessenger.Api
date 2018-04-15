using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VkConnector.Model;
using VkConnector.Model.Messages;
using VkConnector.Services;
using VkNet;

namespace VkConnector.Extensions
{
    public static class SubscribeExtention
    {
        public static async Task StartGettingUpdates(this Subscription subscriptionModel)
        {
            var connector = new VKUpdatesGetter(subscriptionModel.User);
            var sender = new HttpClient();

            while (true)
            {
                var updates = connector.GetUserUpdates();
                if (updates.Count() != 0)
                {
                    var sendUpdatesTasks = updates.
                        Select(x => Task.Factory.StartNew(() => SendToPathAsync(sender, subscriptionModel.Url, x))).
                        ToArray();

                    Task.WaitAll(sendUpdatesTasks);
                }
            }
        }

        private static void SendToPathAsync(HttpClient client, Uri path, RecievedMessage message)
        {
            var coded = JsonConvert.SerializeObject(message);
            var values = new Dictionary<string, string>()
            {
                {"message", coded }
            };

            var toSend = new FormUrlEncodedContent(values);
            var result = client.PostAsync(path, toSend).Result;
        }
    }
}
