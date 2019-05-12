using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommandsAndEvents.MongoDb
{
    public class MongoDbRepository<T>
        where T : AggregateRoot
    {
        public MongoDbRepository() : this("mongodb://127.0.0.1:27018", "CommandsAndEvents") { }

        public MongoDbRepository(string connectionString, string databaseName)
        {
            _mongoDbClient = new MongoClient(connectionString);
            _mongoDatabase = _mongoDbClient.GetDatabase(databaseName);

            BsonClassMap.RegisterClassMap<AggregateRoot>(classMap =>
            {
                classMap.AutoMap();
                classMap.MapIdMember(x => x.Id);
                classMap.UnmapMember(x => x.DomainEvents);
            });
            BsonClassMap.RegisterClassMap<T>(classMap =>
            {
                classMap.AutoMap();
            });
        }

        private static MongoDbRepository<T> _instance;
        private static readonly object _syncRoot = new object();
        public static MongoDbRepository<T> Instance
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new MongoDbRepository<T>();
                    return _instance;
                }
            }
        }
        
        private MongoClient _mongoDbClient { get; }
        private IMongoDatabase _mongoDatabase { get; }

        public async void SaveAsync(T aggregate)
        {
            var coll = _mongoDatabase.GetCollection<T>(typeof(T).Name);
            var existingItem = await coll.FindAsync(x => x.Id == aggregate.Id);
            if (existingItem.Any())
            {
                await coll.DeleteOneAsync(x => x.Id == aggregate.Id);    
            }
            await coll.InsertOneAsync(aggregate);
        }

        public void Save(T aggregate)
        {
            var coll = _mongoDatabase.GetCollection<T>(typeof(T).Name);
            var existingItem = coll.Find(x => x.Id == aggregate.Id);
            if (existingItem.Any())
            {
                coll.DeleteOne(x => x.Id == aggregate.Id);
            }
            coll.InsertOne(aggregate);
        }

        public async Task<T> GetAsync(Guid id)
        {
            var coll = _mongoDatabase.GetCollection<T>(typeof(T).Name);
            var item = await coll.FindAsync(x => x.Id == id);
            return item.FirstOrDefault();
        }

        public T Get(Guid id)
        {
            var coll = _mongoDatabase.GetCollection<T>(typeof(T).Name);
            return coll.Find(x => x.Id == id).FirstOrDefault();
        }
    
    }
}
