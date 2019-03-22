﻿using CryptoExchange.Net;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Sockets;
using LiquidQuoine.Net.Converters;
using LiquidQuoine.Net.Interfaces;
using Newtonsoft.Json;
using PusherClient;
using System;
using System.Collections.Generic;

namespace LiquidQuoine.Net.Objects.Socket
{
    public class LiquidQuoineSocketClient : SocketClient, ILiquidQuoineSocketClient
    {
        private Pusher _pusherClient;
        /*

12:49:49.220
TODO: {"event":"pusher:subscribe","data":{"channel":"product_51_resolution_3600_tickers"}}	84	
*/

        /// <summary>
        /// need to send user id, eg 651514
        /// </summary>
        private const string UserInfoChannel = "user_{}";
        /// <summary>
        /// need to send pair code e.g. ethusd and pair id, e.g. 27
        /// </summary>
        private const string MarketInfoChannel = "product_cash_{}_{}";
        /// <summary>
        /// Pair code e.g. ethusd
        /// </summary>
        private const string AllExecutionsChannel = "executions_cash_{}";
        /// <summary>
        /// User id, eg 651514 and pair code e.g. ethusd
        /// </summary>
        private const string UserExecutionsChannel = "executions_{}_cash_{}";
        /// <summary>
        /// User id, eg 651514 and pair ticker e.g. eth
        /// </summary>
        private const string UserCurrencyUpdatesChannel = "user_{}_account_{}";
        /// <summary>
        /// need to send pair code e.g. ethusd and pair id, e.g. 27
        /// </summary>
        private const string OrderBookSideChannel = "price_ladders_cash_{}_{}";
        private readonly string _currentUserId;
        private TimeSpan socketResponseTimeout = TimeSpan.FromSeconds(5);

        public LiquidQuoineSocketClient(LiquidQuoineSocketClientOptions options) : base(options, null)
        {
            _currentUserId = options.UserId;
            Configure(options);
            log.Level = LogVerbosity.Debug;
            _pusherClient = new Pusher(options.PushherAppId, new PusherOptions() { ProtocolNumber = 7, Version = "4.4.0" });
            _pusherClient.Connect();
        }

        public void SubscribeToOrderBookSide(string symbol, OrderSide side, Action<List<LiquidQuoineOrderBookEntry>, OrderSide, string> onData)
        {
            var _myChannel = _pusherClient.Subscribe(FillPathParameter(OrderBookSideChannel, symbol, JsonConvert.SerializeObject(side, new OrderSideConverter())));
            _myChannel.Bind("updated", (dynamic data) =>
            {
                Console.WriteLine(data);
                onData(Deserialize<List<LiquidQuoineOrderBookEntry>>(data.ToString()), side, symbol);
            });
        }

        public void SubscribeToUserExecutions(string symbol, Action<LiquidQuoineExecution, string> onData, string userId = null)
        {
            var _myChannel = _pusherClient.Subscribe(FillPathParameter(UserCurrencyUpdatesChannel, userId ?? _currentUserId, symbol));
            _myChannel.Bind("created", (dynamic data) =>
            {
                onData(Deserialize<LiquidQuoineExecution>(data.ToString()), symbol);
            });
        }
        public void SubscribeToExecutions(string symbol, Action<LiquidQuoineExecution, string> onData)
        {
            var _myChannel = _pusherClient.Subscribe(FillPathParameter(AllExecutionsChannel, symbol));
            _myChannel.Bind("created", (dynamic data) =>
            {
                onData(Deserialize<LiquidQuoineExecution>(data.ToString()), symbol);
            });
        }

        protected override bool SocketReconnect(SocketSubscription subscription, TimeSpan disconnectedTime)
        {
            return true;
        }
    }
}