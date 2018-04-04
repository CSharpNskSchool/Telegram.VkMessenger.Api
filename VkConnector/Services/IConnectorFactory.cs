using NVKConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NVKConnector.Services
{
    /// <summary>
    /// Вообще, тут не совсем фабрика, я не уверен что в паттерне
    /// есть метод "удалить". Но она создает коннекторы, как хочет, поэтому
    /// так назвал.
    /// Должна реализовывать логику добавления новых коннекторов в приложение по запросу.
    /// Это сделано, чтобы сразу предусмотреть проблему контроля ресурсов (вдруг пришло 5кк запросов
    /// на прослушивание, и некоторые придется отклонять)
    /// </summary>
    public interface IConnectorFactory
    {
        /// <summary>
        /// Пробует инициализировать новый коннектор
        /// </summary>
        /// <param name="connector"></param>
        /// <returns></returns>
        bool TryTake(out IConnector connector);

        /// <summary>
        /// Удаляет кооннектор из списка активных (освобождая место)
        /// </summary>
        /// <param name="connector"></param>
        /// <returns></returns>
        bool Remove(IConnector connector);
    }
}
