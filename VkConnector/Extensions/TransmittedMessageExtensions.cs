using System.Linq;
using System.Threading.Tasks;
using VkConnector.Model.Messages;
using VkNet;
using VkNet.Model.RequestParams;

namespace VkConnector.Extensions
{
    public static class TransmittedMessageExtensions
    {
        public static async Task Transfer(this TransmittedMessage transmittedMessage)
        {
            var api = new VkApi();
            await api.AuthorizeAsync(new ApiAuthParams
            {
                AccessToken = transmittedMessage.AuthorizedSender.AccessToken
            });

            var receivers = transmittedMessage.Receivers
                .Select(receiver => api.Utils.ResolveScreenName(receiver.Id))
                .Where(receiver => receiver.Id.HasValue)
                .Select(receiver => receiver.Id.Value);

            foreach (var receiver in receivers)
            {
                await api.Messages.SendAsync(new MessagesSendParams
                {
                    Message = transmittedMessage.Body.Text,
                    PeerId = receiver
                });
            }
        }
    }
}