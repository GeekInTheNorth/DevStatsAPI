﻿using System;
using System.Collections.Generic;
using System.Linq;
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
                          .Select(x => new Defect(x.DefectId, x.Module, x.Type, x.Created, x.Closed));
        }

        public void Save(IEnumerable<Defect> defects)
        {
            foreach (var defect in defects)
                ApplyChanges(defect);

            Context.SaveChanges();
        }

        private void ApplyChanges(Defect defect)
        {
            var dbItem = Context.Defects.FirstOrDefault(x => x.DefectId == defect.DefectId);

            if (dbItem == null)
            {
                dbItem = new Entities.Defect();
                Context.Defects.Add(dbItem);
            }

            dbItem.Created = defect.Created;
            dbItem.Closed = defect.Closed;
            dbItem.Module = defect.Module;
            dbItem.Type = defect.Type.ToString();
        }
    }
}