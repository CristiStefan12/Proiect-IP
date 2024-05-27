using Guna.UI2.WinForms;
using Ollama;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Guna.UI2.Native.WinApi;


/**************************************************************************
 *                                                                        *
 *  Description: <insert description here>                                *
 *  Website:     https://pushi.party                                      *
 *  Copyright:   (c) 2024 Cristian-Mihai Stefan                           *
 *  SPDX-License-Identifier: AGPL-3.0-only                                *
 *                                                                        *
 **************************************************************************/


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

        /// <summary>
        /// Handles the click event of the `guna2Button1` button.
        /// Gathers selected items from `listBox1` and the checked radio button from `guna2GroupBox1`,
        /// sends a query to the `OllamaAdaptor` to get cooking instructions, and displays the result in `guna2TextBox1`.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            guna2TextBox1.Text = "";

            string listBoxItemsStr = String.Join(", ", listBox1.Items
                 .OfType<object>()
                 .Select(item => item.ToString())
                 .ToArray()
             );

            var selectedRecipe = guna2GroupBox1.Controls.OfType<Guna2RadioButton>().FirstOrDefault(r => r.Checked);

            if (selectedRecipe == null)
            {
                MessageBox.Show("Select at least one recipe!");

                return;
            }

            var instructionsOllamaAdaptor = new OllamaAdaptor<InstructionsResponse>(
                $"{selectedRecipe.Text}; ingredients: {listBoxItemsStr}",
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

            try
            {
                MessageBox.Show("Started query to Ollama, please wait...");

                var instructionsResponse = await getInstructions.Run();

                guna2TextBox1.Text = String.Join(Environment.NewLine, instructionsResponse.instructions.Select(i => "- " + i));
            }
            catch (Exception _)
            {
                MessageBox.Show("Error while querying Ollama...");
            }
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

        /// <summary>
        /// Handles the click event of the `guna2CircleButton1` button.
        /// Adds the text from `textBox1` to `listBox1` if it is not already present,
        /// then clears the text in `textBox1`.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length < 1)
            {
                MessageBox.Show("Enter ingredient name!");

                return;
            }

            if (listBox1.Items.Contains(textBox1.Text))
            {
                MessageBox.Show("Ingredient already exists!");
            } else
            {
                listBox1.Items.Add(textBox1.Text);
            }

            textBox1.Text = "";
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the `listBox1` control.
        /// Updates the text in `textBox1` to the currently selected item in `listBox1`.
        /// If no item is selected, clears the text in `textBox1`.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
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

        /// <summary>
        /// Handles the click event of the `guna2Button2` button.
        /// Collects items from `listBox1`, sends a query to generate creative recipe names using the specified ingredients,
        /// and updates `guna2GroupBox1` with the generated recipe names as radio buttons.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
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

            try
            {
                MessageBox.Show("Started query to Ollama, please wait...");

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
            catch (Exception _)
            {
                MessageBox.Show("Error while querying Ollama...");
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Project realized by Andrei-Cristinel Vieru, Sabina Nadejda Barila, Maria Agape and Cristian-Mihai Ștefan.");
        }
    }
}
