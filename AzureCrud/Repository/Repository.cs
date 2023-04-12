using Azure.Data.Tables;
using AzureCrud.Models;
using Microsoft.AspNetCore.Mvc;

namespace AzureCrud.Repository
{
    public class Repository<T>:IRepository<MyDataEntity>
    {

        private const string TableName = "Item";
        private readonly IConfiguration _configuration;
        public Repository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<TableClient> GetTableClient()
        {
            var serviceClient = new TableServiceClient(_configuration["StorageConnectionString"]);
            var tableClient = serviceClient.GetTableClient(TableName);
            await tableClient.CreateIfNotExistsAsync();
            return tableClient;
        }
        public async Task DeleteEntityAsync(string fileName, string id)
        {
            var tableClient = await  GetTableClient();
            await tableClient.DeleteEntityAsync(fileName, id);
           
        }

        public async Task<MyDataEntity> GetEntityAsync(string fileName, string id)
        {
            var tableClient = await GetTableClient();
            var data = await tableClient.GetEntityAsync<MyDataEntity>(fileName, id);
            return data;
        }

        public async Task<MyDataEntity> CreateEntityAsync(MyDataEntity entity)
        {
            var tableClient = await GetTableClient();
            await tableClient.UpsertEntityAsync(entity);
            return entity;
        }

        public async Task<ICollection<MyDataEntity>> GetAllEntityAsync()
        {
            ICollection<MyDataEntity> getAllData = new List<MyDataEntity>();

            var tableClient = await GetTableClient();

            var celebs = tableClient.QueryAsync<MyDataEntity>(filter: "");


            await foreach (var fileDatas in celebs)
            {
                getAllData.Add(fileDatas);
            }
            return getAllData;
        }

        public async Task<MyDataEntity> UpdateEntityAsync(MyDataEntity entity)
        {
            var tableClient = await GetTableClient();
            await tableClient.UpsertEntityAsync(entity);
            return entity;
        }
    }
}
