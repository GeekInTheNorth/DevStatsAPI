using System;
using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.Aha;
using DevStats.Domain.DefectAnalysis;

namespace DevStats.Data.Repositories
{
    public class DefectRepository : BaseRepository, IDefectRepository
    {
        public IEnumerable<Defect> Get(DateTime createdFrom, DateTime createdTo)
        {
            return Context.Defects
                          .Where(x => x.Created >= createdFrom && x.Created <= createdTo)
                          .ToList()
                          .Select(x => new Defect(x.JiraId, x.AhaId, x.Module, x.Type, x.Created, x.Closed));
        }

        public void Save(IEnumerable<Defect> defects)
        {
            foreach (var defect in defects)
                Save(defect);

            Context.SaveChanges();
        }

        private void Save(Defect defect)
        {
            var dbItems = Context.Defects.Where(x => x.JiraId == defect.JiraId || x.AhaId == defect.AhaId).ToList();

            if (!dbItems.Any())
            {
                dbItems.Add(new Entities.Defect
                {
                    JiraId = defect.JiraId,
                    AhaId = defect.AhaId,
                    Created = defect.Created,
                    Closed = defect.Closed,
                    Module = defect.Module,
                    Type = defect.Type.ToString()
                });
            }

            var firstItem = dbItems.FirstOrDefault();
            var itemsToRemove = dbItems.Where(x => x.ID != firstItem.ID);

            firstItem.AhaId = GetBestString(defect.AhaId, firstItem.AhaId);
            firstItem.JiraId = GetBestString(defect.JiraId, firstItem.JiraId);
            firstItem.Module = GetBestString(defect.Module, firstItem.Module);
            firstItem.Created = GetFirstCreated(defect.Created, firstItem.Created);
            firstItem.Closed = GetLastClosed(defect.Closed, firstItem.Closed);

            foreach (var itemToRemove in itemsToRemove)
            {
                firstItem.AhaId = GetBestString(firstItem.AhaId, itemToRemove.AhaId);
                firstItem.JiraId = GetBestString(firstItem.JiraId, itemToRemove.JiraId);
                firstItem.Module = GetBestString(firstItem.Module, itemToRemove.Module);
                firstItem.Created = GetFirstCreated(firstItem.Created, itemToRemove.Created);
                firstItem.Closed = GetLastClosed(firstItem.Closed, itemToRemove.Closed);

                Context.Defects.Remove(itemToRemove);
            }

            if (firstItem.Type == DefectType.NonDefect.ToString() && firstItem.ID > 0)
                Context.Defects.Remove(firstItem);
        }

        public void Save(IEnumerable<AhaDefect> defects)
        {
            foreach (var defect in defects)
                Save(defect);

            Context.SaveChanges();
        }

        private void Save(AhaDefect defect)
        {
            var jiraId = string.IsNullOrWhiteSpace(defect.JiraId) ? null : defect.JiraId;
            var ahaId = string.IsNullOrWhiteSpace(defect.AhaId) ? null : defect.AhaId;

            var dbItem = Context.Defects.FirstOrDefault(x => (x.JiraId == jiraId && x.JiraId != null) || (x.AhaId == defect.AhaId && x.AhaId != null));

            if (dbItem == null)
            {
                dbItem = new Entities.Defect
                {
                    JiraId = defect.JiraId,
                    AhaId = defect.AhaId,
                    Created = defect.Created,
                    Closed = defect.Closed,
                    Product = defect.Product,
                    Module = defect.Module,
                    Type = defect.Type.ToString()
                };

                if (defect.Type != DefectType.NonDefect)
                    Context.Defects.Add(dbItem);
            }

            dbItem.AhaId = GetBestString(defect.AhaId, dbItem.AhaId);
            dbItem.JiraId = GetBestString(defect.JiraId, dbItem.JiraId);
            dbItem.Closed = GetLastClosed(defect.Closed, dbItem.Closed);
        }

        private string GetBestString(string value1, string value2)
        {
            value1 = value1 == null || value1.Equals("Unknown", StringComparison.CurrentCultureIgnoreCase) ? null : value1;
            value2 = value2 == null || value2.Equals("Unknown", StringComparison.CurrentCultureIgnoreCase) ? null : value2;

            if (string.IsNullOrWhiteSpace(value1))
                return value2;

            return value1;
        }

        private DateTime GetFirstCreated(DateTime created1, DateTime created2)
        {
            return created1 < created2 ? created1 : created2;
        }

        private DateTime? GetLastClosed(DateTime? closed1, DateTime? closed2)
        {
            if (closed1.HasValue && closed2.HasValue)
                return closed1.Value > closed2.Value ? closed1 : closed2;

            return closed1 ?? closed2;
        }
    }
}