using System.Web.Http;
using DevStats.Attributes;
using DevStats.Data.Repositories;
using DevStats.Domain.Burndown;

namespace DevStats.Controllers.API
{
    [AllowCors("*", "GET,POST")]
    public class BurndownController : ApiController
    {
        private readonly IBurndownRepository repository;

        public BurndownController(IBurndownRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public BurndownSummary Get([FromUri]string sprint)
        {
            return repository.Get(sprint);
        }

        [HttpPost]
        public bool Save([FromBody]BurndownDay day)
        {
            try
            {
                repository.Save(day);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}