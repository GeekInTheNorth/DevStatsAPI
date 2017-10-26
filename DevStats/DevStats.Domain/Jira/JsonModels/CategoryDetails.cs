using Newtonsoft.Json;

namespace DevStats.Domain.Jira.JsonModels
{
    public class CategoryDetails
    {
        [JsonProperty("value")]
        public string Category { get; set; }

        [JsonProperty("child")]
        public ValueField SubCategory { get; set; }

        public override string ToString()
        {
            var subCategory = SubCategory == null ? string.Empty : SubCategory.Value;

            return string.Format("{0} - {1}", Category, subCategory);
        }
    }
}