using Microsoft.Extensions.Caching.Memory;
using Moq;
using Backend.Services;
using Xunit;
using MemoryCache.Testing.Moq;
using System.Collections.Generic;
using System;

namespace Backend.Tests
{
    public class TranslationServiceTest
    {
        [Theory]
        [InlineData("en", "Common_CancelButtonText", "Cancel")]
        [InlineData("en", "Common_OKButtonText", "OK")]
        [InlineData("fi", "Common_CancelButtonText", "FI_Cancel")]
        [InlineData("fi", "Common_OKButtonText", "FI_OK")]
        public void GetTranslation_Assert_Returned_Value(string langTest, string keyTest, string resultTest)
        {
            // arrange 
            var cahceMock = Create.MockedMemoryCache();
            cahceMock.Set("en/Common_CancelButtonText".ToUpper(), "Cancel");
            cahceMock.Set("en/Common_OKButtonText".ToUpper(), "OK");
            cahceMock.Set("fi/Common_CancelButtonText".ToUpper(), "FI_Cancel");
            cahceMock.Set("fi/Common_OKButtonText".ToUpper(), "FI_OK");

            ITranslationService translationService = new TranslationService(cahceMock);

            // act
            var result = translationService.GetTranslation(langTest, keyTest);

            // assert
            Assert.Equal(result, resultTest);
        }

        [Theory]
        [InlineData("en", "CancelButtonText", "Cancel")]
        [InlineData("en", "Common_OK", "OK")]
        [InlineData("fi", "CancelButtonText", "FI_Cancel")]
        [InlineData("fi", "OKButtonText", "FI_OK")]
        public void GetTranslation_ThrowException(string langTest, string keyTest, string resultTest)
        {
            // arrange 
            var cahceMock = Create.MockedMemoryCache();
            cahceMock.Set("en/Common_CancelButtonText".ToUpper(), "Cancel");
            cahceMock.Set("en/Common_OKButtonText".ToUpper(), "OK");
            cahceMock.Set("fi/Common_CancelButtonText".ToUpper(), "FI_Cancel");
            cahceMock.Set("fi/Common_OKButtonText".ToUpper(), "FI_OK");

            ITranslationService translationService = new TranslationService(cahceMock);
            string result = "";
            // act
            Exception ex = Assert.Throws<KeyNotFoundException>(() =>
            {
                result = translationService.GetTranslation(langTest, keyTest);
            });

            // assert
            Assert.Empty(result);
            Assert.Equal("There is no match for given key!", ex.Message);
        }
    }
}
