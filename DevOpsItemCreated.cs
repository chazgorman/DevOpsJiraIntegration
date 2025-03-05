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
    public class DevOpsOperation
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

        private DevOpsImageUtils _imageUtils;

        public DevOpsItemCreated(ILogger<DevOpsItemCreated> logger)
        {
            _logger = logger;

            _imageUtils = new DevOpsImageUtils(_logger);
        }

        public JiraItemCreatedResponse? SendJiraRequest(DevOps root)
        {
            string ?projectKey = Utils.GetEnvironmentVariable("JiraProjectKey");
            
            _logger.LogInformation("Jira Project Key: " + projectKey);

            Jira jira = new Jira();

            string[] devOpsIssueTitle = root.message.text.Split("created");
            JiraFields jiraFields = new JiraFields();
            jiraFields.summary = "[DevOps " + root.resource.fields.SystemWorkItemType + ":" + root.resource.id.ToString() + "]: " + root.resource.fields.SystemTitle;
            jiraFields.description = root.detailedMessage.text;
            jiraFields.components = new List<Component> { new Component() { name = "N/A" } };
            jiraFields.customfield_10062 = new Customfield10062() { value = "CX Development" };

            if (root.resource.fields.SystemWorkItemType == "Bug")
            {
                jiraFields.issuetype = new Issuetype() { name = "CX Request" };
            }
            else
            {
                jiraFields.issuetype = new Issuetype() { name = "CX Request" };
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

            string? user = Utils.GetEnvironmentVariable("JiraUser");
            _logger.LogInformation("Jira User: " + user);

            string? token = Utils.GetEnvironmentVariable("JiraToken");

            string? url = Utils.GetEnvironmentVariable("JiraRootUrl");
            _logger.LogInformation("Jira Url: " + url);

            if(url is null)
            {
                _logger.LogError("Jira URL environment variable is null");
                return null;
            }

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
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(cred));

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

        public void UpdateDevOpsIssueWithJiraNumberAndKey(string devOpsItemId, string JiraID, string JiraKey)
        {
            DevOpsOperation addJiraID = new DevOpsOperation()
            {
                op = "add",
                path = "/fields/Custom.JiraID",
                value = JiraID
            };

            DevOpsOperation addJiraKey = new DevOpsOperation()
            {
                op = "add",
                path = "/fields/Custom.JiraKey",
                value = JiraKey
            };

            List<DevOpsOperation> tags = new List<DevOpsOperation> { addJiraID, addJiraKey };

            string tagJson = JsonConvert.SerializeObject(tags);

            try
            {
                string? user = Utils.GetEnvironmentVariable("DevOpsUser");
                _logger.LogInformation("DevOps User: " + user);

                string? token = Utils.GetEnvironmentVariable("DevOpsToken");

                string? url = Utils.GetEnvironmentVariable("DevOpsRootUrl");
                _logger.LogInformation("DevOps Url: " + url);

                string? project = Utils.GetEnvironmentVariable("DevOpsProject");
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
                        UpdateDevOpsIssueWithJiraNumberAndKey(rootDevOpsItem.resource.id.ToString(), jiraResponse.id, jiraResponse.key);
                    }

                    if (rootDevOpsItem.resource.fields.SystemHistory.Contains("<img"))
                    {
                        _logger.LogInformation("Sending DevOps image attachments to Jira.");

                        var imgAttachmentIds = _imageUtils.FindIssueAttachments(rootDevOpsItem.resource.fields.SystemHistory);
                        string JiraIssueNumber = jiraResponse.key;
                        int imgIndex = 1;

                        foreach (var imgAttachment in imgAttachmentIds)
                        {
                            FileInfo fileInfo = new FileInfo(imgAttachment.Key);
                            string imgFileName = "DevOps_" + rootDevOpsItem.resource.id + "_attachment_" + imgIndex++ + fileInfo.Extension;
                            await _imageUtils.DownloadDevOpsAttachment(JiraIssueNumber, imgFileName, imgAttachment.Value);
                        }
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
    }
}
