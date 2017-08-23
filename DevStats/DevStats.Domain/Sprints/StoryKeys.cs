using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace DevStats.Domain.Sprints
{
    public class StoryKeys
    {
        [JsonProperty("issues")]
        public List<string> Keys { get; set; }

        public StoryKeys(IEnumerable<string> keys)
        {
            Keys = keys.ToList();
        }
    }
}