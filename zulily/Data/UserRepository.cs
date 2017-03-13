using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zulilySurvey.Entities;

namespace zulilySurvey.Data
{

    public interface IUserRepository
    {
        User InsertOrReplace(User Entity);
        User GetByEmail(String Email);
        User GetById(ObjectId Id);
        List<User> GetById(List<ObjectId> Id);
        void DeleteById(ObjectId Id);
    }

    public class DbUserRepository : CoreMongoRepository<User>, IUserRepository
    {
        public User GetByEmail(string Email)
        {
            var collection = GetCollection<User>();
            return (from p in collection.AsQueryable() where p.Email == Email select p).FirstOrDefault();
        }
    }

    public class FakeUserRepository : CoreFakeMongoRepository<User>, IUserRepository
    {
        public User GetByEmail(string Email)
        {
            throw new NotImplementedException();
        }

        public List<User> GetById(List<ObjectId> Id)
        {
            throw new NotImplementedException();
        }
    }
}
