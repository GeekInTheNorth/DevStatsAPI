using System.Linq;
using DevStats.Domain.Burndown;

namespace DevStats.Data.Repositories
{
    public class BurndownRepository : BaseRepository, IBurndownRepository
    {
        public BurndownSummary Get(string sprintName)
        {
            var burndownDays = (from days in Context.BurndownDays
                                where days.Sprint == sprintName
                                select new BurndownDay
                                {
                                    Date = days.Date,
                                    Sprint = days.Sprint,
                                    WorkRemaining = days.WorkRemaining
                                }).ToList();

            return new BurndownSummary(burndownDays);
        }

        public void Save(BurndownDay burndownDay)
        {
            var existingItem = Context.BurndownDays.FirstOrDefault(x => x.Sprint == burndownDay.Sprint && x.Date == burndownDay.Date);

            if (existingItem == null)
            {
                var newEntity = new Entities.BurndownDay
                {
                    Sprint = burndownDay.Sprint,
                    Date = burndownDay.Date,
                    WorkRemaining = burndownDay.WorkRemaining
                };

                Context.BurndownDays.Add(newEntity);
            }
            else
            {
                existingItem.WorkRemaining = burndownDay.WorkRemaining;
            }

            Context.SaveChanges();
        }
    }
}