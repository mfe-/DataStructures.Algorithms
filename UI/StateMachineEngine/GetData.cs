using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StateMachineEngine
{
    public class GetHttpData<TData>
    {
        public GetHttpData()
        {
            HttpClient = new HttpClient(new HttpClientHandler());
        }
        protected HttpClient HttpClient { get; set; }

        public async Task<TData> GetAsync(string url)
        {
            var response = await HttpClient.GetAsync(url);
            return await ToObjectAsync<TData>(response);
        }
        public async Task<T> ToObjectAsync<T>(HttpResponseMessage response)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<T>(jsonString);
            return model;
        }
        public HttpContent ToJson<T>(T model)
        {
            String jsonString = JsonConvert.SerializeObject(model);
            var content = new StringContent(jsonString, Encoding.UTF8, "text/plain");
            return content;
        }
    }
}
