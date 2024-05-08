using BaseLibrary.Responses;

namespace ServerLibrary.Repositories.Contracts
{
    public interface IGenericRepository<T>
    {
        Task<ICollection<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<GeneralResponse> InsertAsync(T item);
        Task<GeneralResponse> UpdateAsync(T item);
        Task<GeneralResponse> DeleteByIdAsync(int id);
    }
}