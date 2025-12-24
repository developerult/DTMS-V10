using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace NGL.Test.Core
{
    public class DBWrapper
    {


    }



    public sealed class Injector
    {
        private readonly ConcurrentDictionary<string, Func<object>> _dictionary;

        // Private constructor
        private Injector()
        {
            _dictionary = new ConcurrentDictionary<string, Func<object>>();
        }

        // Thread-safe singleton instance
        private static readonly Lazy<Injector> _instance =
            new Lazy<Injector>(() => new Injector());

        public static Injector Instance => _instance.Value;

        public void SetObject(string key, Func<object> factory)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            _dictionary[key] = factory;
        }

        public T GetObject<T>(string key, Func<T> defaultFactory)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key cannot be null or empty.", nameof(key));

            if (_dictionary.TryGetValue(key, out var factory))
            {
                var result = factory();

                // Ensure the result can be cast to the requested type
                if (result is T typedResult)
                {
                    return typedResult;
                }

                throw new InvalidCastException($"The object registered with key '{key}' cannot be cast to type {typeof(T).FullName}.");
            }

            // Use the provided default factory if no override exists
            if (defaultFactory == null)
                throw new ArgumentNullException(nameof(defaultFactory));

            return defaultFactory();
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public int Count()
        {
            return _dictionary.Count;
        }
    }

}
