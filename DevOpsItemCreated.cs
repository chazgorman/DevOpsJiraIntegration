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
    public class DevOpsTag
    {
        [JsonProperty("op")]
        [JsonPropertyName("op")]
        public string op;

        [JsonProperty("path")]
        [JsonPropertyName("path")]
        public string path;

        [JsonProperty("value")]
        [JsonPropertyName("value")]
        public string value;
    }

    public class DevOpsItemCreated
    {
        private readonly ILogger<DevOpsItemCreated> _logger;

        public DevOpsItemCreated(ILogger<DevOpsItemCreated> logger)
        {
            _logger = logger;
        }

        public JiraItemCreatedResponse? SendJiraRequest(DevOps root)
        {
            string projectKey = GetEnvironmentVariable("JiraProjectKey").Split(':')[1].Trim();
            _logger.LogInformation("Jira Project Key: " + projectKey);

            Jira jira = new Jira();

            string[] devOpsIssueTitle = root.message.text.Split("created");
            JiraFields jiraFields = new JiraFields();
            jiraFields.summary = "[DevOps " + root.resource.fields.SystemWorkItemType + ":" + root.resource.id.ToString() + "]: " + root.resource.fields.SystemTitle;
            jiraFields.description = root.detailedMessage.text;

            if(root.resource.fields.SystemWorkItemType == "Bug")
            {
                jiraFields.issuetype = new Issuetype() { name = "Bug" };
            }
            else
            {
                jiraFields.issuetype = new Issuetype() { name = "Issue" };
            }
            
            jiraFields.project = new JiraProject() { key = projectKey };

            if(root.resource.fields.MicrosoftVSTSTCMReproSteps is not null)
            {
                var html = root.resource.fields.MicrosoftVSTSTCMReproSteps;
                var converter = new Converter();
                var markdown = converter.Convert(html);

                jiraFields.description += "\r\nRepro Steps: \r\n" + markdown;
                jiraFields.description = jiraFields.description.Replace("<div>", "").Replace("</div>", "");
            }
            jira.fields = jiraFields;

            string data = JsonConvert.SerializeObject(jira);

            _logger.LogInformation("Jira request payload: " + data);

            //string data = @"{
            //    ""fields"": {
            //        ""project"": {
            //            ""key"": ""CPG""
            //        },
            //        ""summary"": ""Sample Bug"",
            //        ""description"": ""Creating a bug via REST API"",
            //        ""issuetype"": {
            //            ""name"": ""Bug""
            //        }
            //    }
            //}";

            HttpClient client = new HttpClient();

            string user = GetEnvironmentVariable("JiraUser").Split(':')[1].Trim();
            _logger.LogInformation("Jira User: " + user);

            string token = GetEnvironmentVariable("JiraToken").Split(':')[1].Trim();
            _logger.LogInformation("Jira Token: " + token);

            string url = GetEnvironmentVariable("JiraRootUrl").Split(':')[1].Trim();
            _logger.LogInformation("Jira Url: " + url);

            UriBuilder builder = new UriBuilder(url);
            builder.Scheme = "https";
            builder.Host = url;
            builder.Path = "rest/api/latest/issue";
            builder.Port = -1;

            string requestUrl = builder.Uri.AbsoluteUri;

            //Putting the credentials as bytes.
            byte[] cred = UTF8Encoding.UTF8.GetBytes(user + ":" + token);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUrl);

            request.Headers.Add("Accept", "application/json");

            //Putting credentials in Authorization headers.
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(cred));

            request.Content = new StringContent(data);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                HttpResponseMessage response = client.Send(request);
                response.EnsureSuccessStatusCode();
                Task<string> responseBody = response.Content.ReadAsStringAsync();

                responseBody.Wait();

                _logger.LogInformation("Jira request response code: " + response.StatusCode);

                string responseString = responseBody.Result;
                
                if(responseString != null)
                {
                    JiraItemCreatedResponse? jiraItemCreatedResponse = JsonConvert.DeserializeObject<JiraItemCreatedResponse>(responseString);
                    return jiraItemCreatedResponse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Exception sending Jira request: " + ex.Message);
            }

            return null;
        }

        public void TagDevOpsIssueWithJiraNumber(string devOpsItemId, string JiraIssueNumber)
        {
            DevOpsTag tag = new DevOpsTag()
            {
                op = "add",
                path = "/fields/System.Tags",
                value = "Jira:" + JiraIssueNumber
            };

            List<DevOpsTag> tags = new List<DevOpsTag> { tag };

            string tagJson = JsonConvert.SerializeObject(tags);

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

                UriBuilder builder = new UriBuilder(url);
                builder.Scheme = "https";
                builder.Host = url;
                builder.Path = project + "/_apis/wit/workItems/" + devOpsItemId;
                builder.Query = "api-version=7.0-preview.3";
                builder.Port = -1;

                string requestUrl = builder.Uri.AbsoluteUri;

                //Putting the credentials as bytes.
                byte[] cred = UTF8Encoding.UTF8.GetBytes(user + ":" + token);


                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(cred));

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, requestUrl);
                request.Headers.Add("Accept", "application/json");
                request.Content = new StringContent(tagJson);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json-patch+json");

                HttpResponseMessage response = client.Send(request);

                _logger.LogInformation("Jira request response code: " + response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Exception sending Jira request: " + ex.Message);
            }
        }

        [Function("DevOpsItemCreated")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string requestBody = new StreamReader(req.Body).ReadToEndAsync().Result;
                dynamic data = JsonConvert.ToString(requestBody);

                _logger.LogInformation("DevOpsItemCreated Request Body: " + requestBody);

                DevOps? rootDevOpsItem = JsonConvert.DeserializeObject<DevOps>(requestBody);

                if (rootDevOpsItem != null)
                {
                    _logger.LogInformation("Received DevOpsItem " + rootDevOpsItem?.id);
                    string messageContent = $"{data}";

                    _logger.LogInformation("Sending create Jira item request");
                    var jiraResponse = SendJiraRequest(rootDevOpsItem);
                    if (jiraResponse != null && rootDevOpsItem != null && rootDevOpsItem.resource != null && rootDevOpsItem.resource.id != null)
                    {
                        TagDevOpsIssueWithJiraNumber(rootDevOpsItem.resource.id.ToString(), jiraResponse.id);
                    }                    
                }
                else
                {
                    _logger.LogError("DevOpsItemCreated: rootDevOpsItem is NULL");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception in DevOpsItemCreated: " + ex.Message);
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
