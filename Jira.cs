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

        [JsonProperty("components")]
        [JsonPropertyName("components")]
        public List<Component> components { get; set; }

        [JsonProperty("customfield_10062")]
        [JsonPropertyName("customfield_10062")]
        public Customfield10062 customfield_10062 { get; set; }

        [JsonProperty("issuetype")]
        [JsonPropertyName("issuetype")]
        public Issuetype issuetype { get; set; }
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

    public class JiraComment
    {
        [JsonProperty("body")]
        [JsonPropertyName("body")]
        public string body;
    }

    public class Component
    {
        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string name { get; set; }
    }

    public class Customfield10062
    {
        [JsonProperty("value")]
        [JsonPropertyName("value")]
        public string value { get; set; }
    }
}
