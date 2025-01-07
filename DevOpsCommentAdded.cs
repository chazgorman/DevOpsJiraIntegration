using Azure.Core;
using Html2Markdown;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Serialization;
using VSCodeFunction;

namespace lms
{
    public class DevOpsCommentAdded
    {
        private readonly ILogger<DevOpsCommentAdded> _logger;

        public DevOpsCommentAdded(ILogger<DevOpsCommentAdded> logger)
        {
            _logger = logger;
        }

        public void SendDevOpsCommentToJira(string JiraIssueNumber, string comment)
        {
            JiraComment jiraComment = new JiraComment();
            jiraComment.body = comment;

            string commentJson = JsonConvert.SerializeObject(jiraComment);

            try
            {
                HttpClient client = new HttpClient();

                string? user = Utils.GetEnvironmentVariable("JiraUser");
                _logger.LogInformation("Jira User: " + user);

                string? token = Utils.GetEnvironmentVariable("JiraToken");
                _logger.LogInformation("Jira Token: " + token);

                string? url = Utils.GetEnvironmentVariable("JiraRootUrl");
                _logger.LogInformation("Jira Url: " + url);

                if(url is null)
                {
                    _logger.LogError("Jira URL environment variable is null");
                    return;
                }

                UriBuilder builder = new UriBuilder(url);
                builder.Scheme = "https";
                builder.Host = url;
                builder.Path = "rest/api/latest/issue/" + JiraIssueNumber + "/comment";
                builder.Port = -1;

                string requestUrl = builder.Uri.AbsoluteUri;

                //Putting the credentials as bytes.
                byte[] cred = UTF8Encoding.UTF8.GetBytes(user + ":" + token);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(cred));

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

        [Function("DevOpsCommentAdded")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = new StreamReader(req.Body).ReadToEndAsync().Result;
                dynamic data = JsonConvert.ToString(requestBody);

                _logger.LogInformation("DevOpsCommentAdded Request Body: " + requestBody);

                DevOps? rootDevOpsItem = JsonConvert.DeserializeObject<DevOps>(requestBody);

                if (rootDevOpsItem != null)
                {
                    _logger.LogInformation("Received DevOps Comment " + rootDevOpsItem?.id);
                    string messageContent = $"{data}";

                    if (rootDevOpsItem != null && rootDevOpsItem.resource != null && rootDevOpsItem.resource.id != null)
                    {
                        // If the comment was originally from Jira, don't send back to Jira
                        if(!rootDevOpsItem.detailedMessage.text.Contains("Jira Update"))
                        {
                            _logger.LogInformation("Sending DevOps comment to Jira.");

                            string JiraIssueNumber = rootDevOpsItem.resource.fields.JiraID;

                            string comment = "DevOps Update:\r\n" + rootDevOpsItem.detailedMessage.text;

                            SendDevOpsCommentToJira(JiraIssueNumber, comment);
                        }
                    }                    
                }
                else
                {
                    _logger.LogError("DevOpsCommentAdded: rootDevOpsItem is NULL");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception in DevOpsCommentAdded: " + ex.Message);
            }
            return new OkObjectResult("OK");
        }
    }
}
