﻿using API.DTOs.Accounts;
using API.Utilities.Enums;
using Client.Contracts;
using Newtonsoft.Json;
using System.Text;

namespace Client.Repositories
{
    public class AccountRepository : GeneralRepository<RegisterDto, string>, IAccountRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string request;
        public AccountRepository(string request = "accounts/") : base(request)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7025/api/")
            };
            this.request = request;
        }

        public async Task<ResponseHandler<string>> Login(LoginDto entity)
        {
            ResponseHandler<string> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity),
            Encoding.UTF8, "application/json");
            using (var response = _httpClient.PostAsync(request + "login", content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandler<string>>(apiResponse);
            }

            return entityVM;
        }

        public async Task<ResponseHandler<AccountRepository>> Register(RegisterDto entity)
        {
            ResponseHandler<AccountRepository> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity),Encoding.UTF8, "application/json");
            using (var response = _httpClient.PostAsync(request +"register", content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandler<AccountRepository>>(apiResponse);
            }
            return entityVM;
        }
    }
}
