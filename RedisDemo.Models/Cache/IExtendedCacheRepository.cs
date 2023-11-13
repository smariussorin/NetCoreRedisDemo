namespace RedisDemo.Models.Cache
{
    public interface IExtendedCacheRepository<T>
    {
        Task ClearKeyLocalAsync(string key);
        Task ClearKeysLocalAsync(IEnumerable<string> keys);
        Task<bool> ContainsKeyAsync(string key);
        Task<ICollection<T>> GetAllAsync(string pattern);
        Task<T> GetValueAsync(string key);
        Task<ICollection<T>> GetValuesForKeysAsync(ICollection<string> keys);
        Task SetKeyAsync(string key, T value);
        Task SetKeysAsync(Dictionary<string, T> data);
    }
}