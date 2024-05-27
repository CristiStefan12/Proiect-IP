using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ollama
{
    public class OllamaAdaptor<T>
    {
        private string _prompt;
        private string _systemPrompt;

        public OllamaAdaptor(string prompt, string systemPrompt)
        {
            _prompt = prompt;
            _systemPrompt = systemPrompt.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ");
        }


        public async Task<T> RunQuery()
        {
            HttpClient client = new HttpClient();
            string url = "http://192.168.100.82:11434/api/generate";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent($@"
                {{
                  ""model"": ""llama3:8b-instruct-q8_0"",
                  ""prompt"": ""{_prompt}"",

                  ""format"": ""json"",
                  ""stream"": false,
              
                  ""system"":  ""{_systemPrompt}""
                }}
            ", Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            var jsonResponse = JsonSerializer.Deserialize<OllamaResponse>(responseBody);

            return JsonSerializer.Deserialize<T>(jsonResponse.response);
        }
    }

    // TODO: comment
    public class OllamaResponse
    {
        // TODO: comment
        public string response { get; set; }
        // Unused
        // public string model { get; set; }
        // public string created_at { get; set; }
        // public bool done { get; set; }
        // public int[] context { get; set; }
        // public int total_duration { get; set; }
        // public int load_duration { get; set; }
        // public int prompt_eval_count { get; set; }
        // public int prompt_eval_duration { get; set; }
        // public int eval_count { get; set; }
        // public int eval_duration { get; set; }
    }

    // TODO: comment
    public class IngredientsResponse
    {
        // TODO: comment
        public string[] ingredients { get; set; }
    }

    // TODO: comment
    public class RecipeNamesResponse
    {
        // TODO: comment
        public string[] recipeNames { get; set; }
    }

    // TODO: comment
    public class InstructionsResponse
    {
        // TODO: comment
        public string[] instructions { get; set; }
    }
}
