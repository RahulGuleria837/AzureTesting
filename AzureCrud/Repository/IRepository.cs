using AzureCrud.Models;

namespace AzureCrud.Repository
{
    public interface IRepository<T>
    {
        Task<MyDataEntity> GetEntityAsync(string fileName, string id);
        Task<MyDataEntity> CreateEntityAsync(MyDataEntity entity);

        Task<MyDataEntity> UpdateEntityAsync(MyDataEntity entity);

        Task DeleteEntityAsync(string fileName, string id);

        public Task<ICollection<T>> GetAllEntityAsync();
    }
}
