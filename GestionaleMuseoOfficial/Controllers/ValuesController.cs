using GestionaleMuseoOfficial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GestionaleMuseoOfficial.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
        //API GetBigliettiPerId
        [Route("api/GetBigliettiPerId/{idApi}")]
        public IEnumerable<Biglietti> GetBigliettiPerId(int idApi)
        {
            ModelDbContext dbContext = new ModelDbContext();
            List<Biglietti> listaBiglietti = new List<Biglietti>();
            listaBiglietti = dbContext.Biglietti.Where(b => b.IdVisita == idApi).ToList();
            return listaBiglietti;
        }

        [Route("api/GetTotBigliettiPerCategoria/{id}")]
        public int GetTotBigliettiPerCategoria(int id)
        {
            ModelDbContext dbContext = new ModelDbContext();
            List<Biglietti> listaBiglietti = new List<Biglietti>();
            listaBiglietti = dbContext.Biglietti.Where(b => b.IdCategorieVisitatori == id).ToList();
            int totaleBiglietti = listaBiglietti.Count();
            return totaleBiglietti;
        }

    }
}
