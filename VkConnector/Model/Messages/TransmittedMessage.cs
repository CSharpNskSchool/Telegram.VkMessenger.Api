using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VkConnector.Model.Users;

namespace VkConnector.Model.Messages
{
    /// <summary>
    ///     Передаваемое от пользователя соц. сети сообщение
    /// </summary>
    public class TransmittedMessage
    {
        /// <summary>
        ///     Авторизованный отправитель сообщения
        /// </summary>
        [Required]
        public AuthorizedUser AuthorizedSender { get; set; }

        /// <summary>
        ///     Получатели сообщения
        /// </summary>
        [Required]
        public IEnumerable<ExternalUser> Receivers { get; set; }


        /// <summary>
        ///     Тело сообщения
        /// </summary>
        [Required]
        public MessageBody Body { get; set; }
    }
}