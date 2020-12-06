using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface ITranslationService
    {
        string GetTranslation(string lang, string key);

        Task ReadDictionariesIntoMemory();
    }
}
