using System.ComponentModel.DataAnnotations;

namespace VkConnector.Model
{
    /// <summary>
    ///     Информация, необходимая для аутентификации под именем пользователя Вконтакте
    /// </summary>
    public class AuthModel
    {
        /// <summary>
        ///     acess_token пользователя Вконтакте
        /// </summary>
        [Required]
        public string AcessToken { get; set; }
    }
}