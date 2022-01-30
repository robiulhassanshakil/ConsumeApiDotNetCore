using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Movies.Client.Models;

namespace Movies.Client.Services
{
    public class CRUDService : IIntegrationService
    {
        public static HttpClient _HttpClient = new();

        public CRUDService()
        {
            _HttpClient.BaseAddress = new Uri("http://localhost:57863");
            _HttpClient.Timeout = new TimeSpan(0, 0, 30);
            _HttpClient.DefaultRequestHeaders.Clear();
            /*_HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _HttpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/xml"));*/
        }
        public async Task Run()
        {
            //await GetResource();
            await GetResourceThroughHttpRequestMessage();
        }

        public async Task GetResource()
        {
            var response = await _HttpClient.GetAsync("api/movies");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var movies = new List<Movie>();
            if (response.Content.Headers.ContentType != null && response.Content.Headers.ContentType.MediaType == "application/json")
            {
                movies = JsonSerializer.Deserialize<List<Movie>>(content);
            }
            else
            {
                var serializer = new XmlSerializer(typeof(List<Movie>));
                movies = (List<Movie>)serializer.Deserialize(new StringReader(content));
            }
            
        }

        public async Task GetResourceThroughHttpRequestMessage()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/movies");
            request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            var response = await _HttpClient.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            var movies = JsonSerializer.Deserialize<IEnumerable<Movie>>(content);
        }
    }
}