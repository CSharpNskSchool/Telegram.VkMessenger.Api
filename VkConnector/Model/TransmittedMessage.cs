using System.ComponentModel.DataAnnotations;

namespace VkConnector.Model
{
    /// <summary>
    ///     Пересылаемое сообщение
    /// </summary>
    public class TransmittedMessage
    {
        /// <summary>
        ///     Информация для авторизации
        /// </summary>
        [Required]
        public AuthModel AuthModel { get; set; }

        /// <summary>
        ///     Сообщение
        /// </summary>
        [Required]
        public Message Message { get; set; }
    }
}