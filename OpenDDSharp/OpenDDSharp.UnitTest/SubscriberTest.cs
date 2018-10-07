﻿/*********************************************************************
This file is part of OpenDDSharp.

OpenDDSharp is a .NET wrapper for OpenDDS.
Copyright (C) 2018 Jose Morato

OpenDDSharp is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

OpenDDSharp is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with OpenDDSharp. If not, see <http://www.gnu.org/licenses/>.
**********************************************************************/
using System.Linq;
using System.Collections.Generic;
using OpenDDSharp.DDS;
using OpenDDSharp.Test;
using OpenDDSharp.OpenDDS.DCPS;
using OpenDDSharp.UnitTest.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OpenDDSharp.UnitTest
{
    [TestClass]
    public class SubscriberTest
    {
        #region Constants
        private const int DOMAIN_ID = 42;
        #endregion

        #region Fields
        private static DomainParticipantFactory _dpf;
        private DomainParticipant _participant;
        #endregion

        #region Initialization/Cleanup
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _dpf = ParticipantService.Instance.GetDomainParticipantFactory();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _participant = _dpf.CreateParticipant(DOMAIN_ID);
            Assert.IsNotNull(_participant);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (_participant != null)
            {
                ReturnCode result = _participant.DeleteContainedEntities();
                Assert.AreEqual(ReturnCode.Ok, result);
            }

            if (_dpf != null)
            {
                ReturnCode result = _dpf.DeleteParticipant(_participant);
                Assert.AreEqual(ReturnCode.Ok, result);
            }
        }
        #endregion

        #region Test Methods
        [TestMethod]
        [TestCategory("Subscriber")]
        public void TestGetParticipant()
        {
            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);
            Assert.AreEqual(_participant, subscriber.Participant);
        }

        [TestMethod]
        [TestCategory("Subscriber")]
        public void TestGetQos()
        {
            // Create a non-default QoS and create a subscriber with it
            SubscriberQos qos = new SubscriberQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.GroupData.Value = new List<byte> { 0x42 };
            qos.Partition.Name = new List<string> { "TestPartition" };
            qos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos;
            qos.Presentation.CoherentAccess = true;
            qos.Presentation.OrderedAccess = true;

            Subscriber subscriber = _participant.CreateSubscriber(qos);
            Assert.IsNotNull(subscriber);

            // Call GetQos and check the values received
            qos = new SubscriberQos();
            ReturnCode result = subscriber.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsFalse(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(1, qos.GroupData.Value.Count());
            Assert.AreEqual(0x42, qos.GroupData.Value.First());
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(1, qos.Partition.Name.Count());
            Assert.AreEqual("TestPartition", qos.Partition.Name.First());
            Assert.IsTrue(qos.Presentation.CoherentAccess);
            Assert.IsTrue(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.GroupPresentationQos, qos.Presentation.AccessScope);
        }

        [TestMethod]
        [TestCategory("Subscriber")]
        public void TestSetQos()
        {
            // Create a new Subscriber using the default QoS
            Subscriber subscriber = _participant.CreateSubscriber();

            // Get the qos to ensure that is using the default properties
            SubscriberQos qos = new SubscriberQos();
            ReturnCode result = subscriber.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsTrue(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(0, qos.GroupData.Value.Count());
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(0, qos.Partition.Name.Count());
            Assert.IsFalse(qos.Presentation.CoherentAccess);
            Assert.IsFalse(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.InstancePresentationQos, qos.Presentation.AccessScope);

            // Try to change an immutable property
            qos.Presentation.CoherentAccess = true;
            qos.Presentation.OrderedAccess = true;
            qos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos;
            result = subscriber.SetQos(qos);
            Assert.AreEqual(ReturnCode.ImmutablePolicy, result);

            // Change some mutable properties and check them
            qos = new SubscriberQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.GroupData.Value = new List<byte> { 0x42 };
            qos.Partition.Name = new List<string> { "TestPartition" };
            result = subscriber.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new SubscriberQos();
            result = subscriber.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsFalse(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(1, qos.GroupData.Value.Count());
            Assert.AreEqual(0x42, qos.GroupData.Value.First());
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(1, qos.Partition.Name.Count());
            Assert.AreEqual("TestPartition", qos.Partition.Name.First());
            Assert.IsFalse(qos.Presentation.CoherentAccess);
            Assert.IsFalse(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.InstancePresentationQos, qos.Presentation.AccessScope);

            // Try to set immutable QoS properties before enable the publisher
            DomainParticipantQos pQos = new DomainParticipantQos();
            pQos.EntityFactory.AutoenableCreatedEntities = false;
            result = _participant.SetQos(pQos);
            Assert.AreEqual(ReturnCode.Ok, result);

            Subscriber otherSubscriber = _participant.CreateSubscriber();
            qos = new SubscriberQos();
            qos.EntityFactory.AutoenableCreatedEntities = false;
            qos.GroupData.Value = new List<byte> { 0x42 };
            qos.Partition.Name = new List<string> { "TestPartition" };
            qos.Presentation.CoherentAccess = true;
            qos.Presentation.OrderedAccess = true;
            qos.Presentation.AccessScope = PresentationQosPolicyAccessScopeKind.GroupPresentationQos;
            result = otherSubscriber.SetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new SubscriberQos();
            result = otherSubscriber.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            Assert.IsNotNull(qos.EntityFactory);
            Assert.IsNotNull(qos.GroupData);
            Assert.IsNotNull(qos.Partition);
            Assert.IsNotNull(qos.Presentation);
            Assert.IsFalse(qos.EntityFactory.AutoenableCreatedEntities);
            Assert.IsNotNull(qos.GroupData.Value);
            Assert.AreEqual(1, qos.GroupData.Value.Count());
            Assert.AreEqual(0x42, qos.GroupData.Value.First());
            Assert.IsNotNull(qos.Partition.Name);
            Assert.AreEqual(1, qos.Partition.Name.Count());
            Assert.AreEqual("TestPartition", qos.Partition.Name.First());
            Assert.IsTrue(qos.Presentation.CoherentAccess);
            Assert.IsTrue(qos.Presentation.OrderedAccess);
            Assert.AreEqual(PresentationQosPolicyAccessScopeKind.GroupPresentationQos, qos.Presentation.AccessScope);
        }

        [TestMethod]
        [TestCategory("Subscriber")]
        public void TestGetListener()
        {
            // Create a new Subscriber with a listener
            MySubscriberListener listener = new MySubscriberListener();
            Subscriber subscriber = _participant.CreateSubscriber(listener);
            Assert.IsNotNull(subscriber);

            // Call to GetListener and check the listener received
            MySubscriberListener received = (MySubscriberListener)subscriber.GetListener();
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);
        }

        [TestMethod]
        [TestCategory("Subscriber")]
        public void TestSetListener()
        {
            // Create a new Subscriber without listener
            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            MySubscriberListener listener = (MySubscriberListener)subscriber.GetListener();
            Assert.IsNull(listener);

            // Create a listener, set it and check that is correctly setted
            listener = new MySubscriberListener();
            ReturnCode result = subscriber.SetListener(listener, StatusMask.AllStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);

            MySubscriberListener received = (MySubscriberListener)subscriber.GetListener();
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            // Remove the listener calling SetListener with null and check it
            result = subscriber.SetListener(null, StatusMask.NoStatusMask);
            Assert.AreEqual(ReturnCode.Ok, result);

            received = (MySubscriberListener)subscriber.GetListener();
            Assert.IsNull(received);
        }

        [TestMethod]
        [TestCategory("Subscriber")]
        public void TestNewDataReaderQos()
        {
            DataReaderQos qos = new DataReaderQos();
            TestDefaultDataReaderQos(qos);
        }

        [TestMethod]
        [TestCategory("Subscriber")]
        public void TestGetDefaultDataReaderQos()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestGetDefaultDataReaderQos), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.GetListener());
            Assert.AreEqual(nameof(TestGetDefaultDataReaderQos), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            // Create a non-default DataReader Qos, call GetDefaultDataReaderQos and check the default values 
            DataReaderQos qos = CreateNonDefaultDataReaderQos();
            result = subscriber.GetDefaultDataReaderQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestDefaultDataReaderQos(qos);
        }

        [TestMethod]
        [TestCategory("Subscriber")]
        public void TestSetDefaultDataReaderQos()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestSetDefaultDataReaderQos), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.GetListener());
            Assert.AreEqual(nameof(TestSetDefaultDataReaderQos), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            // Creates a non-default QoS, set it an check it
            DataReaderQos qos = CreateNonDefaultDataReaderQos();
            result = subscriber.SetDefaultDataReaderQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = new DataReaderQos();
            result = subscriber.GetDefaultDataReaderQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestNonDefaultDataReaderQos(qos);

            DataReader reader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(reader);

            qos = new DataReaderQos();
            result = reader.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestNonDefaultDataReaderQos(qos);

            // Put back the default QoS and check it
            qos = new DataReaderQos();
            result = subscriber.SetDefaultDataReaderQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);

            qos = CreateNonDefaultDataReaderQos();
            result = subscriber.GetDefaultDataReaderQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestDefaultDataReaderQos(qos);

            DataReader otherReader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(otherReader);

            qos = CreateNonDefaultDataReaderQos();
            result = otherReader.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestDefaultDataReaderQos(qos);

            // Create an inconsistent QoS and try to set it
            qos = CreateNonDefaultDataReaderQos();
            qos.TimeBasedFilter.MinimumSeparation = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };
            result = subscriber.SetDefaultDataReaderQos(qos);
            Assert.AreEqual(ReturnCode.InconsistentPolicy, result);
        }

        [TestMethod]
        [TestCategory("Subscriber")]
        public void TestLookupDataReader()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestLookupDataReader), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.GetListener());
            Assert.AreEqual(nameof(TestLookupDataReader), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            Subscriber otherSubscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(otherSubscriber);

            // Create a DataReader and lookup in the subscribers
            DataReader datareader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(datareader);
            Assert.AreEqual(subscriber, datareader.Subscriber);
            Assert.IsNull(datareader.GetListener());

            DataReader received = subscriber.LookupDataReader(nameof(TestLookupDataReader));
            Assert.IsNotNull(received);
            Assert.AreEqual(datareader, received);

            received = otherSubscriber.LookupDataReader(nameof(TestLookupDataReader));
            Assert.IsNull(received);

            // Create other DataReader in the same topic and lookup again
            DataReader otherDatareader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(otherDatareader);
            Assert.AreEqual(subscriber, otherDatareader.Subscriber);
            Assert.IsNull(otherDatareader.GetListener());

            received = subscriber.LookupDataReader(nameof(TestLookupDataReader));
            Assert.IsNotNull(received);
            Assert.IsTrue(datareader == received || otherDatareader == received);

            received = otherSubscriber.LookupDataReader(nameof(TestLookupDataReader));
            Assert.IsNull(received);
        }

        [TestMethod]
        [TestCategory("Subscriber")]
        public void TestCreateDataReaderr()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestCreateDataReaderr), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.GetListener());
            Assert.AreEqual(nameof(TestCreateDataReaderr), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            // Test simplest overload
            DataReader datareader1 = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(datareader1);
            Assert.AreEqual(subscriber, datareader1.Subscriber);
            Assert.IsNull(datareader1.GetListener());

            DataReaderQos qos = new DataReaderQos();
            result = datareader1.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestDefaultDataReaderQos(qos);

            // Test overload with QoS parameter
            qos = CreateNonDefaultDataReaderQos();
            DataReader datareader2 = subscriber.CreateDataReader(topic, qos);
            Assert.IsNotNull(datareader2);
            Assert.AreEqual(subscriber, datareader2.Subscriber);
            Assert.IsNull(datareader2.GetListener());

            qos = new DataReaderQos();
            result = datareader2.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestNonDefaultDataReaderQos(qos);

            // Test overload with listener parameter
            MyDataReaderListener listener = new MyDataReaderListener();
            DataReader datareader3 = subscriber.CreateDataReader(topic, listener);
            Assert.IsNotNull(datareader3);
            Assert.AreEqual(subscriber, datareader3.Subscriber);
            MyDataReaderListener received = (MyDataReaderListener)datareader3.GetListener();
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            qos = new DataReaderQos();
            result = datareader3.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestDefaultDataReaderQos(qos);

            // Test overload with listener and StatusMask parameters
            listener = new MyDataReaderListener();
            DataReader datareader4 = subscriber.CreateDataReader(topic, listener, StatusMask.AllStatusMask);
            Assert.IsNotNull(datareader4);
            Assert.AreEqual(subscriber, datareader4.Subscriber
);
            received = (MyDataReaderListener)datareader4.GetListener();
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            qos = new DataReaderQos();
            result = datareader4.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestDefaultDataReaderQos(qos);

            // Test overload with QoS and listener parameters
            qos = CreateNonDefaultDataReaderQos();
            listener = new MyDataReaderListener();
            DataReader datareader5 = subscriber.CreateDataReader(topic, qos, listener);
            Assert.IsNotNull(datareader5);
            Assert.AreEqual(subscriber, datareader5.Subscriber);
            received = (MyDataReaderListener)datareader5.GetListener();
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            qos = new DataReaderQos();
            result = datareader5.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestNonDefaultDataReaderQos(qos);

            // Test full call overload
            qos = CreateNonDefaultDataReaderQos();
            listener = new MyDataReaderListener();
            DataReader datareader6 = subscriber.CreateDataReader(topic, qos, listener, StatusMask.AllStatusMask);
            Assert.IsNotNull(datareader6);
            Assert.AreEqual(subscriber, datareader6.Subscriber);
            received = (MyDataReaderListener)datareader6.GetListener();
            Assert.IsNotNull(received);
            Assert.AreEqual(listener, received);

            qos = new DataReaderQos();
            result = datareader6.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestNonDefaultDataReaderQos(qos);
        }

        [TestMethod]
        [TestCategory("Subscriber")]
        public void TestDeleteDataReader()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestDeleteDataReader), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.GetListener());
            Assert.AreEqual(nameof(TestDeleteDataReader), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            Subscriber otherSubscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(otherSubscriber);

            // Create a DataReader and try to delete it with another subscriber
            DataReader datareader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(datareader);
            Assert.AreEqual(subscriber, datareader.Subscriber);
            Assert.IsNull(datareader.GetListener());

            DataReaderQos qos = new DataReaderQos();
            result = datareader.GetQos(qos);
            Assert.AreEqual(ReturnCode.Ok, result);
            TestDefaultDataReaderQos(qos);

            result = otherSubscriber.DeleteDataReader(datareader);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            // Delete the datareader with the correct subscriber
            result = subscriber.DeleteDataReader(datareader);
            Assert.AreEqual(ReturnCode.Ok, result);

            // Try to remove it again
            result = subscriber.DeleteDataReader(datareader);
            Assert.AreEqual(ReturnCode.Error, result);
        }

        [TestMethod]
        [TestCategory("Subscriber")]
        public void TestDeleteContainedEntities()
        {
            // Initialize entities
            TestStructTypeSupport support = new TestStructTypeSupport();
            string typeName = support.GetTypeName();
            ReturnCode result = support.RegisterType(_participant, typeName);
            Assert.AreEqual(ReturnCode.Ok, result);

            Topic topic = _participant.CreateTopic(nameof(TestDeleteContainedEntities), typeName);
            Assert.IsNotNull(topic);
            Assert.IsNull(topic.GetListener());
            Assert.AreEqual(nameof(TestDeleteContainedEntities), topic.Name);
            Assert.AreEqual(typeName, topic.TypeName);

            Subscriber subscriber = _participant.CreateSubscriber();
            Assert.IsNotNull(subscriber);

            // Call DeleteContainedEntities in an empty subscriber
            result = subscriber.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);

            // Create a DataWriter in the publisher
            DataReader dataReader = subscriber.CreateDataReader(topic);
            Assert.IsNotNull(subscriber);

            // Try to delete the publisher without delete the datareader
            result = _participant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.PreconditionNotMet, result);

            // Call DeleteContainedEntities and remove the subscriber again
            result = subscriber.DeleteContainedEntities();
            Assert.AreEqual(ReturnCode.Ok, result);

            result = _participant.DeleteSubscriber(subscriber);
            Assert.AreEqual(ReturnCode.Ok, result);
        }
        #endregion

        #region Private Methods
        private DataReaderQos CreateNonDefaultDataReaderQos()
        {
            DataReaderQos qos = new DataReaderQos();
            
            qos.Deadline.Period = new Duration
            {
                Seconds = 5,
                NanoSeconds = 0
            };
            qos.DestinationOrder.Kind = DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos;
            qos.Durability.Kind = DurabilityQosPolicyKind.TransientLocalDurabilityQos;
            qos.History.Depth = 5;
            qos.History.Kind = HistoryQosPolicyKind.KeepAllHistoryQos;
            qos.LatencyBudget.Duration = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };            
            qos.Liveliness.Kind = LivelinessQosPolicyKind.ManualByParticipantLivelinessQos;
            qos.Liveliness.LeaseDuration = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };
            qos.Ownership.Kind = OwnershipQosPolicyKind.ExclusiveOwnershipQos;
            qos.ReaderDataLifecycle.AutopurgeDisposedSamplesDelay = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };
            qos.ReaderDataLifecycle.AutopurgeNowriterSamplesDelay = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };
            qos.Reliability.Kind = ReliabilityQosPolicyKind.ReliableReliabilityQos;
            qos.Reliability.MaxBlockingTime = new Duration
            {
                Seconds = 5,
                NanoSeconds = 5
            };            
            qos.ResourceLimits.MaxInstances = 5;
            qos.ResourceLimits.MaxSamples = 5;
            qos.ResourceLimits.MaxSamplesPerInstance = 5;
            qos.TimeBasedFilter.MinimumSeparation = new Duration
            {
                Seconds = 3,
                NanoSeconds = 3
            };
            qos.UserData.Value = new List<byte> { 0x5 };           

            return qos;
        }

        private void TestDefaultDataReaderQos(DataReaderQos qos)
        {
            Assert.IsNotNull(qos);
            Assert.IsNotNull(qos.Deadline);
            Assert.IsNotNull(qos.DestinationOrder);
            Assert.IsNotNull(qos.Durability);                  
            Assert.IsNotNull(qos.History);
            Assert.IsNotNull(qos.LatencyBudget);                
            Assert.IsNotNull(qos.Liveliness);
            Assert.IsNotNull(qos.Ownership);
            Assert.IsNotNull(qos.ReaderDataLifecycle);
            Assert.IsNotNull(qos.Reliability);
            Assert.IsNotNull(qos.ResourceLimits);
            Assert.IsNotNull(qos.TimeBasedFilter);
            Assert.IsNotNull(qos.UserData);                     
            Assert.IsNotNull(qos.Deadline.Period);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.Deadline.Period.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.Deadline.Period.NanoSeconds);
            Assert.AreEqual(DestinationOrderQosPolicyKind.ByReceptionTimestampDestinationOrderQos, qos.DestinationOrder.Kind);
            Assert.AreEqual(DurabilityQosPolicyKind.VolatileDurabilityQos, qos.Durability.Kind);
            Assert.AreEqual(HistoryQosPolicyKind.KeepLastHistoryQos, qos.History.Kind);
            Assert.AreEqual(1, qos.History.Depth);
            Assert.IsNotNull(qos.LatencyBudget.Duration);
            Assert.AreEqual(Duration.ZeroSeconds, qos.LatencyBudget.Duration.Seconds);
            Assert.AreEqual(Duration.ZeroNanoseconds, qos.LatencyBudget.Duration.NanoSeconds);            
            Assert.AreEqual(LivelinessQosPolicyKind.AutomaticLivelinessQos, qos.Liveliness.Kind);
            Assert.IsNotNull(qos.Liveliness.LeaseDuration);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.Liveliness.LeaseDuration.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.Liveliness.LeaseDuration.NanoSeconds);
            Assert.AreEqual(OwnershipQosPolicyKind.SharedOwnershipQos, qos.Ownership.Kind);
            Assert.IsNotNull(qos.ReaderDataLifecycle.AutopurgeDisposedSamplesDelay);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.ReaderDataLifecycle.AutopurgeDisposedSamplesDelay.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.ReaderDataLifecycle.AutopurgeDisposedSamplesDelay.NanoSeconds);
            Assert.IsNotNull(qos.ReaderDataLifecycle.AutopurgeNowriterSamplesDelay);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.ReaderDataLifecycle.AutopurgeNowriterSamplesDelay.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.ReaderDataLifecycle.AutopurgeNowriterSamplesDelay.NanoSeconds);
            Assert.AreEqual(ReliabilityQosPolicyKind.BestEffortReliabilityQos, qos.Reliability.Kind);
            Assert.IsNotNull(qos.Reliability.MaxBlockingTime);
            Assert.AreEqual(Duration.InfiniteSeconds, qos.Reliability.MaxBlockingTime.Seconds);
            Assert.AreEqual(Duration.InfiniteNanoseconds, qos.Reliability.MaxBlockingTime.NanoSeconds);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.ResourceLimits.MaxInstances);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.ResourceLimits.MaxSamples);
            Assert.AreEqual(ResourceLimitsQosPolicy.LengthUnlimited, qos.ResourceLimits.MaxSamplesPerInstance);
            Assert.IsNotNull(qos.TimeBasedFilter.MinimumSeparation);
            Assert.AreEqual(0, qos.TimeBasedFilter.MinimumSeparation.Seconds);
            Assert.AreEqual((uint)0, qos.TimeBasedFilter.MinimumSeparation.NanoSeconds);
            Assert.IsNotNull(qos.UserData.Value);
            Assert.AreEqual(0, qos.UserData.Value.Count());            
        }

        private void TestNonDefaultDataReaderQos(DataReaderQos qos)
        {
            Assert.IsNotNull(qos);
            Assert.IsNotNull(qos.Deadline);
            Assert.IsNotNull(qos.DestinationOrder);
            Assert.IsNotNull(qos.Durability);            
            Assert.IsNotNull(qos.History);
            Assert.IsNotNull(qos.LatencyBudget);            
            Assert.IsNotNull(qos.Liveliness);
            Assert.IsNotNull(qos.Ownership);
            Assert.IsNotNull(qos.ReaderDataLifecycle);
            Assert.IsNotNull(qos.Reliability);
            Assert.IsNotNull(qos.ResourceLimits);
            Assert.IsNotNull(qos.TimeBasedFilter);
            Assert.IsNotNull(qos.UserData);            

            Assert.IsNotNull(qos.Deadline.Period);
            Assert.AreEqual(5, qos.Deadline.Period.Seconds);
            Assert.AreEqual(Duration.ZeroNanoseconds, qos.Deadline.Period.NanoSeconds);
            Assert.AreEqual(DestinationOrderQosPolicyKind.BySourceTimestampDestinationOrderQos, qos.DestinationOrder.Kind);
            Assert.AreEqual(DurabilityQosPolicyKind.TransientLocalDurabilityQos, qos.Durability.Kind);
            Assert.AreEqual(HistoryQosPolicyKind.KeepAllHistoryQos, qos.History.Kind);
            Assert.AreEqual(5, qos.History.Depth);
            Assert.IsNotNull(qos.LatencyBudget.Duration);
            Assert.AreEqual(5, qos.LatencyBudget.Duration.Seconds);
            Assert.AreEqual((uint)5, qos.LatencyBudget.Duration.NanoSeconds);
            Assert.AreEqual(LivelinessQosPolicyKind.ManualByParticipantLivelinessQos, qos.Liveliness.Kind);
            Assert.IsNotNull(qos.Liveliness.LeaseDuration);
            Assert.AreEqual(5, qos.Liveliness.LeaseDuration.Seconds);
            Assert.AreEqual((uint)5, qos.Liveliness.LeaseDuration.NanoSeconds);
            Assert.AreEqual(OwnershipQosPolicyKind.ExclusiveOwnershipQos, qos.Ownership.Kind);
            Assert.IsNotNull(qos.ReaderDataLifecycle.AutopurgeDisposedSamplesDelay);
            Assert.AreEqual(5, qos.ReaderDataLifecycle.AutopurgeDisposedSamplesDelay.Seconds);
            Assert.AreEqual((uint)5, qos.ReaderDataLifecycle.AutopurgeDisposedSamplesDelay.NanoSeconds);
            Assert.IsNotNull(qos.ReaderDataLifecycle.AutopurgeNowriterSamplesDelay);
            Assert.AreEqual(5, qos.ReaderDataLifecycle.AutopurgeNowriterSamplesDelay.Seconds);
            Assert.AreEqual((uint)5, qos.ReaderDataLifecycle.AutopurgeNowriterSamplesDelay.NanoSeconds);
            Assert.AreEqual(ReliabilityQosPolicyKind.ReliableReliabilityQos, qos.Reliability.Kind);
            Assert.IsNotNull(qos.Reliability.MaxBlockingTime);
            Assert.AreEqual(5, qos.Reliability.MaxBlockingTime.Seconds);
            Assert.AreEqual((uint)5, qos.Reliability.MaxBlockingTime.NanoSeconds);
            Assert.AreEqual(5, qos.ResourceLimits.MaxInstances);
            Assert.AreEqual(5, qos.ResourceLimits.MaxSamples);
            Assert.AreEqual(5, qos.ResourceLimits.MaxSamplesPerInstance);
            Assert.AreEqual(1, qos.UserData.Value.Count());
            Assert.IsNotNull(qos.TimeBasedFilter.MinimumSeparation);
            Assert.AreEqual(3, qos.TimeBasedFilter.MinimumSeparation.Seconds);
            Assert.AreEqual((uint)3, qos.TimeBasedFilter.MinimumSeparation.NanoSeconds);
            Assert.AreEqual(0x5, qos.UserData.Value.First());
        }
        #endregion
    }
}
