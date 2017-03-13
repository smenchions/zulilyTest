using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace zulilySurvey.Data
{


    public class RepositoryFactory
    {

        /// <summary>
        /// If true, will remove the connections to a real mongo DB and use an in memory list for testing purposes to back the Repository
        /// </summary>
        public static bool IsFake
        {
            get; set;
        }
        /// <summary>
        /// Connection string for mongo in the form of mongodb://domain.com:27017/
        /// Initailized on app startup within Startup.cs. Read from appsettings.json
        /// </summary>
        public static string _connectionString;
        /// <summary>
        /// Database name string for mongo in the form of nameforyourdatabase
        /// Initailized on app startup within Startup.cs. Read from appsettings.json
        /// </summary>
        public static string _databaseName;

    }

    /// <summary>
    /// A class which creates the base of all repositories which connect with MongoDB.  
    /// </summary>
    /// <typeparam name="T"></typeparam>
       public abstract class CoreMongoRepository<T> where T : class
       {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _db;

        public CoreMongoRepository()
        {
            if (String.IsNullOrEmpty(RepositoryFactory._connectionString) || String.IsNullOrEmpty(RepositoryFactory._databaseName))
                throw new ArgumentNullException("Connection String and/or database name not correctly set for MongoDB");
            _client = new MongoClient(RepositoryFactory._connectionString);
            _db = _client.GetDatabase(RepositoryFactory._databaseName);
        }

        protected ObjectId GetId(T entity)
        {
            Type type = typeof(T);
            string name = type.Name.Split('.').Last();

            PropertyInfo idProperty = type.GetProperty("Id");
            if (idProperty == null)
            {
                throw new Exception("The required property: " + name + "Id was not found");
            }
            ObjectId idValue = (ObjectId)idProperty.GetValue(entity, null);
            return idValue;
        }


        public IMongoCollection<TEntity> GetCollection<TEntity>()
        {
            return _db.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual T InsertOrReplace(T entity)
        {
            var collection = GetCollection<T>();
            var filter = Builders<T>.Filter.Eq("_id", BsonValue.Create(GetId(entity)));
            collection.ReplaceOne(filter, entity, new UpdateOptions { IsUpsert = true });
            return entity;
        }

        public void DeleteById(ObjectId Id)
        {
            var collection = GetCollection<T>();
            var filter = Builders<T>.Filter.Eq("_id", BsonValue.Create(Id));
            collection.DeleteOne(filter);
        }

        public virtual T GetById(ObjectId Id)
        {
            var collection = GetCollection<T>();
            var filter = Builders<T>.Filter.Eq("_id", BsonValue.Create(Id));
            var entity = collection.Find<T>(filter).FirstOrDefault();
            return entity;
        }

        public virtual List<T> GetById(List<ObjectId> Id)
        {
            var collection = GetCollection<T>();
            var IdArray = new BsonArray();
            foreach (ObjectId singleId in Id)
            {
                IdArray.Add(BsonValue.Create(singleId));
            }
            var filter = Builders<T>.Filter.In<BsonValue>("_id", IdArray);

            var entity = collection.Find<T>(filter);
            return entity.ToList();
        }
    }


    public abstract class CoreFakeMongoRepository<T> where T : class
    {
        protected static Dictionary<ObjectId, T> entities = new Dictionary<ObjectId, T>();

        protected ObjectId GetId(T entity)
        {
            Type type = typeof(T);
            string name = type.Name.Split('.').Last();

            PropertyInfo idProperty = type.GetProperty("Id");
            if (idProperty == null)
            {
                throw new Exception("The required property: " + name + "Id was not found");
            }
            ObjectId idValue = (ObjectId)idProperty.GetValue(entity, null);
            return idValue;
        }

        public virtual T InsertOrReplace(T entity)
        {
            ObjectId Id = GetId(entity);
            T existing = GetById(GetId(entity));
            if (existing != null)
            {
                entities.Remove(Id);
            }
            entities.Add(Id, entity);
            return entity;
        }

        public virtual T GetById(ObjectId id)
        {
            if (entities.ContainsKey(id))
            {
                return entities[id];
            }
            return null;
        }

        public virtual void DeleteById(ObjectId id)
        {
            entities.Remove(id);
        }

        public void Clear()
        {
            entities.Clear();
        }

    }
}
