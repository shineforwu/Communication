using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConsoleHostWebApi.Controllers
{
    public class TestController : ApiController
    {

        // GET api/<controller>/5
        [HttpGet]
        public Object Get()
        {


            return "{\r\n  \"Name\": \"ABC\"\r\n}";
        }

        // POST api/<controller>
        [HttpPost]
        public Object Post(Object value)
        {
            return value.ToString();
        }

        // PUT api/<controller>/5
        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}
