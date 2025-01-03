using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VSCodeFunction
{
    public class Assignee
    {
        [JsonProperty("self")]
        [JsonPropertyName("self")]
        public string self;

        [JsonProperty("accountId")]
        [JsonPropertyName("accountId")]
        public string accountId;

        [JsonProperty("avatarUrls")]
        [JsonPropertyName("avatarUrls")]
        public AvatarUrls avatarUrls;

        [JsonProperty("displayName")]
        [JsonPropertyName("displayName")]
        public string displayName;

        [JsonProperty("active")]
        [JsonPropertyName("active")]
        public bool? active;

        [JsonProperty("timeZone")]
        [JsonPropertyName("timeZone")]
        public string timeZone;

        [JsonProperty("accountType")]
        [JsonPropertyName("accountType")]
        public string accountType;
    }

    public class Author
    {
        [JsonProperty("self")]
        [JsonPropertyName("self")]
        public string self;

        [JsonProperty("accountId")]
        [JsonPropertyName("accountId")]
        public string accountId;

        [JsonProperty("avatarUrls")]
        [JsonPropertyName("avatarUrls")]
        public AvatarUrls avatarUrls;

        [JsonProperty("displayName")]
        [JsonPropertyName("displayName")]
        public string displayName;

        [JsonProperty("active")]
        [JsonPropertyName("active")]
        public bool? active;

        [JsonProperty("timeZone")]
        [JsonPropertyName("timeZone")]
        public string timeZone;

        [JsonProperty("accountType")]
        [JsonPropertyName("accountType")]
        public string accountType;
    }

    public class AvatarUrls
    {
        [JsonProperty("48x48")]
        [JsonPropertyName("48x48")]
        public string _48x48;

        [JsonProperty("24x24")]
        [JsonPropertyName("24x24")]
        public string _24x24;

        [JsonProperty("16x16")]
        [JsonPropertyName("16x16")]
        public string _16x16;

        [JsonProperty("32x32")]
        [JsonPropertyName("32x32")]
        public string _32x32;
    }

    public class Comment
    {
        [JsonProperty("self")]
        [JsonPropertyName("self")]
        public string self;

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string id;

        [JsonProperty("author")]
        [JsonPropertyName("author")]
        public Author author;

        [JsonProperty("body")]
        [JsonPropertyName("body")]
        public string body;

        [JsonProperty("updateAuthor")]
        [JsonPropertyName("updateAuthor")]
        public UpdateAuthor updateAuthor;

        [JsonProperty("created")]
        [JsonPropertyName("created")]
        public DateTime? created;

        [JsonProperty("updated")]
        [JsonPropertyName("updated")]
        public DateTime? updated;

        [JsonProperty("jsdPublic")]
        [JsonPropertyName("jsdPublic")]
        public bool? jsdPublic;
    }

    public class JiraIssueCommentFields
    {
        [JsonProperty("summary")]
        [JsonPropertyName("summary")]
        public string summary;

        [JsonProperty("issuetype")]
        [JsonPropertyName("issuetype")]
        public JiraIssueCommentIssuetype issuetype;

        [JsonProperty("project")]
        [JsonPropertyName("project")]
        public JiraIssueCommentProject project;

        [JsonProperty("assignee")]
        [JsonPropertyName("assignee")]
        public Assignee assignee;

        [JsonProperty("priority")]
        [JsonPropertyName("priority")]
        public Priority priority;

        [JsonProperty("status")]
        [JsonPropertyName("status")]
        public Status status;
    }

    public class Issue
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string id;

        [JsonProperty("self")]
        [JsonPropertyName("self")]
        public string self;

        [JsonProperty("key")]
        [JsonPropertyName("key")]
        public string key;

        [JsonProperty("fields")]
        [JsonPropertyName("fields")]
        public JiraIssueCommentFields fields;
    }

    public class JiraIssueCommentIssuetype
    {
        [JsonProperty("self")]
        [JsonPropertyName("self")]
        public string self;

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string id;

        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string description;

        [JsonProperty("iconUrl")]
        [JsonPropertyName("iconUrl")]
        public string iconUrl;

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string name;

        [JsonProperty("subtask")]
        [JsonPropertyName("subtask")]
        public bool? subtask;

        [JsonProperty("avatarId")]
        [JsonPropertyName("avatarId")]
        public int? avatarId;

        [JsonProperty("entityId")]
        [JsonPropertyName("entityId")]
        public string entityId;

        [JsonProperty("hierarchyLevel")]
        [JsonPropertyName("hierarchyLevel")]
        public int? hierarchyLevel;
    }

    public class Priority
    {
        [JsonProperty("self")]
        [JsonPropertyName("self")]
        public string self;

        [JsonProperty("iconUrl")]
        [JsonPropertyName("iconUrl")]
        public string iconUrl;

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string name;

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string id;
    }

    public class JiraIssueCommentProject
    {
        [JsonProperty("self")]
        [JsonPropertyName("self")]
        public string self;

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string id;

        [JsonProperty("key")]
        [JsonPropertyName("key")]
        public string key;

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string name;

        [JsonProperty("projectTypeKey")]
        [JsonPropertyName("projectTypeKey")]
        public string projectTypeKey;

        [JsonProperty("simplified")]
        [JsonPropertyName("simplified")]
        public bool? simplified;

        [JsonProperty("avatarUrls")]
        [JsonPropertyName("avatarUrls")]
        public AvatarUrls avatarUrls;
    }

    public class JiraIssueComment
    {
        [JsonProperty("timestamp")]
        [JsonPropertyName("timestamp")]
        public long? timestamp;

        [JsonProperty("webhookEvent")]
        [JsonPropertyName("webhookEvent")]
        public string webhookEvent;

        [JsonProperty("comment")]
        [JsonPropertyName("comment")]
        public Comment comment;

        [JsonProperty("issue")]
        [JsonPropertyName("issue")]
        public Issue issue;

        [JsonProperty("eventType")]
        [JsonPropertyName("eventType")]
        public string eventType;
    }

    public class Status
    {
        [JsonProperty("self")]
        [JsonPropertyName("self")]
        public string self;

        [JsonProperty("description")]
        [JsonPropertyName("description")]
        public string description;

        [JsonProperty("iconUrl")]
        [JsonPropertyName("iconUrl")]
        public string iconUrl;

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string name;

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string id;

        [JsonProperty("statusCategory")]
        [JsonPropertyName("statusCategory")]
        public StatusCategory statusCategory;
    }

    public class StatusCategory
    {
        [JsonProperty("self")]
        [JsonPropertyName("self")]
        public string self;

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public int? id;

        [JsonProperty("key")]
        [JsonPropertyName("key")]
        public string key;

        [JsonProperty("colorName")]
        [JsonPropertyName("colorName")]
        public string colorName;

        [JsonProperty("name")]
        [JsonPropertyName("name")]
        public string name;
    }

    public class UpdateAuthor
    {
        [JsonProperty("self")]
        [JsonPropertyName("self")]
        public string self;

        [JsonProperty("accountId")]
        [JsonPropertyName("accountId")]
        public string accountId;

        [JsonProperty("avatarUrls")]
        [JsonPropertyName("avatarUrls")]
        public AvatarUrls avatarUrls;

        [JsonProperty("displayName")]
        [JsonPropertyName("displayName")]
        public string displayName;

        [JsonProperty("active")]
        [JsonPropertyName("active")]
        public bool? active;

        [JsonProperty("timeZone")]
        [JsonPropertyName("timeZone")]
        public string timeZone;

        [JsonProperty("accountType")]
        [JsonPropertyName("accountType")]
        public string accountType;
    }


}
