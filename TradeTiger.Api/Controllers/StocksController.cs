using ClassLibrary2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TradeTiger.Api.Controllers
{
    public class StocksController : ApiController
    {
        // GET api/values
        public IEnumerable<Nifty> Get()
        {
            Stocks stks = new Stocks();
            return stks.GetNiftyData();
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
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
