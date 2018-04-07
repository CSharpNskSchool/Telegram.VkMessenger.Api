using System;
using System.ComponentModel.DataAnnotations;
using VkConnector.Model.Users;

namespace VkConnector.Model
{
    /// <summary>
    /// </summary>
    public class SubscriptionModel
    {
        /// <summary>
        ///     Url, на который будут приходить уведомления о новых сообщениях
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        ///     Информация для авторизации
        /// </summary>
        [Required]
        public AuthorizedUser User { get; }
    }
}