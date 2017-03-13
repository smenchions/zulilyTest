using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zulilySurvey.Entities;

namespace zulilySurvey.Data
{

    public interface ISurveyRepository
    {
        Survey InsertOrReplace(Survey entity);
        Survey GetById(ObjectId Id);
        List<Survey> GetById(List<ObjectId> Id);
        void DeleteById(ObjectId Id);
        List<Survey> GetByGender(Gender Gender);
        List<Survey> GetByAge(AgeRange Range);
        List<Survey> GetByAgeAndGender(AgeRange? Range, Gender? Gender);
        Survey GetByUserId(ObjectId UserId);
    }

    public class DbSurveyRepository : CoreMongoRepository<Survey>, ISurveyRepository
    {

        public Survey GetByUserId(ObjectId UserId)
        {
            //this assumes that only one survey per user
            var collection = GetCollection<Survey>();
            return (from p in collection.AsQueryable() where p.UserId == UserId select p).FirstOrDefault();
        }
        /// <summary>
        /// Get survey templates by Gender
        /// </summary>
        /// <param name="Gender"></param>
        /// <returns></returns>
        public List<Survey> GetByGender(Gender Gender)
        {
            return GetByAgeAndGender(null, Gender);
        }

        /// <summary>
        /// get survey templates by Age
        /// </summary>
        /// <param name="Range"></param>
        /// <returns></returns>
        public List<Survey> GetByAge(AgeRange Range)
        {
            return GetByAgeAndGender(Range, null);
        }

        /// <summary>
        /// get Survey templates by age and gender
        /// </summary>
        /// <param name="Range"></param>
        /// <param name="Gender"></param>
        /// <returns></returns>
        public List<Survey> GetByAgeAndGender(AgeRange? Range, Gender? Gender)
        {
            var collection = GetCollection<Survey>();
            var query = from p in collection.AsQueryable() where p.Type == SurveyType.Template select p;
            if (Gender.HasValue)
                query = query.Where(t => t.Gender == Gender.Value);
            if (Range.HasValue)
                query = query.Where(t => t.AgeRange == Range.Value);

            return query.ToList();
        }
    }

    public class FakeSurveyRepository : CoreFakeMongoRepository<Survey>, ISurveyRepository
    {
        public List<Survey> GetByAge(AgeRange Range)
        {
            throw new NotImplementedException();
        }

        public List<Survey> GetByAgeAndGender(AgeRange? Range, Gender? Gender)
        {
            throw new NotImplementedException();
        }

        public List<Survey> GetByGender(Gender Gender)
        {
            throw new NotImplementedException();
        }

        public List<Survey> GetById(List<ObjectId> Id)
        {
            throw new NotImplementedException();
        }

        public Survey GetByUserId(ObjectId UserId)
        {
            throw new NotImplementedException();
        }
    }
}
