using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zulilySurvey.Entities
{

    public class Image
    {
        public Image()
        {
            Id = ObjectId.GenerateNewId();
            DateCreated = DateTime.UtcNow;
            Tags = new List<string>();
        }

        public Image( params String[] Tags)
        {
            Id = ObjectId.GenerateNewId();
            DateCreated = DateTime.UtcNow;
            this.Tags = new List<string>();
            foreach (var Tag in Tags)
            {
                this.Tags.Add(Tag);
            }
        }
        public ObjectId Id { get; set; }
        public String sId
        {
            get { return Id.ToString(); }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    this.Id = new ObjectId(value);
            }
        }
        public string Description { get; set; }
        public Uri Url { get; set; }
        public List<String> Tags { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
