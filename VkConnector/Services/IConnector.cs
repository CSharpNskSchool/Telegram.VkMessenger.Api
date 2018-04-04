using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NVKConnector
{
    /// <summary>
    /// Потом поменяем формат, когда определимся с ним
    /// </summary>
    public class Message
    {
        public string Text { get; set; }
    }


    /// <summary>
    /// Потом поменяем формат, когда определимся с ним
    /// </summary>
    public class User
    {
        public string Id { get; set; }
        public string Token { get; set; }
    }

    /// <summary>
    /// Операции, которые реализует коннектор к социальной сети.
    /// Некоторые пока лишние, постарался подсмотреть чутка вперед.
    /// </summary>
    public interface IConnector
    {
        /// <summary>
        /// Настраивает пользователя для коннектора.
        /// Через конструктор передать не смог сразу, 
        /// там какая-то сложность с этим в Dependency injection,
        /// поэтому пока не стал делать.
        /// </summary>
        /// 
        /// <param name="user"> Пользователь коннектора. </param>
        void SetUser(User user);

        /// <summary>
        /// Получить обновления сообщений данного пользователя
        /// </summary>
        /// <returns></returns>
        Task<List<Message>> GetUserUpdatesAsync();

        /// <summary>
        /// пока не предполагается к реализации
        /// </summary>
        void CloseUserSession();

        /// <summary>
        /// пока не предполагается к реализации
        /// </summary>
        void SendMessage(User user, Message message);
    }
}
