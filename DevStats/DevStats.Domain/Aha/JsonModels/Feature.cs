using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DevStats.Domain.Aha.JsonModels
{
    public class Feature
    {
        [JsonProperty("reference_num")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Description { get; set; }

        [JsonProperty("created_at")]
        public DateTime Created { get; set; }

        [JsonProperty("updated_at")]
        public DateTime Updated { get; set; }

        [JsonProperty("workflow_status")]
        public SimpleField Status { get; set; }

        [JsonProperty("workflow_kind")]
        public SimpleField FeatureType { get; set; }

        [JsonProperty("integration_fields")]
        public List<IntegrationField> IntegrationFields { get; set; }

        [JsonProperty("custom_fields")]
        public List<CustomField> CustomFields { get; set; }
    }
}