using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        [HttpGet]
        public Object Download(string filePath)//Download file 
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            var stream = new FileStream(filePath, FileMode.Open);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(stream)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            return response;
        }
        [HttpGet]
        public async Task<HttpResponseMessage> Download2(string filePath)//Download file 
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            var stream = new FileStream(filePath, FileMode.Open);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(stream)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            return response;
        }
    }
}
