using System;

namespace DevStats.Data.Repositories
{
    public class BaseRepository : IDisposable
    {
        private DevStatContext context;

        protected DevStatContext Context
        {
            get
            {
                if (context == null)
                    context = new DevStatContext();

                return context;
            }
        }

        public void Dispose()
        {
            if (context != null)
                context.Dispose();
        }
    }
}