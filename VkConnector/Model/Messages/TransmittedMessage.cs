using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VkConnector.Model.Users;

namespace VkConnector.Model.Messages
{
    /// <summary>
    ///     Передаваемое от пользователя бота людям из соц. сети сообщение
    /// </summary>
    public class TransmittedMessage
    {
        /// <summary>
        ///     Получатели сообщения
        /// </summary>
        [Required]
        public IEnumerable<ExternalUser> Receivers { get; }


        /// <summary>
        ///     Тело сообщения
        /// </summary>
        [Required]
        public MessageBody Body { get; }
    }
}
