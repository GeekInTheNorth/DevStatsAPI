using System.Web.Http;
using DevStats.Data.Repositories;
using DevStats.Domain.Burndown;

namespace DevStats.Controllers
{
    public class BurndownController : ApiController
    {
        private readonly IBurndownRepository repository;

        public BurndownController()
        {
            repository = new BurndownRepository();
        }

        [HttpGet]
        public string Check()
        {
            return "Hello World!";
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