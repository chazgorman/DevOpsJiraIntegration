using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VSCodeFunction
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class Account
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string id;

        [JsonProperty("baseUrl")]
        [JsonPropertyName("baseUrl")]
        public string baseUrl;
    }

    public class Collection
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string id;

        [JsonProperty("baseUrl")]
        [JsonPropertyName("baseUrl")]
        public string baseUrl;
    }

    public class CommentVersionRef
    {
        [JsonProperty("commentId")]
        [JsonPropertyName("commentId")]
        public int? commentId;

        [JsonProperty("version")]
        [JsonPropertyName("version")]
        public int? version;

        [JsonProperty("url")]
        [JsonPropertyName("url")]
        public string url;
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
        public string SystemCreatedBy;

        [JsonProperty("System.ChangedDate")]
        [JsonPropertyName("System.ChangedDate")]
        public DateTime? SystemChangedDate;

        [JsonProperty("System.ChangedBy")]
        [JsonPropertyName("System.ChangedBy")]
        public string SystemChangedBy;

        [JsonProperty("System.CommentCount")]
        [JsonPropertyName("System.CommentCount")]
        public int? SystemCommentCount;

        [JsonProperty("System.Title")]
        [JsonPropertyName("System.Title")]
        public string SystemTitle;

        [JsonProperty("Microsoft.VSTS.Common.StateChangeDate")]
        [JsonPropertyName("Microsoft.VSTS.Common.StateChangeDate")]
        public DateTime? MicrosoftVSTSCommonStateChangeDate;

        [JsonProperty("Microsoft.VSTS.Common.Priority")]
        [JsonPropertyName("Microsoft.VSTS.Common.Priority")]
        public int? MicrosoftVSTSCommonPriority;

        [JsonProperty("Microsoft.VSTS.Common.Severity")]
        [JsonPropertyName("Microsoft.VSTS.Common.Severity")]
        public string MicrosoftVSTSCommonSeverity;

        [JsonProperty("Microsoft.VSTS.Common.ValueArea")]
        [JsonPropertyName("Microsoft.VSTS.Common.ValueArea")]
        public string MicrosoftVSTSCommonValueArea;

        [JsonProperty("System.History")]
        [JsonPropertyName("System.History")]
        public string SystemHistory;

        [JsonProperty("Microsoft.VSTS.TCM.SystemInfo")]
        [JsonPropertyName("Microsoft.VSTS.TCM.SystemInfo")]
        public string MicrosoftVSTSTCMSystemInfo;

        [JsonProperty("Microsoft.VSTS.TCM.ReproSteps")]
        [JsonPropertyName("Microsoft.VSTS.TCM.ReproSteps")]
        public string MicrosoftVSTSTCMReproSteps;

        [JsonProperty("href")]
        [JsonPropertyName("href")]
        public string href;

        [JsonProperty("Custom.JiraID")]
        [JsonPropertyName("Custom.JiraID")]
        public string JiraID;

        [JsonProperty("Custom.JiraKey")]
        [JsonPropertyName("Custom.JiraKey")]
        public string JiraKey;
    }

    public class Html
    {
        [JsonProperty("href")]
        [JsonPropertyName("href")]
        public string href;
    }

    public class Links
    {
        [JsonProperty("self")]
        [JsonPropertyName("self")]
        public Self self;

        [JsonProperty("workItemUpdates")]
        [JsonPropertyName("workItemUpdates")]
        public WorkItemUpdates workItemUpdates;

        [JsonProperty("workItemRevisions")]
        [JsonPropertyName("workItemRevisions")]
        public WorkItemRevisions workItemRevisions;

        [JsonProperty("workItemComments")]
        [JsonPropertyName("workItemComments")]
        public WorkItemComments workItemComments;

        [JsonProperty("html")]
        [JsonPropertyName("html")]
        public Html html;

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

        [JsonProperty("baseUrl")]
        [JsonPropertyName("baseUrl")]
        public string baseUrl;
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

        [JsonProperty("commentVersionRef")]
        [JsonPropertyName("commentVersionRef")]
        public CommentVersionRef commentVersionRef;

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

    public class WorkItemComments
    {
        [JsonProperty("href")]
        [JsonPropertyName("href")]
        public string href;
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