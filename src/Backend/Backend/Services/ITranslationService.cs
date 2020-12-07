using System.Threading;
using System.Threading.Tasks;

namespace Backend.Services
{
    public interface ITranslationService
    {
        string GetTranslation(string lang, string key);

        Task ReadDictionariesIntoMemoryAsync(CancellationToken token);
    }
}
