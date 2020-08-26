using CryptoExchange.Net.Objects;
using System;

namespace LiquidQuoine.Net.Objects.Socket
{
    public class LiquidQuoineSocketClientOptions : SocketClientOptions
    {

        public string UserId { get; set; }
        public string PushherAppId { get; set; }

        private const string DefaultPusherId = "2ff981bb060680b5ce97";
        private const string BaseSocketUrl = "wss://echo.websocket.org";

        public new TimeSpan SocketResponseTimeout { get; set; } = TimeSpan.FromSeconds(5);

        public LiquidQuoineSocketClientOptions(
            string userId,
            string pusherId = DefaultPusherId) : base(BaseSocketUrl)
        {
            UserId = userId;
            PushherAppId = pusherId;
        }
    }
}
