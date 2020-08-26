﻿using LiquidQuoine.Net;
using LiquidQuoine.Net.Objects;
using LiquidQuoine.Net.Objects.Socket;
using Newtonsoft.Json;
using PusherClient;
using System;
using System.Collections.Generic;

namespace LiquidQuioine.Net.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                LiquidQuoineSocketClient _socketclient = new LiquidQuoineSocketClient(new LiquidQuoineSocketClientOptions("")
                {
                    LogVerbosity = CryptoExchange.Net.Logging.LogVerbosity.Debug,
                    // BaseAddress= "wss://tap.liquid.com/app/LiquidTapClient"

                });
                _socketclient.SubscribeToOrderBookSide("btcusd", OrderSide.Buy, OnData);
                //Console.WriteLine("subscrbng");
                //_socketclient.SubscribeToMyExecutions("QASHETH", Catch);

                Console.WriteLine("asdasdasd");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }

        private static void OnData(List<LiquidQuoineOrderBookEntry> arg1, OrderSide arg2, string arg3)
        {
            try
            {
                Console.WriteLine(JsonConvert.SerializeObject(arg1));
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
        }

        private static void _myChannel_Subscribed(object sender)
        {
            Console.WriteLine("Subscribed");
        }

        private static void _pusher_ConnectionStateChanged(object sender, ConnectionState state)
        {
            Console.WriteLine(state);
        }

        private static void _pusher_Error(object sender, PusherException error)
        {
            Console.WriteLine(JsonConvert.SerializeObject(error));
        }

        //private static void Catch(LiquidQuoineExecution e, string symbol)
        //{
        //    var eo = e;
        //    try
        //    {
        //        Console.WriteLine(JsonConvert.SerializeObject(eo));
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("hmmm");
        //        Console.WriteLine(ex.ToString());
        //    }
        //}
    }
}
