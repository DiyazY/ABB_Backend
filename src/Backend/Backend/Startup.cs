using Backend.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Backend.Startup))]

namespace Backend
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddMemoryCache();
            builder.Services.AddTransient<ITranslationService, TranslationService>();
        }
    }
}