using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using zulilySurvey.Data;
using zulilySurvey.Entities;
using Microsoft.AspNetCore.Http;

namespace zulilySurvey.Controllers
{
    [Route("api/[controller]")]
    // We handle all exceptions thrown from the C# code with this custom attribute.  The front end JS will then show the exception message in an error
    // bar. Returns as 400
    [CustomExceptionFilterAttribute]
    public class UserController : Controller
    {

        // GET api/user/{id}
        /// <summary>
        /// Gets a user with the given id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public JsonResult GetUser(String Id)
        {
            var User = Entities.User.Repository.GetById(new ObjectId(Id));
            SaveUserCookie(User);
            return new JsonResult(User);
        }


        // GET api/user/email/{address}
        /// <summary>
        /// Gets a user with the given id
        /// </summary>
        /// <returns></returns>
        [HttpGet("email/{Email}")]
        public JsonResult GetUserByEmail(String Email)
        {
            var User = Entities.User.Repository.GetByEmail(Email);
            SaveUserCookie(User);
            return new JsonResult(User);
        }


        /// PUT api/user/
        /// <summary>
        /// Updates or Replaces a user. Finds if an existing user has the email address
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPut]
        public JsonResult SaveUser([FromBody] User Data)
        {
            User Result = Entities.User.Repository.InsertOrReplace(Data);
            SaveUserCookie(Result);
            return new JsonResult(Result);
        }


        private void SaveUserCookie (User User)
        {
            if (User != null && !String.IsNullOrEmpty(User.sId))
            {
                var options = new CookieOptions()
                {
                    Expires = DateTime.UtcNow.AddDays(30),
                };
                HttpContext.Response.Cookies.Append("User", User.sId, options);
            }
        }
    }
}
