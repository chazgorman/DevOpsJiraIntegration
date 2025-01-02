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
    public class JiraItemUpdated
    {
        private readonly ILogger<JiraItemUpdated> _logger;

        public JiraItemUpdated(ILogger<JiraItemUpdated> logger)
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

        [Function("JiraItemUpdated")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {               
                string requestBody = new StreamReader(req.Body).ReadToEndAsync().Result;
                //dynamic data = JsonConvert.ToString(requestBody);

                _logger.LogInformation("JiraItemUpdated Request Body: " + requestBody);

                HttpClient client = new HttpClient();

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://webhook.site/380de732-9847-4c25-a68c-109d2b3cf33f");
                request.Headers.Add("Accept", "application/json");
                request.Content = new StringContent(requestBody);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = client.Send(request);

                _logger.LogInformation("Jira request response code: " + response.StatusCode);

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
