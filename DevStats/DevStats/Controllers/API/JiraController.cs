using System.Web.Http;
using System.Web.Http.Cors;
using DevStats.Domain.Jira.WebHookLog;

namespace DevStats.Controllers.API
{
    public class JiraController : ApiController
    {
        private readonly IJiraWebHookLogRepository hookLogRepository;

        public JiraController(IJiraWebHookLogRepository hookLogRepository)
        {
            this.hookLogRepository = hookLogRepository;
        }

        [EnableCors("*", "*", "*")]
        [HttpPost]
        public void CreateDefaultSubTasks([FromUri]string user_id, [FromUri]string user_key, [FromBody]string content)
        {
            hookLogRepository.Save(user_id, user_key, content);
        }
    }
}