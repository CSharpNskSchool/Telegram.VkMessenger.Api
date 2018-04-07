using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VkConnector.Model.Users;

namespace VkConnector.Model.Messages
{
    /// <summary>
    ///     Получаемое пользователем бота сообщение
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
