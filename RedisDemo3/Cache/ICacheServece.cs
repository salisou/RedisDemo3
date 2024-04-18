namespace RedisDemo3.Cache
{
    public interface ICacheService
    {
        /// <summary>
        /// Ottiene i dati associati alla chiave specificata.
        /// </summary>
        /// <typeparam name="T">Il tipo di dati da restituire.</typeparam>
        /// <param name="key">La chiave associata ai dati da recuperare.</param>
        /// <returns>I dati associati alla chiave specificata.</returns>
        T? GetData<T>(string key);

        /// <summary>
        /// Memorizza i dati associati alla chiave specificata.
        /// </summary>
        /// <typeparam name="T">Il tipo di dati da memorizzare.</typeparam>
        /// <param name="key">La chiave associata ai dati da memorizzare.</param>
        /// <param name="value">I dati da memorizzare.</param>
        /// <param name="expirationTime">Il momento in cui i dati memorizzati dovrebbero scadere.</param>
        void SetData<T>(string key, T value, DateTimeOffset expirationTime);

        /// <summary>
        /// Rimuove i dati associati alla chiave specificata.
        /// </summary>
        /// <param name="key">La chiave associata ai dati da rimuovere.</param>
        void RemoveData(string key);
    }
}
