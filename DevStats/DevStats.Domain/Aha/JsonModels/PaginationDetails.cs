using Newtonsoft.Json;

namespace DevStats.Domain.Aha.JsonModels
{
    public class PaginationDetails
    {
        [JsonProperty("total_records")]
        public int TotalRecords { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("current_page")]
        public int CurrentPage { get; set; }
    }
}