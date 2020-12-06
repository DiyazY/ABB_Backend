using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Backend.Services;

namespace Backend
{
    public class Translations
    {
        private readonly ITranslationService _translationService;
        public Translations(ITranslationService translationService)
        {
            _translationService = translationService;
        }

        [FunctionName("Translations")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "translations/{lang}/{key}")] HttpRequest req,
            string lang,
            string key,
            ILogger log)
        {
            log.LogInformation($"lang: {lang}, key:{key} ");

            try
            {
                var translation = _translationService.GetTranslation(lang, key);
                log.LogInformation(translation);
                return new OkObjectResult(translation);
            }
            catch (Exception ex)
            {
                log.LogError($"The translation is not found! Key: {key}, Language: {lang}");
                return new NotFoundObjectResult(ex.Message);
            }
        }
    }
}
