using System;
using System.Collections.Generic;
using System.Linq;
using DevStats.Domain.Aha;
using DevStats.Domain.DefectAnalysis;
using DevStats.Domain.Jira;

namespace DevStats.Data.Repositories
{
    public class DefectRepository : BaseRepository, IDefectRepository
    {
        public IEnumerable<Defect> Get(DateTime createdFrom, DateTime createdTo)
        {
            var rawDefects = (from defect in Context.Defects
                              join mapping in Context.DefectMappings on new { defect.Product, defect.Module } equals new { Product = mapping.OriginalProduct, Module = mapping.OriginalModule } into mappings
                              from map in mappings.DefaultIfEmpty()
                              select new
                              {
                                  defect.JiraId,
                                  defect.AhaId,
                                  Product = map != null ? map.DisplayProduct : defect.Product,
                                  Module = map != null ? map.DisplayModule : defect.Module,
                                  defect.Type,
                                  defect.Created,
                                  defect.Closed
                              }).ToList();

            return rawDefects.Select(x => new Defect(x.JiraId, x.AhaId, x.Product, x.Module, x.Type, x.Created, x.Closed));
        }

        public void Save(IEnumerable<AhaDefect> defects)
        {
            if (defects == null || !defects.Any()) return;

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

            if (!dbItem.Closed.HasValue)
                dbItem.Closed = defect.Closed;
        }

        public void Save(IEnumerable<JiraDefect> defects)
        {
            if (defects == null || !defects.Any()) return;

            foreach (var defect in defects)
                Save(defect);

            Context.SaveChanges();
        }

        private void Save(JiraDefect defect)
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
            else if (defect.Type == DefectType.NonDefect)
            {
                Context.Defects.Remove(dbItem);
            }
            else
            {
                dbItem.AhaId = GetBestString(defect.AhaId, dbItem.AhaId);
                dbItem.JiraId = GetBestString(defect.JiraId, dbItem.JiraId);
                dbItem.Module = GetBestString(defect.Module, dbItem.Module);
                dbItem.Product = defect.Product;
                dbItem.Created = GetFirstCreated(defect.Created, dbItem.Created);
                dbItem.Closed = dbItem.Closed ?? defect.Closed;
                dbItem.Type = defect.Type.ToString();
            }
        }

        public void Delete(string jiraId)
        {
            var dbItem = Context.Defects.FirstOrDefault(x => x.JiraId == jiraId);

            if (dbItem != null)
            {
                Context.Defects.Remove(dbItem);
                Context.SaveChanges();
            }
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
    }
}