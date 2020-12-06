using Azure.Storage.Blobs;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class TranslationService : ITranslationService
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
            if (_cache.TryGetValue($"{lang.ToUpper()}/{key.ToUpper()}", out string value) && !string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
            else
            {
                throw new KeyNotFoundException("There is no match for given key!");
            }
        }

        public async Task ReadDictionariesIntoMemoryAsync(CancellationToken token)
        {
            var blobStorageConnectionString = Environment.GetEnvironmentVariable("bs_con_str", EnvironmentVariableTarget.Process);
            BlobServiceClient blobServiceClient = new BlobServiceClient(blobStorageConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient("translations");
            await foreach (var blobItem in containerClient.GetBlobsAsync().WithCancellation(token))
            {
                var blockBlob = containerClient.GetBlobClient(blobItem.Name);
                if (await blockBlob.ExistsAsync(token))
                {
                    using var stream = new MemoryStream();
                    await blockBlob.DownloadToAsync(stream, token);
                    string jsonText = Encoding.Default.GetString(stream.ToArray());
                    var options = new JsonSerializerOptions
                    {
                        ReadCommentHandling = JsonCommentHandling.Skip,
                        AllowTrailingCommas = true,
                    };
                    var jsonElement = JsonSerializer.Deserialize<JsonElement>(jsonText, options);

                    if (jsonElement.TryGetProperty("Language", out var lang))
                    {
                        var langCode = lang.GetString().ToUpper();
                        if (!string.IsNullOrWhiteSpace(langCode))
                        {
                            Console.WriteLine($"Lang: {langCode}");
                            foreach (var item in jsonElement.EnumerateObject())
                            {
                                if (item.Name == "Language") continue;
                                _cache.Set($"{langCode}/{item.Name.ToUpper()}", item.Value.GetString());
                            }
                        }
                    }
                }
            }
        }
    }
}
