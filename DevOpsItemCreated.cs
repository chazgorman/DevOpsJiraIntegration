using Azure.Core;
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
using VSCodeFunction;

namespace lms
{
    public class DevOpsItemCreated
    {
        private readonly ILogger<DevOpsItemCreated> _logger;

        public DevOpsItemCreated(ILogger<DevOpsItemCreated> logger)
        {
            _logger = logger;
        }

        public void SendJiraRequest(DevOps root)
        {
            Jira jira = new Jira();

            JiraFields jiraFields = new JiraFields();
            jiraFields.summary = "LMS Bug " + root.id.ToString(); // root.resource.fields.SystemTitle;
            jiraFields.description = root.detailedMessage.text;
            jiraFields.issuetype = new Issuetype() { name = "Bug" };
            jiraFields.project = new JiraProject() { key = "CPG" };

            jira.fields = jiraFields;

            string data = JsonConvert.SerializeObject(jira);

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

            string token = GetEnvironmentVariable("JiraToken").Split(':')[1].Trim();
            _logger.LogInformation("Jira Token: " + token);

            //Putting the credentials as bytes.
            byte[] cred = UTF8Encoding.UTF8.GetBytes("chaz.gorman@gmail.com:" + token);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://chazgorman.atlassian.net/rest/api/latest/issue");

            request.Headers.Add("Accept", "application/json");

            //Putting credentials in Authorization headers.
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(cred));

            request.Content = new StringContent(data);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                HttpResponseMessage response = client.Send(request);
                //response.EnsureSuccessStatusCode();
                //string responseBody = response.Content.ReadAsStringAsync();

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
            //HttpClient client = new HttpClient();

            //var request = new HttpRequestMessage
            //{
            //    Method = HttpMethod.Post,
            //    RequestUri = new Uri("https://webhook.site/62403fa8-fc74-4249-8520-7f40b646fb3c"),
            //    Content = new StringContent(
            //    messageContent,
            //    Encoding.UTF8,
            //    MediaTypeNames.Application.Json), // or "application/json" in older versions
            //};

            //request.Headers.Add("Accept", "application/json");

            //var response = await client.SendAsync(request)
            //    .ConfigureAwait(false);
            //response.EnsureSuccessStatusCode();

            //var responseBody = await response.Content.ReadAsStringAsync()
            //    .ConfigureAwait(false);

            //var responseString = await response.Content.ReadAsStringAsync();

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
                    SendJiraRequest(rootDevOpsItem);
                }
                else
                {
                    _logger.LogInformation("DevOpsItemCreated: rootDevOpsItem is NULL");
                }

            }
            catch (Exception)
            {

                throw;
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
