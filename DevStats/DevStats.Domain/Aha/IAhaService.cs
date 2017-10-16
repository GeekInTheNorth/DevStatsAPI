using System;

namespace DevStats.Domain.Aha
{
    public interface IAhaService
    {
        void UpdateDefectsFromAha(DateTime earliestModified);
    }
}