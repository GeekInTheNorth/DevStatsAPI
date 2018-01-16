using System;
using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.Reports.TaskingStatus;

namespace DevStats.Data.Repositories
{
    public class EstimationRepository : BaseRepository, IEstimationRepository
    {
        private class Story
        {
            public string TShirtSize;

            public decimal ActualTime;
        }

        public Dictionary<string, decimal> GetTShirtAverages()
        {
            var stories = (from story in Context.WorkLogStories
                           select new Story
                           {
                               TShirtSize = story.TShirtSize,
                               ActualTime = story.ActualTimeInSeconds
                           }).ToList();

            var tShirtAverages = (from story in stories
                               group story by story.TShirtSize into storiesByTShirt
                               select new
                               {
                                   TShirtSize = storiesByTShirt.Key,
                                   ActualTime = storiesByTShirt.Sum(x => x.ActualTime),
                                   Count = storiesByTShirt.Count()
                               }).ToDictionary(keySelector: x => x.TShirtSize, elementSelector: x => Math.Round((x.ActualTime / 3600), 2) / x.Count);

            return tShirtAverages;
        }
    }
}