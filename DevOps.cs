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
        [JsonPropertyName("id")]
        public string id;

        [JsonPropertyName("baseUrl")]
        public string baseUrl;
    }

    public class Collection
    {
        [JsonPropertyName("id")]
        public string id;

        [JsonPropertyName("baseUrl")]
        public string baseUrl;
    }

    public class CommentVersionRef
    {
        [JsonPropertyName("commentId")]
        public int? commentId;

        [JsonPropertyName("version")]
        public int? version;

        [JsonPropertyName("url")]
        public string url;
    }

    public class DetailedMessage
    {
        [JsonPropertyName("text")]
        public string text;
    }

    public class Fields
    {
        [JsonPropertyName("System.AreaPath")]
        public string SystemAreaPath;

        [JsonPropertyName("System.TeamProject")]
        public string SystemTeamProject;

        [JsonPropertyName("System.IterationPath")]
        public string SystemIterationPath;

        [JsonPropertyName("System.WorkItemType")]
        public string SystemWorkItemType;

        [JsonPropertyName("System.State")]
        public string SystemState;

        [JsonPropertyName("System.Reason")]
        public string SystemReason;

        [JsonPropertyName("System.CreatedDate")]
        public DateTime? SystemCreatedDate;

        [JsonPropertyName("System.CreatedBy")]
        public string SystemCreatedBy;

        [JsonPropertyName("System.ChangedDate")]
        public DateTime? SystemChangedDate;

        [JsonPropertyName("System.ChangedBy")]
        public string SystemChangedBy;

        [JsonPropertyName("System.CommentCount")]
        public int? SystemCommentCount;

        [JsonPropertyName("System.Title")]
        public string SystemTitle;

        [JsonPropertyName("Microsoft.VSTS.Common.StateChangeDate")]
        public DateTime? MicrosoftVSTSCommonStateChangeDate;

        [JsonPropertyName("Microsoft.VSTS.Common.Priority")]
        public int? MicrosoftVSTSCommonPriority;

        [JsonPropertyName("Microsoft.VSTS.Common.Severity")]
        public string MicrosoftVSTSCommonSeverity;

        [JsonPropertyName("Microsoft.VSTS.Common.ValueArea")]
        public string MicrosoftVSTSCommonValueArea;

        [JsonPropertyName("System.History")]
        public string SystemHistory;

        [JsonPropertyName("Microsoft.VSTS.TCM.SystemInfo")]
        public string MicrosoftVSTSTCMSystemInfo;

        [JsonPropertyName("Microsoft.VSTS.TCM.ReproSteps")]
        public string MicrosoftVSTSTCMReproSteps;

        [JsonPropertyName("href")]
        public string href;
    }

    public class Html
    {
        [JsonPropertyName("href")]
        public string href;
    }

    public class Links
    {
        [JsonPropertyName("self")]
        public Self self;

        [JsonPropertyName("workItemUpdates")]
        public WorkItemUpdates workItemUpdates;

        [JsonPropertyName("workItemRevisions")]
        public WorkItemRevisions workItemRevisions;

        [JsonPropertyName("workItemComments")]
        public WorkItemComments workItemComments;

        [JsonPropertyName("html")]
        public Html html;

        [JsonPropertyName("workItemType")]
        public WorkItemType workItemType;

        [JsonPropertyName("fields")]
        public Fields fields;
    }

    public class Message
    {
        [JsonPropertyName("text")]
        public string text;
    }

    public class Project
    {
        [JsonPropertyName("id")]
        public string id;

        [JsonPropertyName("baseUrl")]
        public string baseUrl;
    }

    public class Resource
    {
        [JsonPropertyName("id")]
        public int? id;

        [JsonPropertyName("rev")]
        public int? rev;

        [JsonPropertyName("fields")]
        public Fields fields;

        [JsonPropertyName("commentVersionRef")]
        public CommentVersionRef commentVersionRef;

        [JsonPropertyName("_links")]
        public Links _links;

        [JsonPropertyName("url")]
        public string url;
    }

    public class ResourceContainers
    {
        [JsonPropertyName("collection")]
        public Collection collection;

        [JsonPropertyName("account")]
        public Account account;

        [JsonPropertyName("project")]
        public Project project;
    }

    public class DevOps
    {
        [JsonPropertyName("subscriptionId")]
        public string subscriptionId;

        [JsonPropertyName("notificationId")]
        public int? notificationId;

        [JsonPropertyName("id")]
        public string id;

        [JsonPropertyName("eventType")]
        public string eventType;

        [JsonPropertyName("publisherId")]
        public string publisherId;

        [JsonPropertyName("message")]
        public Message message;

        [JsonPropertyName("detailedMessage")]
        public DetailedMessage detailedMessage;

        [JsonPropertyName("resource")]
        public Resource resource;

        [JsonPropertyName("resourceVersion")]
        public string resourceVersion;

        [JsonPropertyName("resourceContainers")]
        public ResourceContainers resourceContainers;

        [JsonPropertyName("createdDate")]
        public DateTime? createdDate;
    }

    public class Self
    {
        [JsonPropertyName("href")]
        public string href;
    }

    public class WorkItemComments
    {
        [JsonPropertyName("href")]
        public string href;
    }

    public class WorkItemRevisions
    {
        [JsonPropertyName("href")]
        public string href;
    }

    public class WorkItemType
    {
        [JsonPropertyName("href")]
        public string href;
    }

    public class WorkItemUpdates
    {
        [JsonPropertyName("href")]
        public string href;
    }
}
