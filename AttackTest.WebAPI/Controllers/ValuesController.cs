using HackerSpray.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace AttackTest.WebAPI.Controllers
{
    public class ValuesController : ApiController
    {
        public static int MaxValidLogin = 3;
        public static TimeSpan MaxValidLoginInterval = TimeSpan.FromMinutes(10);

        public static int MaxInvalidLogin = 3;
        public static TimeSpan MaxInvalidLoginInterval = TimeSpan.FromMinutes(15);

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public async Task<IHttpActionResult> Get(int id)
        {
            var originIP = "0.0.0.1";
            
            var ip = IPAddress.Parse(originIP);

            return await Hacker.DefendAsync<IHttpActionResult>(
                async (success, fail) =>
            {
                if (id == 5)
                {

                    return await success(Ok($"sms for id {id}"));
                }
                else
                {

                    return await fail(BadRequest($"bad request for id {id}"));
                }
            },
               blocked => new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.Forbidden)),
               validActionKey: "ValidLogin:" + id, maxValidAttempt: MaxValidLogin, validAttemptInterval: MaxValidLoginInterval,
               invalidActionKey: "InvalidLogin:" + id, maxInvalidAttempt: MaxInvalidLogin, invalidAttemptInterval: MaxInvalidLoginInterval,
               origin: ip
           );

        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
