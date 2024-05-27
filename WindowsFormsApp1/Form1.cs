﻿using Guna.UI2.WinForms;
using Ollama;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Guna.UI2.Native.WinApi;

namespace WindowsFormsApp1
{


    public partial class Form1 : Form
    {
        string[] hiddenIngredients;

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
            if (listBox1.Items.Contains(textBox1.Text))
            {
                listBox1.Items.Remove(textBox1.Text);
            }

            textBox1.Text = "";
        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {

        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            string listBoxItemsStr = String.Join(", ", listBox1.Items
                 .OfType<object>()
                 .Select(item => item.ToString())
                 .ToArray()
             );

            var selectedRecipe = guna2GroupBox1.Controls.OfType<Guna2RadioButton>().FirstOrDefault(r => r.Checked).Text;

            var instructionsOllamaAdaptor = new OllamaAdaptor<InstructionsResponse>(
                $"{selectedRecipe}; ingredients: {listBoxItemsStr}",
                @"
                  You are a master chef, an expert of food.
                    1. The recipe must have a minimum of 100 words worth of instructions in TEXT format
                    1a. Instructions must contain each step needed to create the recipe using ONLY the ingredients provided
                    1b. ONLY USE THE ingredients provided THAT ARE RELEVANT TO THE RECIPE, otherwise a puppy will die.
                    Respond only in JSON using this format
                    ```ts
                    {
                      instructions: string[]
                    }
                    ```
                "
             );

            var getInstructions = new Logger.OllamaLoggerDecorator<InstructionsResponse>(instructionsOllamaAdaptor.RunQuery);

            var instructionsResponse = await getInstructions.Run();

            guna2TextBox1.Text = String.Join(Environment.NewLine, instructionsResponse.instructions.Select(i => "- " + i));
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2GroupBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            if (!listBox1.Items.Contains(textBox1.Text))
            {
                listBox1.Items.Add(textBox1.Text);
            }

            textBox1.Text = "";
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                textBox1.Text = "";
            } else
            {
                textBox1.Text = listBox1.SelectedItem.ToString();
            }
        }

        private async void guna2Button2_Click(object sender, EventArgs e)
        {
            string listBoxItemsStr = String.Join(", ", listBox1.Items
             .OfType<object>()
             .Select(item => item.ToString())
             .ToArray()
            );

            //var ingredientsResponse = await new OllamaAdaptor<IngredientsResponse>(
            //    listBoxItemsStr,
            //    @"
            //        You are a master chef, an expert of food.
            //        Write 10 more ingredients commonly used in recipes with the ingredients provided.
            //        Respond only in JSON using this format.
            //        ```ts
            //        {
            //            ingredients: string[]
            //        }
            //        ```
            //    "
            // ).RunQuery();

            //hiddenIngredients = ingredientsResponse.ingredients;

            var recipeOllamaAdaptor = new OllamaAdaptor<RecipeNamesResponse>(
                listBoxItemsStr,
                @"
                    You are a master chef, an expert of food.
                    Create 10 recipe names that use ONLY the ingredients provided. Be very creative.
                    If you don't use the ingredients provided a puppy will die.
                    Respond only in JSON using this format
                    ```ts
                    {
                      recipeNames: string[]
                    }
                    ```

                "
             );

            var getRecipies = new Logger.OllamaLoggerDecorator<RecipeNamesResponse>(recipeOllamaAdaptor.RunQuery);

            var recipiesResponse = await getRecipies.Run();

            guna2GroupBox1.Controls.Clear();

            for (int i = 0; i < recipiesResponse.recipeNames.Length; i++)
            {
                var recipeName = recipiesResponse.recipeNames[i];

                guna2GroupBox1.Controls.Add(new Guna.UI2.WinForms.Guna2RadioButton
                {
                    Text = recipeName,
                    Location = new System.Drawing.Point(0, 40 * (i + 1)),
                    Width = 324,
                    Height = 40,
                });
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Project realized by Andrei-Cristinel Vieru, Sabina Nadejda Barila, Maria Agape and Cristian-Mihai Ștefan.");
        }
    }
}
