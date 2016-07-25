using System.Collections.Generic;
using System.Linq;

namespace DevStats.Data.Repositories
{
    public class OriginsRepository : BaseRepository
    {
        public IEnumerable<string> Get()
        {
            return Context.AllowedOrigins.Select(x => x.Origin).ToList();
        }
    }
}