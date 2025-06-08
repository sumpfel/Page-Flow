using System;
using System.IO;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HTTPClient
{
    public class HttpControler
    {
        public string Address = "192.168.66.18";
        public string Port = "5000";
        private string Username;
        private string Password;

        public string GetUserName() { if (Username != null) { return Username; } else { return ""; } }
        public void LogOut() { Username = null;Password = null; }
        public string GetUrl()
        {
            return $"http://{Address}:{Port}";
        }

        public HttpControler(string address, string port) { Address = address;Port = port; }

        private async Task<bool> UserComands(string username, string password, string url)
        {
            Dictionary<string, string> payload_dict = new Dictionary<string, string>() { { "user_name", username }, { "pwd",password} };
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string payload = JsonSerializer.Serialize(payload_dict);
                    HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode) { return true; }
                    return false;
                }
                catch (Exception ex)
                {
                    //TODO: LOG ex
                    throw(ex);
                }
            }
            return true;
        }
        public async Task<bool> CreateUser(string username, string password)//register
        {
            bool result = await UserComands(username, password, GetUrl() + "/create_user");
            if (result)
            {
                Username = username;
                Password = password;
            }
            return result;
        }

        public async Task<bool> CheckUser(string username, string password)//login
        {
            
            bool result = await UserComands(username, password, GetUrl() + "/check_user");
            if (result)
            {
                Username = username;
                Password = password;
            }
            return result;
        }

        private async Task<bool> Download(string url, string download_path)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                    if (response.IsSuccessStatusCode)
                    {
                        using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                                      fileStream = new FileStream(download_path, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            await contentStream.CopyToAsync(fileStream);
                        }
                        return true;
                    }
                    else { return false; }
                }
            }
            catch (Exception ex)
            {
                //TODO Log
            }
            return false;
        }
        
        public async Task<bool> DownloadPreviewFile(string download_path)
        {
            return await Download(GetUrl() + "/preview_books", download_path);
        }

        public async Task<bool> DownloadBook(string book_name,string download_path)
        {
            return await Download(GetUrl() + "/download/" + book_name, download_path);
        }

        public async Task<bool> DownloadAll(string download_path)
        {
            return await Download(GetUrl() + "/download/all", download_path);
        }

        private async Task<bool> SendFeedback(Dictionary<string,string> payload_dict)
        {
            
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string payload = JsonSerializer.Serialize(payload_dict);
                    HttpContent content = new StringContent(payload, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(GetUrl() + "/feedback", content);
                    if (response.IsSuccessStatusCode) { return true; }
                    return false;
                }
            }
            catch (Exception ex)
            {
                //TODO: LOG ex
                throw (ex);
            }
            
        }
        public async Task<bool> sendLikes(string book_path, string likes)
        {
            Dictionary<string, string> payload_dict = new Dictionary<string, string>() { { "user_name", Username }, { "pwd", Password }, { "book_path", book_path }, { "like", likes } };
            return await SendFeedback(payload_dict);
        }

        public async Task<bool> sendComment(string book_path,string comment)
        {
            Dictionary<string, string> payload_dict = new Dictionary<string, string>() { { "user_name", Username }, { "pwd", Password }, { "book_path", book_path }, { "comment", comment } };
            return await SendFeedback(payload_dict);
        }
    }
}
