using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zulilySurvey.Data;

namespace zulilySurvey.Entities
{
    public class User
    {
        public User()
        {
            Id = ObjectId.GenerateNewId();
            DateCreated = DateTime.UtcNow;
        }

        public static IUserRepository Repository
        {
            get
            {
                if (RepositoryFactory.IsFake)
                    return new FakeUserRepository();
                else
                    return new DbUserRepository();
            }
        }

        public ObjectId Id { get; set; }
        //TODO: this concept of having string versions of the ObjectId can be imprvoed with better MVC .NET Mapping
        [BsonIgnore]
        public String sId
        {
            get { return Id.ToString(); }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    this.Id = new ObjectId(value);
            }
        }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
