using System;
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
        public async Task Run([TimerTrigger("*/1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            try
            {
                await _translationService.ReadDictionariesIntoMemory();
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex.StackTrace);
                // here the code may send some notification or just log the error somewhere 
            }
        }
    }
}
