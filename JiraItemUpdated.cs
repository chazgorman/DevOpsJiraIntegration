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

    public class JiraItemUpdated
    {
        private readonly ILogger<JiraItemUpdated> _logger;

        public JiraItemUpdated(ILogger<JiraItemUpdated> logger)
        {
            _logger = logger;
        }

        public void SendDevOpsRequest(JiraIssueComment root)
        {
            DevOpsComment comment = new DevOpsComment() { text = root.comment.body };
            string commentJson = JsonConvert.SerializeObject(comment);

            try
            {
                string user = GetEnvironmentVariable("DevOpsUser").Split(':')[1].Trim();
                _logger.LogInformation("DevOps User: " + user);

                string token = GetEnvironmentVariable("DevOpsToken").Split(':')[1].Trim();
                _logger.LogInformation("DevOps Token: " + token);

                string url = GetEnvironmentVariable("DevOpsRootUrl").Split(':')[1].Trim();
                _logger.LogInformation("DevOps Url: " + url);

                string project = GetEnvironmentVariable("DevOpsProject").Split(':')[1].Trim();
                _logger.LogInformation("DevOps Project: " + project);

                string commentId = "41";

                UriBuilder builder = new UriBuilder(url);
                builder.Scheme = "https";
                builder.Host = url;
                builder.Path = project + "/_apis/wit/workItems/" + commentId + "/comments";
                builder.Query = "api-version=7.0-preview.3";
                builder.Port = -1;

                string requestUrl = builder.Uri.AbsoluteUri;

                //Putting the credentials as bytes.
                byte[] cred = UTF8Encoding.UTF8.GetBytes(user + ":" + token);


                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(cred));

                // DevOps create comment: https://dev.azure.com/CharlieGorman/Issue%20Tracking/_apis/wit/workItems/{workItemId}/comments?api-version=7.0-preview.3
                //string requestUrl = "https://dev.azure.com/CharlieGorman/Issue%20Tracking/_apis/wit/workItems/41/comments";


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

        public static string GetEnvironmentVariable(string name)
        {
            return name + ": " +
                System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
