using System;
using System.Threading;
using System.Threading.Tasks;
using Backend.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Backend
{
    public class Dictionaries
    {
        private readonly ITranslationService _translationService;

        public Dictionaries(ITranslationService translationService)
        {
            _translationService = translationService;
        }

        [FunctionName("Dictionaries")]
        public async Task Run([TimerTrigger("0 */6 * * *")]TimerInfo myTimer, ILogger log)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            try
            {
                await _translationService.ReadDictionariesIntoMemoryAsync(token);
            }
            catch (Exception ex)
            {
                tokenSource.Cancel();
                log.LogError(ex.Message, ex.StackTrace);
                // here the code may send some notification or just log the error somewhere 
            }
        }
    }
}
