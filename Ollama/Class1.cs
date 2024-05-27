using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

/**************************************************************************
 *                                                                        *
 *  Description: <insert description here>                                *
 *  Website:     https://pushi.party                                      *
 *  Copyright:   (c) 2024 Andrei-Cristinel Vieru                          *
 *  SPDX-License-Identifier: AGPL-3.0-only                                *
 *                                                                        *
 **************************************************************************/

namespace Ollama
{
    public class OllamaAdaptor<T>
    {
        private readonly string _prompt;
        private readonly string _systemPrompt;
        private readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Initializes a new instance of the <see cref="OllamaAdaptor{T}"/> class with the specified prompts.
        /// </summary>
        /// <param name="prompt">The prompt to be used in the query.</param>
        /// <param name="systemPrompt">The system prompt to be used in the query, with line breaks replaced by spaces.</param>
        public OllamaAdaptor(string prompt, string systemPrompt, HttpClient httpClient)
        {
            _prompt = prompt;
            _systemPrompt = systemPrompt.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ");
            _httpClient = httpClient;
        }

        public OllamaAdaptor(string prompt, string systemPrompt)
        {
            _prompt = prompt;
            _systemPrompt = systemPrompt.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ");
        }

        /// <summary>
        /// Sends an HTTP POST request to the specified API endpoint to generate a response based on the given prompt and system prompt.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, with a result of type <typeparamref name="T"/>.</returns>
        public async Task<T> RunQuery()
        { 
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

            HttpResponseMessage response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            var jsonResponse = JsonSerializer.Deserialize<OllamaResponse>(responseBody);

            return JsonSerializer.Deserialize<T>(jsonResponse.response);
        }
    }

    /// <summary>
    /// Represents a response from the Ollama API.
    /// </summary>
    public class OllamaResponse
    {
        /// <summary>
        /// Gets or sets the main response content.
        /// </summary>
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

    /// <summary>
    /// Represents a response containing a list of ingredients.
    /// </summary>
    public class IngredientsResponse
    {  /// <summary>
       /// Gets or sets the list of ingredients.
       /// </summary>

        public string[] ingredients { get; set; }
    }

    /// <summary>
    /// Represents a response containing a list of recipe names.
    /// </summary>
    public class RecipeNamesResponse
    {
        /// <summary>
        /// Gets or sets the list of recipe names.
        /// </summary>
        public string[] recipeNames { get; set; }
    }

    /// <summary>
    /// Represents a response containing a list of instructions.
    /// </summary>
    public class InstructionsResponse
    {

        /// <summary>
        /// Gets or sets the list of instructions.
        /// </summary>
        public string[] instructions { get; set; }
    }
}
