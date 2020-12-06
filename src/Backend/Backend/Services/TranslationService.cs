using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class TranslationService: ITranslationService
    {
        private readonly IMemoryCache _cache;

        public TranslationService(IMemoryCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Returns translation by given key and lang code. 
        /// If it doesn't find a match, it throws an exception.
        /// </summary>
        /// <param name="lang">lang : fi, en</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        public string GetTranslation(string lang, string key)
        {
            if (_cache.TryGetValue($"{lang}/{key}", out string value) && !string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
            else
            {
                throw new KeyNotFoundException("There is no match for given key!");
            }
        }

        public Task ReadDictionariesIntoMemory()
        {
            _cache.Set("fi/Common_OKButtonText", "FI_OK");
            _cache.Set("fi/Common_CancelButtonText", "FI_Cancel");
            return Task.CompletedTask;
        }
    }
}
