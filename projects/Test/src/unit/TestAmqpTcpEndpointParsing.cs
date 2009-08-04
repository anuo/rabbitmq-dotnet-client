// This source code is dual-licensed under the Apache License, version
// 2.0, and the Mozilla Public License, version 1.1.
//
// The APL v2.0:
//
//---------------------------------------------------------------------------
//   Copyright (C) 2007-2009 LShift Ltd., Cohesive Financial
//   Technologies LLC., and Rabbit Technologies Ltd.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//---------------------------------------------------------------------------
//
// The MPL v1.1:
//
//---------------------------------------------------------------------------
//   The contents of this file are subject to the Mozilla Public License
//   Version 1.1 (the "License"); you may not use this file except in
//   compliance with the License. You may obtain a copy of the License at
//   http://www.rabbitmq.com/mpl.html
//
//   Software distributed under the License is distributed on an "AS IS"
//   basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//   License for the specific language governing rights and limitations
//   under the License.
//
//   The Original Code is The RabbitMQ .NET Client.
//
//   The Initial Developers of the Original Code are LShift Ltd,
//   Cohesive Financial Technologies LLC, and Rabbit Technologies Ltd.
//
//   Portions created before 22-Nov-2008 00:00:00 GMT by LShift Ltd,
//   Cohesive Financial Technologies LLC, or Rabbit Technologies Ltd
//   are Copyright (C) 2007-2008 LShift Ltd, Cohesive Financial
//   Technologies LLC, and Rabbit Technologies Ltd.
//
//   Portions created by LShift Ltd are Copyright (C) 2007-2009 LShift
//   Ltd. Portions created by Cohesive Financial Technologies LLC are
//   Copyright (C) 2007-2009 Cohesive Financial Technologies
//   LLC. Portions created by Rabbit Technologies Ltd are Copyright
//   (C) 2007-2009 Rabbit Technologies Ltd.
//
//   All Rights Reserved.
//
//   Contributor(s): ______________________________________.
//
//---------------------------------------------------------------------------
using NUnit.Framework;
using System;

namespace RabbitMQ.Client.Unit
{
    [TestFixture]
    public class TestAmqpTcpEndpointParsing
    {
        [Test]
        public void TestHostWithPort()
        {
            AmqpTcpEndpoint e = AmqpTcpEndpoint.Parse(Protocols.DefaultProtocol, "host:1234");
            Assert.AreEqual(Protocols.DefaultProtocol, e.Protocol);
            Assert.AreEqual("host", e.HostName);
            Assert.AreEqual(1234, e.Port);
        }

        [Test]
        public void TestHostWithoutPort()
        {
            AmqpTcpEndpoint e = AmqpTcpEndpoint.Parse(Protocols.DefaultProtocol, "host");
            Assert.AreEqual(Protocols.DefaultProtocol, e.Protocol);
            Assert.AreEqual("host", e.HostName);
            Assert.AreEqual(Protocols.DefaultProtocol.DefaultPort, e.Port);
        }

        [Test]
        public void TestEmptyHostWithPort()
        {
            AmqpTcpEndpoint e = AmqpTcpEndpoint.Parse(Protocols.DefaultProtocol, ":1234");
            Assert.AreEqual(Protocols.DefaultProtocol, e.Protocol);
            Assert.AreEqual("", e.HostName);
            Assert.AreEqual(1234, e.Port);
        }

        [Test]
        public void TestEmptyHostWithoutPort()
        {
            AmqpTcpEndpoint e = AmqpTcpEndpoint.Parse(Protocols.DefaultProtocol, ":");
            Assert.AreEqual(Protocols.DefaultProtocol, e.Protocol);
            Assert.AreEqual("", e.HostName);
            Assert.AreEqual(Protocols.DefaultProtocol.DefaultPort, e.Port);
        }

        [Test]
        public void TestCompletelyEmptyString()
        {
            AmqpTcpEndpoint e = AmqpTcpEndpoint.Parse(Protocols.DefaultProtocol, "");
            Assert.AreEqual(Protocols.DefaultProtocol, e.Protocol);
            Assert.AreEqual("", e.HostName);
            Assert.AreEqual(Protocols.DefaultProtocol.DefaultPort, e.Port);
        }

        [Test]
        public void TestInvalidPort()
        {
            try
            {
                AmqpTcpEndpoint.Parse(Protocols.DefaultProtocol, "host:port");
                Assert.Fail("Expected FormatException");
            }
            catch (FormatException)
            {
                // OK.
            }
        }

        [Test]
        public void TestMultipleNone()
        {
            AmqpTcpEndpoint[] es = AmqpTcpEndpoint.ParseMultiple(Protocols.DefaultProtocol, "  ");
            Assert.AreEqual(0, es.Length);
        }

        [Test]
        public void TestMultipleOne()
        {
            AmqpTcpEndpoint[] es = AmqpTcpEndpoint.ParseMultiple(Protocols.DefaultProtocol,
                                                                 " host:1234 ");
            Assert.AreEqual(1, es.Length);
            Assert.AreEqual("host", es[0].HostName);
            Assert.AreEqual(1234, es[0].Port);
        }

        [Test]
        public void TestMultipleTwo()
        {
            AmqpTcpEndpoint[] es = AmqpTcpEndpoint.ParseMultiple(Protocols.DefaultProtocol,
                                                                 " host:1234, other:2345 ");
            Assert.AreEqual(2, es.Length);
            Assert.AreEqual("host", es[0].HostName);
            Assert.AreEqual(1234, es[0].Port);
            Assert.AreEqual("other", es[1].HostName);
            Assert.AreEqual(2345, es[1].Port);
        }

        [Test]
        public void TestMultipleTwoMultipleCommas()
        {
            AmqpTcpEndpoint[] es = AmqpTcpEndpoint.ParseMultiple(Protocols.DefaultProtocol,
                                                                 ", host:1234,, ,,, other:2345,, ");
            Assert.AreEqual(2, es.Length);
            Assert.AreEqual("host", es[0].HostName);
            Assert.AreEqual(1234, es[0].Port);
            Assert.AreEqual("other", es[1].HostName);
            Assert.AreEqual(2345, es[1].Port);
        }
    }
}
