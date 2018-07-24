using System.Collections.Generic;

namespace AlexanderIvanov.ShanoMVVM.Infrastructure
{
    public static class DictionaryExtensions
    {
        public static TValue AddNew<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
        {
            TValue value = new TValue();
            dictionary.Add(key, value);
            return value;
        }
    }
}
