using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VSCodeFunction
{
    public class Account
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string id;
    }

    public class Avatar
    {
        [JsonProperty("href")]
        [JsonPropertyName("href")]
        public string href;
    }

    public class Collection
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string id;
    }

    public class DetailedMessage
    {
        [JsonProperty("text")]
        [JsonPropertyName("text")]
        public string text;
    }

    public class Fields
    {
        [JsonProperty("System.AreaPath")]
        [JsonPropertyName("System.AreaPath")]
        public string SystemAreaPath;

        [JsonProperty("System.TeamProject")]
        [JsonPropertyName("System.TeamProject")]
        public string SystemTeamProject;

        [JsonProperty("System.IterationPath")]
        [JsonPropertyName("System.IterationPath")]
        public string SystemIterationPath;

        [JsonProperty("System.WorkItemType")]
        [JsonPropertyName("System.WorkItemType")]
        public string SystemWorkItemType;

        [JsonProperty("System.State")]
        [JsonPropertyName("System.State")]
        public string SystemState;

        [JsonProperty("System.Reason")]
        [JsonPropertyName("System.Reason")]
        public string SystemReason;

        [JsonProperty("System.CreatedDate")]
        [JsonPropertyName("System.CreatedDate")]
        public DateTime? SystemCreatedDate;

        [JsonProperty("System.CreatedBy")]
        [JsonPropertyName("System.CreatedBy")]
        public SystemCreatedBy SystemCreatedBy;

        [JsonProperty("System.ChangedDate")]
        [JsonPropertyName("System.ChangedDate")]
        public DateTime? SystemChangedDate;

        [JsonProperty("System.ChangedBy")]
        [JsonPropertyName("System.ChangedBy")]
        public SystemChangedBy SystemChangedBy;

        [JsonProperty("System.Title")]
        [JsonPropertyName("System.Title")]
        public string SystemTitle;

        [JsonProperty("Microsoft.VSTS.Common.Severity")]
        [JsonPropertyName("Microsoft.VSTS.Common.Severity")]
        public string MicrosoftVSTSCommonSeverity;

        [JsonProperty("WEF_EB329F44FE5F4A94ACB1DA153FDF38BA_Kanban.Column")]
        [JsonPropertyName("WEF_EB329F44FE5F4A94ACB1DA153FDF38BA_Kanban.Column")]
        public string WEF_EB329F44FE5F4A94ACB1DA153FDF38BA_KanbanColumn;

        [JsonProperty("href")]
        [JsonPropertyName("href")]
        public string href;
    }

    public class Links
    {
        [JsonProperty("avatar")]
        [JsonPropertyName("avatar")]
        public Avatar avatar;

        [JsonProperty("self")]
        [JsonPropertyName("self")]
        public Self self;

        [JsonProperty("workItemUpdates")]
        [JsonPropertyName("workItemUpdates")]
        public WorkItemUpdates workItemUpdates;

        [JsonProperty("workItemRevisions")]
        [JsonPropertyName("workItemRevisions")]
        public WorkItemRevisions workItemRevisions;

        [JsonProperty("workItemType")]
        [JsonPropertyName("workItemType")]
        public WorkItemType workItemType;

        [JsonProperty("fields")]
        [JsonPropertyName("fields")]
        public Fields fields;
    }

    public class Message
    {
        [JsonProperty("text")]
        [JsonPropertyName("text")]
        public string text;
    }

    public class Project
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string id;
    }

    public class Resource
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public int? id;

        [JsonProperty("rev")]
        [JsonPropertyName("rev")]
        public int? rev;

        [JsonProperty("fields")]
        [JsonPropertyName("fields")]
        public Fields fields;

        [JsonProperty("_links")]
        [JsonPropertyName("_links")]
        public Links _links;

        [JsonProperty("url")]
        [JsonPropertyName("url")]
        public string url;
    }

    public class ResourceContainers
    {
        [JsonProperty("collection")]
        [JsonPropertyName("collection")]
        public Collection collection;

        [JsonProperty("account")]
        [JsonPropertyName("account")]
        public Account account;

        [JsonProperty("project")]
        [JsonPropertyName("project")]
        public Project project;
    }

    public class DevOps
    {
        [JsonProperty("subscriptionId")]
        [JsonPropertyName("subscriptionId")]
        public string subscriptionId;

        [JsonProperty("notificationId")]
        [JsonPropertyName("notificationId")]
        public int? notificationId;

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string id;

        [JsonProperty("eventType")]
        [JsonPropertyName("eventType")]
        public string eventType;

        [JsonProperty("publisherId")]
        [JsonPropertyName("publisherId")]
        public string publisherId;

        [JsonProperty("message")]
        [JsonPropertyName("message")]
        public Message message;

        [JsonProperty("detailedMessage")]
        [JsonPropertyName("detailedMessage")]
        public DetailedMessage detailedMessage;

        [JsonProperty("resource")]
        [JsonPropertyName("resource")]
        public Resource resource;

        [JsonProperty("resourceVersion")]
        [JsonPropertyName("resourceVersion")]
        public string resourceVersion;

        [JsonProperty("resourceContainers")]
        [JsonPropertyName("resourceContainers")]
        public ResourceContainers resourceContainers;

        [JsonProperty("createdDate")]
        [JsonPropertyName("createdDate")]
        public DateTime? createdDate;
    }

    public class Self
    {
        [JsonProperty("href")]
        [JsonPropertyName("href")]
        public string href;
    }

    public class SystemChangedBy
    {
        [JsonProperty("displayName")]
        [JsonPropertyName("displayName")]
        public string displayName;

        [JsonProperty("url")]
        [JsonPropertyName("url")]
        public string url;

        [JsonProperty("_links")]
        [JsonPropertyName("_links")]
        public Links _links;

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string id;

        [JsonProperty("uniqueName")]
        [JsonPropertyName("uniqueName")]
        public string uniqueName;

        [JsonProperty("imageUrl")]
        [JsonPropertyName("imageUrl")]
        public string imageUrl;

        [JsonProperty("descriptor")]
        [JsonPropertyName("descriptor")]
        public string descriptor;
    }

    public class SystemCreatedBy
    {
        [JsonProperty("displayName")]
        [JsonPropertyName("displayName")]
        public string displayName;

        [JsonProperty("url")]
        [JsonPropertyName("url")]
        public string url;

        [JsonProperty("_links")]
        [JsonPropertyName("_links")]
        public Links _links;

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string id;

        [JsonProperty("uniqueName")]
        [JsonPropertyName("uniqueName")]
        public string uniqueName;

        [JsonProperty("imageUrl")]
        [JsonPropertyName("imageUrl")]
        public string imageUrl;

        [JsonProperty("descriptor")]
        [JsonPropertyName("descriptor")]
        public string descriptor;
    }

    public class WorkItemRevisions
    {
        [JsonProperty("href")]
        [JsonPropertyName("href")]
        public string href;
    }

    public class WorkItemType
    {
        [JsonProperty("href")]
        [JsonPropertyName("href")]
        public string href;
    }

    public class WorkItemUpdates
    {
        [JsonProperty("href")]
        [JsonPropertyName("href")]
        public string href;
    }
}
