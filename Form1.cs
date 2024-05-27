using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace WindowsFormsApp1
{



    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {

        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {

        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            string url = "http://192.168.100.82:11434/api/generate";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(@"
                {
                  ""model"": ""llama3:8b-instruct-fp16"",
                  ""prompt"": ""tomato sauce, cheese, parmesan"",

                  ""format"": ""json"",
                  ""stream"": false,
                  ""options"": {
                    ""num_predict"": -1
                  },

                  ""system"":  ""You are a master chef, an expert of food. Write 10 more ingredients commonly used in recipes with the ingredients provided. Respond only in JSON using this format. ```ts { ingredients: string[] } ```""
                }
            ", Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var jsonResponse = JsonSerializer.Deserialize<OllamaResponse>(responseBody);
            var llmResponseJson = JsonSerializer.Deserialize<IngredientsResponse>(jsonResponse.response);

            string a = "";

            for (int i = 0; i < llmResponseJson.ingredients.Length; i++)
            {
                a += llmResponseJson.ingredients[i] + Environment.NewLine;
            }

            guna2TextBox1.Text = a;
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }

    class OllamaResponse
    {
        public string model { get; set; }
        public string created_at { get; set; }
        public string response { get; set; }
        public bool done { get; set; }
        public int[] context { get; set; }
        //public int total_duration { get; set; }
        //public int load_duration { get; set; }
        //public int prompt_eval_count { get; set; }
        //public int prompt_eval_duration { get; set; }
        //public int eval_count { get; set; }
        //public int eval_duration { get; set; }
    }

    class IngredientsResponse
    {
        public string[] ingredients { get; set; }
    }

}
