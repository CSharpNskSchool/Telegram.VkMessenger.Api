using System.ComponentModel.DataAnnotations;
using VkConnector.Model.Users;

namespace VkConnector.Model.Messages
{
    /// <summary>
    ///     Получаемое пользователем сообщение
    /// </summary>
    public class RecievedMessage
    {
        /// <summary>
        ///     Отправитель (челик из соц.сети)
        /// </summary>
        [Required]
        public ExternalUser Sender { get; }

        /// <summary>
        ///     Тело сообщения
        /// </summary>
        [Required]
        public MessageBody Body { get; }
    }
}