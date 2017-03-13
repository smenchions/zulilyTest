using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zulilySurvey.Data;

namespace zulilySurvey.Entities
{

    public enum Gender { Female, Male };
    public enum AgeRange { Range0to4, Range5to16, Range17to29, Range30to45, Range46to64, Range65Plus };
    public enum SurveyType { Template, ABTest, UserResult}

    public class Survey
    {
        public Survey ()
        {
            Id = ObjectId.GenerateNewId();
            DateCreated = DateTime.UtcNow;
            Images = new List<Image>();
        }

        public static ISurveyRepository Repository
        {
            get
            {
                if (RepositoryFactory.IsFake)
                    return new FakeSurveyRepository();
                else
                    return new DbSurveyRepository();
            }
        }


        public ObjectId Id { get; set; }
        //TODO: this concept of having string versions of the ObjectId can be imprvoed with better MVC .NET Mapping
        [BsonIgnore]
        public String sId
        {
            get { return Id.ToString(); }
            set {
                if (!String.IsNullOrEmpty(value))
                    this.Id = new ObjectId(value);
            }
        }
        public ObjectId? UserId { get; set; }
        [BsonIgnore]
        public String sUserId
        {
            get { return UserId.ToString(); }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    this.UserId = new ObjectId(value);
            }
        }
        [JsonConverter(typeof(StringEnumConverter))]
        public SurveyType? Type { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender? Gender { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public AgeRange? AgeRange { get; set; }
        public List<Image> Images { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
