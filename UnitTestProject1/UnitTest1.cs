using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Ollama;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

/**************************************************************************
 *                                                                        *
 *  Description: Unit tests                                               *
 *  Website:     https://pushi.party                                      *
 *  Copyright:   (c) 2024 Sabina Nadejda Barila                           *
 *  SPDX-License-Identifier: AGPL-3.0-only                                *
 *                                                                        *
 **************************************************************************/

namespace OllamaTests
{
    [TestClass]
    public class OllamaAdaptorTests
    {
        [TestMethod]
        public void TestInitializationWithValidPrompts()
        {
            var adaptor = new OllamaAdaptor<string>("testPrompt", "testSystemPrompt");
            Assert.IsNotNull(adaptor);
        }

        [TestMethod]
        public void TestSystemPromptFormatting()
        {
            var systemPrompt = "line1\r\nline2\nline3\rline4";
            var adaptor = new OllamaAdaptor<string>("testPrompt", systemPrompt);
            var formattedSystemPrompt = typeof(OllamaAdaptor<string>)
                .GetField("_systemPrompt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(adaptor) as string;

            Assert.AreEqual("line1 line2 line3 line4", formattedSystemPrompt);
        }

        [TestMethod]
        public async Task TestRunQueryWithValidPrompts()
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"response\":\"{}\"}")
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var adaptor = new OllamaAdaptor<string>("testPrompt", "testSystemPrompt");

            var response = await adaptor.RunQuery();

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void TestOllamaResponseDeserialization()
        {
            var jsonResponse = "{\"response\":\"testResponse\"}";
            var ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(jsonResponse);

            Assert.IsNotNull(ollamaResponse);
            Assert.AreEqual("testResponse", ollamaResponse.response);
        }

        [TestMethod]
        public void TestIngredientsResponseDeserialization()
        {
            var jsonResponse = "{\"ingredients\":[\"sugar\",\"flour\"]}";
            var ingredientsResponse = JsonSerializer.Deserialize<IngredientsResponse>(jsonResponse);

            Assert.IsNotNull(ingredientsResponse);
            CollectionAssert.AreEqual(new[] { "sugar", "flour" }, ingredientsResponse.ingredients);
        }

        [TestMethod]
        public void TestRecipeNamesResponseDeserialization()
        {
            var jsonResponse = "{\"recipeNames\":[\"Cake\",\"Pie\"]}";
            var recipeNamesResponse = JsonSerializer.Deserialize<RecipeNamesResponse>(jsonResponse);

            Assert.IsNotNull(recipeNamesResponse);
            CollectionAssert.AreEqual(new[] { "Cake", "Pie" }, recipeNamesResponse.recipeNames);
        }

        [TestMethod]
        public void TestInstructionsResponseDeserialization()
        {
            var jsonResponse = "{\"instructions\":[\"Mix ingredients\",\"Bake\"]}";
            var instructionsResponse = JsonSerializer.Deserialize<InstructionsResponse>(jsonResponse);

            Assert.IsNotNull(instructionsResponse);
            CollectionAssert.AreEqual(new[] { "Mix ingredients", "Bake" }, instructionsResponse.instructions);
        }

        [TestMethod]
        public async Task TestRunQueryWithEmptyPrompts()
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"response\":\"{}\"}")
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var adaptor = new OllamaAdaptor<string>("", "", httpClient);

            var response = await adaptor.RunQuery();

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task TestRunQueryWithNullPrompts()
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"response\":\"{}\"}")
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var adaptor = new OllamaAdaptor<string>(null, null, httpClient);

            var response = await adaptor.RunQuery();

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task TestRunQueryResponseDeserialization()
        {
            var jsonResponse = @"{""response"":{""key"":""value""}}";
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var adaptor = new OllamaAdaptor<Dictionary<string, string>>("testPrompt", "testSystemPrompt", httpClient);

            var response = await adaptor.RunQuery();

            Assert.IsNotNull(response);
            Assert.AreEqual("value", response["key"]);
        }

        [TestMethod]
        public async Task TestSuccessfulApiResponseHandling()
        {
            var jsonResponse = @"{""response"":{""key"":""value""}}";
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var adaptor = new OllamaAdaptor<Dictionary<string, string>>("testPrompt", "testSystemPrompt", httpClient);

            var response = await adaptor.RunQuery();

            Assert.IsNotNull(response);
            Assert.AreEqual("value", response["key"]);
        }

        [TestMethod]
        public async Task TestUnsuccessfulApiResponseHandling()
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var adaptor = new OllamaAdaptor<string>("testPrompt", "testSystemPrompt", httpClient);

            await Assert.ThrowsExceptionAsync<HttpRequestException>(async () => await adaptor.RunQuery());
        }

        [TestMethod]
        public async Task TestHttpRequestContentTypeHeader()
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"response\":\"{}\"}")
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var adaptor = new OllamaAdaptor<string>("testPrompt", "testSystemPrompt", httpClient);

            var response = await adaptor.RunQuery();

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Content.Headers.ContentType.MediaType == "application/json"),
                ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod]
        public async Task TestHttpRequestMethod()
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"response\":\"{}\"}")
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var adaptor = new OllamaAdaptor<string>("testPrompt", "testSystemPrompt", httpClient);

            var response = await adaptor.RunQuery();

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod]
        public async Task TestJsonPayloadStructure()
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"response\":\"{}\"}")
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var adaptor = new OllamaAdaptor<string>("testPrompt", "testSystemPrompt", httpClient);

            var response = await adaptor.RunQuery();

            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Content.ReadAsStringAsync().Result.Contains("\"model\":\"llama3:8b-instruct-q8_0\"") &&
                    req.Content.ReadAsStringAsync().Result.Contains("\"prompt\":\"testPrompt\"") &&
                    req.Content.ReadAsStringAsync().Result.Contains("\"format\":\"json\"") &&
                    req.Content.ReadAsStringAsync().Result.Contains("\"stream\":false") &&
                    req.Content.ReadAsStringAsync().Result.Contains("\"system\":\"testSystemPrompt\"")),
                ItExpr.IsAny<CancellationToken>());
        }

        [TestMethod]
        public async Task TestEnsureSuccessStatusCodeBehavior()
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var adaptor = new OllamaAdaptor<string>("testPrompt", "testSystemPrompt", httpClient);

            await Assert.ThrowsExceptionAsync<HttpRequestException>(async () => await adaptor.RunQuery());
        }

        [TestMethod]
        public void TestMultipleLineBreaksInSystemPrompt()
        {
            var systemPrompt = "line1\n\nline2\r\n\rline3";
            var adaptor = new OllamaAdaptor<string>("testPrompt", systemPrompt);
            var formattedSystemPrompt = typeof(OllamaAdaptor<string>)
                .GetField("_systemPrompt", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(adaptor) as string;

            Assert.AreEqual("line1  line2  line3", formattedSystemPrompt);
        }

        [TestMethod]
        public async Task TestDeserializationToDifferentTypes()
        {
            var jsonResponse = @"{""response"" :{""ingredients"": [""sugar"",""flour""]}}";
            var mockHandler = new Mock<HttpMessageHandler>();

            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            var httpClient = new HttpClient(mockHandler.Object);
            var adaptor = new OllamaAdaptor<IngredientsResponse>("testPrompt", "testSystemPrompt", httpClient);

            var response = await adaptor.RunQuery();

            Assert.IsNotNull(response);
            CollectionAssert.AreEqual(new[] { "sugar", "flour" }, response.ingredients);
        }
    }
}
