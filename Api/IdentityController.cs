using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api
{
    [Route("identity")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            //return new JsonResult(from c in User?.Claims select new { c.Type, c.Value });
            return new JsonResult(User?.Claims?.Select(c => new { c.Type, c.Value }));
        }

        [HttpPost]
        public IActionResult Post([FromBody] PostModel model)
        {
            return Ok(model);
        }

        public class PostModel
        {   
            public class Claim
            {
                public string Type { get; set; }
                public string Value { get; set; }
            }

            public IEnumerable<Claim> Claims { get; set; }
            public IEnumerable<string> Tags { get; set; }
        }
    }
}
