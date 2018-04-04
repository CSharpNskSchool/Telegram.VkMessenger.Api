using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NVKConnector;

namespace NVKConnector.Services
{
    /// <summary>
    /// Фабрика ВК Коннекторов. Уже думаю, не излишний ли оверхед,
    /// но надеюсь что это сделало код более гибким.
    /// Добавляется в сервисы в классе Startup
    /// </summary>
    public class VkConnectorsFactory : IConnectorFactory
    {
        private readonly ConcurrentDictionary<VKConnector, byte> connectors = new ConcurrentDictionary<VKConnector, byte>();

        public bool Remove(IConnector connector)
        {
            var vkConnector = connector as VKConnector;
            connectors.TryRemove(vkConnector, out _);
            return true;
        }

        public bool TryTake(out IConnector connector)
        {
            var candidate = new VKConnector();
            connectors.TryAdd(candidate, 0);

            connector = candidate;
            return true;
        }
    }
}
