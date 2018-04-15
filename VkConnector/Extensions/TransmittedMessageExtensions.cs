using System;
using System.Linq;
using System.Threading.Tasks;
using VkConnector.Model.Messages;
using VkNet;
using VkNet.Model.RequestParams;

namespace VkConnector.Extensions
{
    public static class TransmittedMessageExtensions
    {
        /// <summary>
        ///     Рассылает сообщение по прикрепленным в объекте адресатам
        /// </summary>
        /// 
        /// <param name="transmittedMessage">
        ///     Пересылаемое сообщение. Содержит информацию авторизованного польователя бота,
        ///     тело сообщения, адресатов.
        /// </param>
        /// 
        /// <returns>
        ///     Задача с телом Action
        /// </returns>
        public static async Task Transfer(this TransmittedMessage transmittedMessage)
        {
            var api = new VkApi();
            await api.CheckedAuthorizeAsync(transmittedMessage.AuthorizedSender.AccessToken);

            var transferTasks = transmittedMessage.Receivers
                .Select(receiver => api.GetResolvedId(receiver.Id).Result)
                .Select(receiverId => new Task(async () =>
                {
                    await api.SendMessage(receiverId, transmittedMessage.Body);
                }))
                .ToArray();

            foreach (var transferTask in transferTasks)
            {
                transferTask.Start();
            }

            Task.WaitAll(transferTasks);
        }

        private static async Task SendMessage(this VkApi api, long peerId, MessageBody messageBody)
        {
            await api.Messages.SendAsync(new MessagesSendParams
            {
                Message = messageBody.Text,
                PeerId = peerId
            });
        }

        /// <summary>
        ///     Получение id адресата Вконтакте по короткому имени.
        /// </summary>
        /// <remarks> Отдельно обрабатываются групповые беседы, имеющие идентификаторы вида 'c*номер*' </remarks>
        /// <returns>Id пользователя, если он найден. Иначе null.</returns>
        private static async Task<long> GetResolvedId(this VkApi api, string receiver)
        {
            if (receiver.StartsWith("c") && long.TryParse(receiver.Substring(1), out var groupChatId))
            {
                return 2000000000 + groupChatId;
            }

            try
            {
                var resolvedReceiver = await api.Utils.ResolveScreenNameAsync(receiver);
                if (resolvedReceiver.Id != null)
                {
                    return resolvedReceiver.Id.Value;
                }
            }
            catch (Exception e)
            {
                // ignore
            }

            throw new ArgumentException($"Получатель не найден: {receiver}");
        }
    }
}