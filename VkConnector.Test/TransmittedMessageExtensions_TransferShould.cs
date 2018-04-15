using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VkConnector.Extensions;
using VkConnector.Model.Messages;
using VkConnector.Model.Users;
using VkNet.Exception;
using Xunit;

namespace VkConnector.Test
{    
    public class TransmittedMessageExtensions_TransferShould
    {
        private const string ValidAcessToken = "";

        [Fact]
        public async void Message_With_Invalid_Credentials_Throws_UserAuthorizationFailException()
        {
            TransmittedMessage transmittedMessage = new TransmittedMessage()
            {
                AuthorizedSender = new AuthorizedUser()
                {
                    AccessToken = "invalid"
                }
            };
            
            await Assert.ThrowsAsync<UserAuthorizationFailException>(async () => await transmittedMessage.Transfer());
        }
        
        [Fact]
        public async void Message_With_Nonexistent_Receiver_Throws_AggregateException()
        {
            TransmittedMessage transmittedMessage = new TransmittedMessage()
            {
                AuthorizedSender = new AuthorizedUser()
                {
                    AccessToken = ValidAcessToken
                },
                Body = new MessageBody("sample_text")
            };
            var Receivers = new[]
            {
                new ExternalUser("very long just nonexistent username with spaces")
            };
            
            var aggregateException = await Assert.ThrowsAsync<AggregateException>(async () => await transmittedMessage.Transfer());
        }
    }
}
