using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using lms;
using Microsoft.Extensions.Logging;

namespace VSCodeFunction
{
    internal class DevOpsImageUtils
    {
        private readonly ILogger _logger;

        public DevOpsImageUtils(ILogger logger)
        {
            _logger = logger;
        }

        public Dictionary<string, string> FindIssueAttachments(string systemHistoryString)
        {
            Dictionary<string, string> imageAttachments = new Dictionary<string, string>();

            var html = systemHistoryString;
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Select all img tags
            var imgTags = doc.DocumentNode.SelectNodes("//img");

            if (imgTags != null)
            {
                int imgIndex = 0;

                foreach (var img in imgTags)
                {
                    imgIndex++;

                    string imgSrc = img.GetAttributeValue("src", "");

                    Uri imgUri = new Uri(imgSrc);
                    var queryDictionary = System.Web.HttpUtility.ParseQueryString(imgUri.Query);
                    string? fileName = queryDictionary.Get("filename");

                    if (fileName is null)
                    {
                        fileName = "filename" + imgIndex;
                    }
                    else
                    {
                        FileInfo fileInfo = new FileInfo(fileName);
                        string fileExt = fileInfo.Extension;
                    }

                    string attachmentId = imgSrc.Substring(imgSrc.IndexOf("attachments/") + 12, 36);

                    imageAttachments.Add(fileName, attachmentId);
                }
            }

            return imageAttachments;
        }

        public async Task DownloadDevOpsAttachment(string issueKey, string DevOpsItemId, string attachmentId)
        {
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
                builder.Path = project + "/_apis/wit/attachments/" + attachmentId;
                builder.Query = "api-version=7.1";
                builder.Port = -1;

                string requestUrl = builder.Uri.AbsoluteUri;

                //Putting the credentials as bytes.
                byte[] cred = UTF8Encoding.UTF8.GetBytes(user + ":" + token);


                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(cred));
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl);

                try
                {
                    Task<HttpResponseMessage> httpRequest = client.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
                    HttpResponseMessage httpResponse = httpRequest.Result;
                    HttpStatusCode statusCode = httpResponse.StatusCode;
                    HttpContent responseContent = httpResponse.Content;

                    if (responseContent != null)
                    {
                        Task<byte[]> attachmentContentsTask = responseContent.ReadAsByteArrayAsync();
                        var fileContents = attachmentContentsTask.Result;
                        await SendFileToServerAsync(issueKey, DevOpsItemId, fileContents);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Exception sending Jira request: " + ex.Message);
            }
        }

        private async Task SendFileToServerAsync(string IssueKey, string imgFileName, byte[] fileContents)
        {
            //FileInfo fi = new FileInfo(@"C:\Users\gormanch\Documents\Projects\LMS\Integrations\Jira\PermitSearchExampleScreenshot.png");
            //byte[] fileContents = File.ReadAllBytes(fi.FullName);

            HttpClient client = new HttpClient();

            string? user = Utils.GetEnvironmentVariable("JiraUser");
            _logger.LogInformation("Jira User: " + user);

            string? token = Utils.GetEnvironmentVariable("JiraToken");

            string? url = Utils.GetEnvironmentVariable("JiraRootUrl");
            _logger.LogInformation("Jira Url: " + url);

            if (url is null)
            {
                _logger.LogError("Jira URL environment variable is null");
                return;
            }

            UriBuilder builder = new UriBuilder(url);
            builder.Scheme = "https";
            builder.Host = url;
            builder.Path = "rest/api/latest/issue/" + IssueKey + "/attachments";
            builder.Port = -1;

            string requestUrl = builder.Uri.AbsoluteUri;

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUrl);
            requestMessage.Headers.ExpectContinue = false;

            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(fileContents), "file", imgFileName);
            requestMessage.Content = content;

            //Putting the credentials as bytes.
            byte[] cred = UTF8Encoding.UTF8.GetBytes(user + ":" + token);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(cred));

            client.DefaultRequestHeaders.Add("X-Atlassian-Token", "no-check");
            try
            {
                Task<HttpResponseMessage> httpRequest = client.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
                HttpResponseMessage httpResponse = httpRequest.Result;
                HttpStatusCode statusCode = httpResponse.StatusCode;
                HttpContent responseContent = httpResponse.Content;

                if (responseContent != null)
                {
                    Task<String> stringContentsTask = responseContent.ReadAsStringAsync();
                    string contents = stringContentsTask.Result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
