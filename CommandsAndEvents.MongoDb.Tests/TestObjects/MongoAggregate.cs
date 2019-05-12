using System;
using System.Collections.Generic;
using System.Text;

namespace CommandsAndEvents.MongoDb.Tests.TestObjects
{
    public class MongoAggregate : AggregateRoot
    {
        public string Value { get; set; }
    }
}
