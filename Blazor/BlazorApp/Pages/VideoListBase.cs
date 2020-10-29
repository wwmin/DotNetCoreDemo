using BlazorAppModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorApp.Pages
{
    public class VideoListBase : ComponentBase
    {
        [Inject]
        public HttpClient _httpClient { get; set; }
        public IList<VideoModel> Videos = new List<VideoModel>();

        protected override async Task OnInitializedAsync()
        {
            //_httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
            var res = await _httpClient.GetFromJsonAsync<IList<VideoModel>>("Video");
           
            Console.WriteLine(res.Select(p=>p.title).ToString());
            Videos = res;
            await base.OnInitializedAsync();
        }
    }
}
