﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetXP.NetStandard.NetFramework.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using NetXP.NetStandard.DependencyInjection;
using NetXP.NetStandard.Cryptography;
using NetXP.NetStandard.DependencyInjection.Implementations.StructureMaps;
using StructureMap;
using di = NetXP.NetStandard.DependencyInjection;
using Microsoft.Extensions.Configuration;
using NetXP.NetStandard.Network.Email;
using NetXP.NetStandard.Network.TCP;
using NetXP.NetStandard.Network.Proxy.Implementations;
using System.Net;
using NetXP.NetStandard.Configuration.Implementations;
using Microsoft.Extensions.Options;

namespace NetXP.NetStandard.NetCore.Cryptography.Tests
{
    [TestClass()]
    public class Proxy_Tests
    {
        private di.IContainer container;

        public ISymetricCrypt ISymetric { get; private set; }

        [TestInitialize]
        public void Init()
        {
            Container smapContainer = new Container();
            container = new SMContainer(smapContainer);
            container.Configuration.Configure((IRegister cnf) =>
            {
                cnf.AddNetXPNetCoreRegisters(container);
            });

        }

        [TestMethod]
        public void NCNF_TCP_Proxy()
        {
            var clientConnectorFactory = container.Resolve<IClientConnectorFactory>("proxy");
            var clientConnector = clientConnectorFactory.Create();

            var googleIp = "64.233.190.100";
            var googleHost = "google.com";

            ///Any IP With Data
            ///TODO: Test proxy connetion, clientConnector.Connect()
            clientConnector.Connect(IPAddress.Parse(googleIp), 80);

            var messageToSend = Encoding.ASCII.GetBytes($"GET / HTTP/1.1\r\nHost: {googleHost}\r\nAccept-Language: es\r\n\r\n\r\n");
            clientConnector.Send(messageToSend, 0, messageToSend.Length);

            var buffer = new byte[1024];
            clientConnector.Receive(buffer, 0, buffer.Length);

            var receivedMessage = Encoding.ASCII.GetString(buffer);
        }

        [TestMethod]
        public void NCNF_TCP_NOT_Proxy()
        {
            ///Trying connection without configuration (ProxyOptions = null)
            container.Configuration.Configure((IRegister cnf) =>
            {
                cnf.RegisterInstance<IOptions<ProxyOptions>>(new OptionsInstance<ProxyOptions>(null), LifeTime.Singleton);
            });

            var clientConnectorFactory = container.Resolve<IClientConnectorFactory>("proxy");///Proxy Factory
            var clientConnector = clientConnectorFactory.Create();///Proxy Connector

            var googleIp = "64.233.190.100";
            var googleHost = "google.com";

            ///Any IP With Data
            ///TODO: Test proxy connetion, clientConnector.Connect()
            clientConnector.Connect(IPAddress.Parse(googleIp), 80);

            var messageToSend = Encoding.ASCII.GetBytes($"GET / HTTP/1.1\r\nHost: {googleHost}\r\nAccept-Language: es\r\n\r\n\r\n");
            clientConnector.Send(messageToSend, 0, messageToSend.Length);

            var buffer = new byte[1024];
            clientConnector.Receive(buffer, 0, buffer.Length);

            var receivedMessage = Encoding.ASCII.GetString(buffer);
        }

    }
}