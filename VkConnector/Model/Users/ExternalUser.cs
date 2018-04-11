using System.ComponentModel.DataAnnotations;

namespace VkConnector.Model.Users
{
    /// <summary>
    ///     Пользователь соц.сети, с которым пользователь бота хочет взаимодействовать.
    /// </summary>
    public class ExternalUser
    {
        public ExternalUser(string id)
        {
            this.Id = id;
        }

        /// <summary>
        ///     Id этого челика в соц.сети
        /// </summary>
        public string Id { get; }
    }
}