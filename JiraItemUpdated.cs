using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Serialization;
using VSCodeFunction;

namespace lms
{
    public class DevOpsComment
    {
        [JsonProperty("text")]
        [JsonPropertyName("text")]
        public string text;
    }

    public class JiraItemCreatedResponse
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string id;

        [JsonProperty("key")]
        [JsonPropertyName("key")]
        public string key;

        [JsonProperty("self")]
        [JsonPropertyName("self")]
        public string self;
    }


    public class JiraItemUpdated
    {
        private readonly ILogger<JiraItemUpdated> _logger;

        public JiraItemUpdated(ILogger<JiraItemUpdated> logger)
        {
            _logger = logger;
        }

        public void SendDevOpsRequest(JiraIssueComment root)
        {
            // Throw away non-public comments (identified by the jsdPublic field equal to false)
            if (root.comment.jsdPublic.HasValue && root.comment.jsdPublic.Value == false)
            {
                _logger.LogInformation("Disarding non-public comment");
                return;
            }

            // If the comment was originally from DevOps, don't send to DevOps
            if (root.comment.body.StartsWith("DevOps Update"))
            {
                return;
            }

            DevOpsComment comment = new DevOpsComment() { text = "Jira Update:\r\n" + root.comment.body };
            string commentJson = JsonConvert.SerializeObject(comment);

            string[] summarySplit = root.issue.fields.summary.Split("]");
            string[] idSplit = summarySplit[0].Split(":");
            string devOpsID = idSplit[1];

            try
            {
                string? user = Utils.GetEnvironmentVariable("DevOpsUser");
                _logger.LogInformation("DevOps User: " + user);

                string? token = Utils.GetEnvironmentVariable("DevOpsToken");
                _logger.LogInformation("DevOps Token: " + token);

                string? url = Utils.GetEnvironmentVariable("DevOpsRootUrl");
                _logger.LogInformation("DevOps Url: " + url);

                string? project = Utils.GetEnvironmentVariable("DevOpsProject");
                _logger.LogInformation("DevOps Project: " + project);


                UriBuilder builder = new UriBuilder(url);
                builder.Scheme = "https";
                builder.Host = url;
                builder.Path = project + "/_apis/wit/workItems/" + devOpsID + "/comments";
                builder.Query = "api-version=7.0-preview.3";
                builder.Port = -1;

                string requestUrl = builder.Uri.AbsoluteUri;

                //Putting the credentials as bytes.
                byte[] cred = UTF8Encoding.UTF8.GetBytes(user + ":" + token);


                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(cred));

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
                request.Headers.Add("Accept", "application/json");
                request.Content = new StringContent(commentJson);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.Send(request);

                _logger.LogInformation("Jira request response code: " + response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Exception sending Jira request: " + ex.Message);
            }
        }

        [Function("JiraItemUpdated")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = new StreamReader(req.Body).ReadToEndAsync().Result;
                dynamic data = JsonConvert.ToString(requestBody);

                _logger.LogInformation("JiraItemUpdated Request Body: " + requestBody);

                JiraIssueComment? rootJiraIssueComment = JsonConvert.DeserializeObject<JiraIssueComment>(requestBody);

                if (rootJiraIssueComment != null)
                {
                    _logger.LogInformation("Received DevOpsItem " + rootJiraIssueComment?.issue.id);
                    string messageContent = $"{data}";

                    _logger.LogInformation("Sending create Jira item request");
                    SendDevOpsRequest(rootJiraIssueComment);
                }
                else
                {
                    _logger.LogInformation("DevOpsItemCreated: rootDevOpsItem is NULL");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Exception in JiraItemUpdated: " + ex.Message);
            }
            return new OkObjectResult("OK");
        }
    }
}
