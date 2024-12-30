using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VSCodeFunction
{
    public class JiraFields
    {
        [JsonProperty("project")]
        [JsonPropertyName("project")]
        public JiraProject project;

        [JsonProperty("summary")]
        [JsonPropertyName("summary")]
        public string summary;

        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string description;

        [JsonProperty("issuetype")]
        [JsonPropertyName("issuetype")]
        public Issuetype issuetype;
    }

    public class Issuetype
    {
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string name;
    }

    public class JiraProject
    {
        [JsonProperty("key")]
        [JsonPropertyName("key")]
        public string key;
    }

    public class Jira
    {
        [JsonProperty("fields")]
        [JsonPropertyName("fields")]
        public JiraFields fields;
    }


}
