
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace RedisDemo3.Cache
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cacheService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CacheService(IDistributedCache cacheService)
        {
            // Inizializza il servizio di cache distribuita
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        }

        public T? GetData<T>(string key)
        {
            // Ottiene i dati associati alla chiave specificata dal servizio di cache
            var value = _cacheService.GetString(key);

            // Deserializza i dati in base al tipo specificato
            return string.IsNullOrEmpty(value) ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public void RemoveData(string key)
        {
            // Rimuove i dati associati alla chiave specificata dal servizio di cache
            _cacheService.Remove(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationTime"></param>
        public void SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            // Imposta le opzioni per la memorizzazione dei dati nella cache
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = expirationTime
            };

            // Serializza e memorizza i dati nella cache
            _cacheService.SetString(key, JsonConvert.SerializeObject(value), options);
        }
    }
}
