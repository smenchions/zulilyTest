using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using zulilySurvey.Entities;

namespace zulilySurvey.Controllers
{
    [Route("api/[controller]")]
    // We handle all exceptions thrown from the C# code with this custom attribute.  The fron end JS will then show the exception message in an error
    // bar. Will also return as 400
    [CustomExceptionFilterAttribute]
    public class SurveyController : Controller
    {


        /// GET api/survey/{id}
        /// <summary>
        /// Gets a survey with the given id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public JsonResult GetSurvey(String Id)
        {
            var Survey = Entities.Survey.Repository.GetById(new ObjectId(Id));
            return new JsonResult(Survey);
        }


        /// GET api/survey/{survey}
        /// <summary>
        /// Gets a survey template which matches the posted survey's Age and gender settings
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSurvey([FromBody] Survey Data)
        {  
            // The following is a hack to ensure users only gets survey template data for two
            // different age ranges. We only have data in the DB for two Age ranges due to time constraints
            if (Data.AgeRange != AgeRange.Range0to4 && Data.AgeRange != AgeRange.Range30to45)
            {
                if (Data.AgeRange == AgeRange.Range5to16)
                    Data.AgeRange = AgeRange.Range0to4;
                else
                    Data.AgeRange = AgeRange.Range30to45;
            }
                
            var Survey = Entities.Survey.Repository.GetByAgeAndGender(Data.AgeRange, Data.Gender);
            return new JsonResult(Survey);
        }

        /// GET api/survey/user/{id}
        /// <summary>
        /// Gets a survey with the given UserId
        /// </summary>
        /// <returns></returns>
        [HttpGet("user/{Id}")]
        public JsonResult GetSurveyByUser(String Id)
        {
            var Survey = Entities.Survey.Repository.GetByUserId(new ObjectId(Id));
            return new JsonResult(Survey);
        }


        /// PUT api/survey/
        /// <summary>
        /// Inserts or Replaces a survey
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPut]
        public JsonResult SaveSurvey([FromBody] Survey Data)
        {
            Entities.Survey.Repository.InsertOrReplace(Data);
            return new JsonResult(Data);
        }


        /// DELETE api/survey/
        /// <summary>
        /// Deletes a survey by Id
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpDelete]
        public JsonResult DeleteSurvey([FromBody] Survey Data)
        {
            Entities.Survey.Repository.DeleteById(Data.Id);
            return new JsonResult(Data);
        }

    }
}
