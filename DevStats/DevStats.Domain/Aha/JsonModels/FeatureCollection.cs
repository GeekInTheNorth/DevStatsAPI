using System.Collections.Generic;
using Newtonsoft.Json;

namespace DevStats.Domain.Aha.JsonModels
{
    public class FeatureCollection
    {
        [JsonProperty("features")]
        public List<Feature> Features { get; set; }

        [JsonProperty("pagination")]
        public PaginationDetails Pagination { get; set; }
    }
}