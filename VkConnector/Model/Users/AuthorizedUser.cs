using System;
using System.ComponentModel.DataAnnotations;

namespace VkConnector.Model.Users
{
    /// <summary>
    ///     Информация, необходимая для аутентификации
    /// </summary>

    [Serializable]
    public class AuthorizedUser
    {
        /// <summary>
        ///     access_token социальной сети
        /// </summary>
        [Required]
        public string AccessToken { get; set; }
    }
}