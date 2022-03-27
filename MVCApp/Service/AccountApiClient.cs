using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Common;
using ViewModel.System.Users;

namespace MVCApp.Service
{
    public class AccountApiClient : IAccountApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AccountApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            var response = await client.PostAsync("/api/accounts/authenticate", httpContent);
            var token = await response.Content.ReadAsStringAsync();
            return token;
        }



        public async Task<ApiResult<bool>> DeleteAccount(Guid Id)
        {
            var client = _httpClientFactory.CreateClient();
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var response = await client.DeleteAsync($"/api/accounts/deleteAccount?id={Id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiResult<bool>>(result);
        }

        public async Task<ApiResult<PageResult<CompanyAccountViewModel>>> GetCompanyAccountPagings(GetAccountPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri("https://localhost:5001");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var response = await client.GetAsync($"/api/accounts/getCompanyAccount?pageIndex=" +
               $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");
            var body = await response.Content.ReadAsStringAsync();
            var accounts = JsonConvert.DeserializeObject<ApiSuccessResult<PageResult<CompanyAccountViewModel>>>(body);
            return accounts;
        }

        public async Task<ApiResult<PageResult<AccountViewModel>>> GetUserAccountPagings(GetAccountPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();

            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri("https://localhost:5001");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


            var response = await client.GetAsync($"/api/accounts/getUserAccount?pageIndex=" +
               $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");
            var body = await response.Content.ReadAsStringAsync();
            var accounts = JsonConvert.DeserializeObject<ApiSuccessResult<PageResult<AccountViewModel>>>(body);
            return accounts;
        }
    }
}
