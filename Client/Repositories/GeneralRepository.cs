﻿using API.Utilities.Enums;
using Client.Contracts;
using Newtonsoft.Json;
using System.Text;

namespace Client.Repositories
{
    public class GeneralRepository<Entity, Tid> : IRepository<Entity, Tid>
        where Entity : class
    {
        private readonly string request;
        private readonly HttpClient httpClient;


        public GeneralRepository(string request)
        {
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7025/api/")

            };
            this.request = request;
        }
        public async Task<ResponseHandler<Entity>> Delete(Tid id)
        {
            ResponseHandler<Entity> entityVM = null;
            using (var response = await httpClient.DeleteAsync(this.request+ "?guid="+id))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandler<Entity>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<ResponseHandler<IEnumerable<Entity>>> Get()
        {
            ResponseHandler<IEnumerable<Entity>> entityVM = null;
            using (var response = await httpClient.GetAsync(this.request))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandler<IEnumerable<Entity>>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<ResponseHandler<Entity>> Post(Entity entity)
        {
            ResponseHandler<Entity> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(request, content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandler<Entity>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<ResponseHandler<Entity>> Get(Tid id)
        {
            ResponseHandler<Entity> entity = null;
            using (var response = await httpClient.GetAsync(request + id))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<ResponseHandler<Entity>>(apiResponse);
            }
            return entity;
        }


        public async Task<ResponseHandler<Entity>> Put(Tid id, Entity entity)
        {
            ResponseHandler<Entity> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity),Encoding.UTF8, "application/json");
            using (var response = httpClient.PutAsync(request +"?guid="+ id, content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseHandler<Entity>>(apiResponse);
            }
            return entityVM;
        }
    }
}
