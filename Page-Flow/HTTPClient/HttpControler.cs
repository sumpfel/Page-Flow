using System.Text.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq.Expressions;

namespace HTTPClient
{
    public class HttpControler
    {
        public string Adress = "192.168.66.18";
        public string Port = "5000";
        private string Username;
        private string Password;

        public string GetUrl()
        {
            return $"http://{Adress}:{Port}";
        }

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
                }
            }
            return true;
        }
        public async Task<bool> CreateUser(string username, string password)//register
        {
            Username = username;
            Password = password;
            bool result = await UserComands(username, password, GetUrl() + "/create_user");
            return result;
        }

        public async Task<bool> CheckUser(string username, string password)//login
        {
            Username = username;
            Password = password;
            bool result = await UserComands(Username, Password, GetUrl() + "/check_user");
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
        
    }
}
