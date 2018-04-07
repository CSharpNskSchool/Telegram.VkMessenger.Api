﻿using System.ComponentModel.DataAnnotations;

namespace VkConnector.Model.Users
{
    /// <summary>
    ///     Пользователь соц.сети, с которым пользователь бота хочет взаимодействовать.
    /// </summary>
    public class ExternalUser
    {
        /// <summary>
        ///     Id этого челика в соц.сети
        /// </summary>
        [Required]
        public string Id { get; set; }
    }
}