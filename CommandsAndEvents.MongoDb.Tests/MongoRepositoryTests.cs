using CommandsAndEvents.MongoDb.Tests.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CommandsAndEvents.MongoDb.Tests
{
    [TestClass]
    public class MongoRepositoryTests
    {
        private Guid _aggregateId;

        [TestInitialize]
        public void Initialize()
        {
            _aggregateId = Guid.Parse("6de6fa63-e760-435d-aa73-2c3e1342625a");
        }

        [TestMethod]
        public void TestMethod1()
        {
            var a = new MongoAggregate();
            a.Id = _aggregateId;
            a.Value = "test value";
            MongoDbRepository<MongoAggregate>.Instance.Save(a);
            var readItemTask = MongoDbRepository<MongoAggregate>.Instance.Get(_aggregateId);
            //readItemTask.Wait();
            Assert.IsNotNull(readItemTask);
        }
    }
}
