namespace DevStats.Data.Repositories
{
    public class BaseRepository
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
    }
}
