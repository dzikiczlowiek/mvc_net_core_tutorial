namespace SportStore.DataAccess.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    using StackExchange.Redis;

    public abstract class RedisObjectStore<T>
        where T : class
    {
        protected readonly IDatabase Database;

        protected readonly IServer Server;

        private readonly string @namespace;

        protected RedisObjectStore(IDatabase database, string @namespace)
        {
            this.@namespace = @namespace;
            Database = database;
            Server = Database.Multiplexer.GetServer(Database.Multiplexer.GetEndPoints().First());
            AddIndexes();
        }

        protected IDictionary<Type, IRedisSecondaryIndex<T>> Indexes { get; } =
            new Dictionary<Type, IRedisSecondaryIndex<T>>();

        public virtual long GetNextIdentityId()
        {
            return Database.StringIncrement(GenerateSequenceKey());
        }

        public bool Exist(long keySuffix)
        {
            return Exist(keySuffix.ToString());
        }

        public bool Exist(string keySuffix)
        {
            return Database.KeyExists(GenerateKey(keySuffix));
        }

        public virtual IEnumerable<T> GetAll(IEnumerable<string> keySuffixes)
        {
            var jsons = Database.StringGet(keySuffixes.Select(x => (RedisKey)x).ToArray());
            foreach (var json in jsons)
            {
                if (json.IsNullOrEmpty)
                {
                    throw new ArgumentNullException();
                }

                yield return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public T Get(long id)
        {
            return Get(id.ToString());
        }

        public T Get(string keySuffix)
        {
            var key = GenerateKey(keySuffix);
            var serializedObject = Database.StringGet(key);
            if (serializedObject.IsNullOrEmpty)
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(serializedObject.ToString());
        }

        public void Save(long id, T entity)
        {
            Save(id.ToString(), entity);
        }

        public virtual void Save(string keySuffix, T entity)
        {
            if (entity != null)
            {
                var data = JsonConvert.SerializeObject(entity);
                var key = GenerateKey(keySuffix);
                var getOriginal = Get(keySuffix);

                Database.StringSet(key, data);
                foreach (var index in Indexes)
                {
                    if (getOriginal != null)
                    {
                        index.Value.RemoveIndex(Database, getOriginal, key);
                    }
                    index.Value.AddIndex(Database, entity, key);
                }
            }
        }

        public void Delete(long id)
        {
            Delete(id.ToString());
        }

        public virtual void Delete(string keySuffix)
        {
            if (string.IsNullOrWhiteSpace(keySuffix) || keySuffix.Contains(":"))
            {
                throw new ArgumentException("invalid key");
            }

            var key = GenerateKey(keySuffix);
            var getOriginal = Get(keySuffix);
            Database.KeyDelete(key);

            foreach (var index in Indexes)
            {
                if (getOriginal != null)
                {
                    index.Value.RemoveIndex(Database, getOriginal, key);
                }
            }
        }

        protected virtual void AddIndexes()
        {
        }

        protected virtual string GenerateSequenceKey()
        {
            return $"{Constants.Namespace.Sequence}:{@namespace}";
        }

        protected virtual string GenerateKey(string keySuffix)
        {
            if (!keySuffix.StartsWith($"{@namespace}:"))
            {
                return $"{@namespace}:{keySuffix}";
            }

            return keySuffix;
        }
    }
}
