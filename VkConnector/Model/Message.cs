using System.ComponentModel.DataAnnotations;

namespace VkConnector.Model
{
    /// <summary>
    ///     Сообщение
    /// </summary>
    public class Message
    {
        /// <summary>
        ///     Идентификатор (screename) отправителя Вконтакте
        /// </summary>
        [Required]
        public string From { get; set; }

        /// <summary>
        ///     Идентификатор (screename) получателя Вконтакте
        /// </summary>
        [Required]
        public string To { get; set; }

        /// <summary>
        ///     Текстовое содержимое сообщения
        /// </summary>
        public string Body { get; set; }
    }
}