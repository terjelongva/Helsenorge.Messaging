/*
 * Copyright (c) 2021, Norsk Helsenett SF and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the MIT license
 * available at https://raw.githubusercontent.com/helsenorge/Helsenorge.Messaging/master/LICENSE
 */

using System.Threading.Tasks;
using Helsenorge.Messaging.Abstractions;
using Microsoft.Extensions.Logging;

namespace Helsenorge.Messaging.ServiceBus
{
    public class LinkFactoryPool
    {
        private readonly ServiceBusSettings _settings;
        private readonly ILogger<LinkFactoryPool> _logger;
        private readonly IServiceBusFactoryPool _factoryPool;
        private readonly ServiceBusReceiverPool _receiverPool;
        private readonly ServiceBusSenderPool _senderPool;

        public LinkFactoryPool(ServiceBusSettings settings, ILogger<LinkFactoryPool> logger)
        {
            _settings = settings;
            _logger = logger;
            _factoryPool = new ServiceBusFactoryPool(_settings);
            _receiverPool = new ServiceBusReceiverPool(_settings, _factoryPool);
            _senderPool = new ServiceBusSenderPool(_settings, _factoryPool);
        }

        public Task<IMessagingReceiver> CreateCachedMessagingReceiver(string queue)
            => _receiverPool.CreateCachedMessageReceiver(_logger, queue);

        public Task ReleaseCachedMessagingReceiver(string queue)
            => _receiverPool.ReleaseCachedMessageReceiver(_logger, queue);

        public Task<IMessagingSender> CreateCachedMessagingSender(string queue)
            => _senderPool.CreateCachedMessageSender(_logger, queue);

        public Task ReleaseCachedMessagingSender(string queue)
            => _senderPool.ReleaseCachedMessageSender(_logger, queue);
    }
}
