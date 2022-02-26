using Microsoft.AspNetCore.Http;
using MVCApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Catalog.User;
using ViewModel.Common;

namespace MVCApp.Service
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserApiClient(IHttpClientFactory httpClientFactory,
             IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<UserInformationViewModel>> GetUserInformation(Guid Id)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri("https://localhost:5001");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var response = await client.GetAsync($"/api/users/GetUserInformation?userId={Id}");
            var body = await response.Content.ReadAsStringAsync();
            var companyInfor = JsonConvert.DeserializeObject<ApiSuccessResult<UserInformationViewModel>>(body);
            return companyInfor;
        }

        public async Task<ApiResult<bool>> UpdateUserInformation(Guid Id, UserInformationUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/users/UpdateUserInformation?id={Id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiResult<bool>>(result);
        }
    }
}
