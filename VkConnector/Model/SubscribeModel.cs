using System;

namespace VkConnector.Model
{
    /// <summary>
    /// </summary>
    public class SubscribeModel
    {
        /// <summary>
        ///     url, на который будут приходить уведомления о новых сообщениях
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        ///     Информация для авторизации
        /// </summary>
        public AuthModel AuthModel { get; set; }
    }
}