using System.Collections.Generic;

namespace Backend.Auxiliary
{
    public static class DictionaryExtender
    {
        /// <summary>
        /// merges dictionaries into
        /// </summary>
        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
            this Dictionary<TKey, TValue> src,
            Dictionary<TKey, TValue> arg)
            where TKey : notnull
        {
            foreach (var entry in arg)
                src.Add(entry.Key, entry.Value);

            return src;
        }
    }
}
