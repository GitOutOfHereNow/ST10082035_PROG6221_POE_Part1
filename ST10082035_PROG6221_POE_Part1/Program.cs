using System;

namespace recipeApplication
{

    /// <summary>
    /// Student Number: ST10082035
    /// Student Name: Adrian Silver
    /// Module Code: PROG6221
    /// POE PART 1
    /// The recipe class stores the bulk of the applications processes in the form of multiple methods
    /// </summary>
    class Recipe
    {
        //declarations of variables and arrays
        private int numIngredients;
        private string[] ingredientNames;
        private double[] ingredientQuantities;
        private string[] ingredientUnits;
        private int numSteps;
        private string[] steps;
        private double[] originalQuantities;


        /// <summary>
        /// EnterRecipe() Method takes user input when called from the Main Method and fills in the major fields such as name of ingredients, quantity of ingredients, etc.
        /// </summary>
        public void EnterRecipe()
        {
            Console.Write("Enter the number of ingredients: ");
            numIngredients = int.Parse(Console.ReadLine());

            ingredientNames = new string[numIngredients];
            ingredientQuantities = new double[numIngredients];
            ingredientUnits = new string[numIngredients];
            originalQuantities = new double[numIngredients];

            //for loop to iterate details for name, quantity and unit of measurement based on how many ingredients were entered by the user
            //Additionally, there has been 1 more array declared, namely originalQuantities: this takes the original quantity that the user first enters for each ingredient in the ingredientQuantities array and stores it in a copy to revert to when a user resets to original values
            for (int i = 0; i < numIngredients; i++)
            {
                Console.Write("Enter the name of ingredient #{0}: ", i + 1);
                ingredientNames[i] = Console.ReadLine();


                Console.Write("Enter the quantity of ingredient #{0}: ", i + 1);
                ingredientQuantities[i] = double.Parse(Console.ReadLine());
                originalQuantities[i] = ingredientQuantities[i];

                Console.Write("Enter the unit of measurement of ingredient #{0}: ", i + 1);
                ingredientUnits[i] = Console.ReadLine();
            }

            Console.Write("Enter the number of steps: ");
            numSteps = int.Parse(Console.ReadLine());

            steps = new string[numSteps];

            for (int i = 0; i < numSteps; i++)
            {
                Console.Write("Enter step #{0}: ", i + 1);
                steps[i] = Console.ReadLine();
            }
        }
        //-----------------------------------------------------------EndOfMethod------------------------------------------------------------------------------


        /// <summary>
        /// this method displays the ingredients and steps, essentially showing the user everything that they have entered
        /// </summary>

        public void DisplayRecipe()
        {
            Console.WriteLine("Ingredients:");

            for (int i = 0; i < numIngredients; i++)
            {
                Console.WriteLine("- {0} {1} of {2}", ingredientQuantities[i], ingredientUnits[i], ingredientNames[i]);
            }

            Console.WriteLine("Steps:");

            for (int i = 0; i < numSteps; i++)
            {
                Console.WriteLine("{0}. {1}", i + 1, steps[i]);
            }
        }
        //-----------------------------------------------------------EndOfMethod------------------------------------------------------------------------------



        /// <summary>
        /// this method uses the variable "factor" to scale the quantites of the user's ingredients to either 0.5, 2 or 3 times
        /// </summary>
        /// <param name="factor"></param>


        public void ScaleRecipe(double factor)
        {
            for (int i = 0; i < numIngredients; i++)
            {
                ingredientQuantities[i] *= factor;
            }
        }
        //-----------------------------------------------------------EndOfMethod------------------------------------------------------------------------------
        /// <summary>
        /// this method resets the quantites if either after a user scales their quantities or changes their quantites to the originally entered quantities
        /// </summary>
        public void ResetQuantities()
        {
            for (int i = 0; i < numIngredients; i++)
            {
                // Restore the original quantities
                if (ingredientQuantities[i] != originalQuantities[i])
                {

                    ingredientQuantities[i] = originalQuantities[i];

                }
            }
            //-----------------------------------------------------------EndOfMethod------------------------------------------------------------------------------
        }


        /// <summary>
        /// this method reverts every input and sets it to original before any user entry was made
        /// </summary>
        public void ClearRecipe()
        {
            numIngredients = 0;
            ingredientNames = null;
            ingredientQuantities = null;
            ingredientUnits = null;
            numSteps = 0;
            steps = null;

        }
        //-----------------------------------------------------------EndOfMethod------------------------------------------------------------------------------
    }
    /// <summary>
    /// =============================================================END OF CLASS==============================================================================    /// </summary>

    class Program
    {
        /// <summary>
        /// beginning of Main Method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Recipe recipe = new Recipe();

            while (true)
            {
                Console.WriteLine("Enter a command:");
                Console.WriteLine("1. Enter a new recipe");
                Console.WriteLine("2. Display the recipe");
                Console.WriteLine("3. Scale the recipe");
                Console.WriteLine("4. Reset the quantities");
                Console.WriteLine("5. Clear the recipe");
                Console.WriteLine("6. Exit");

                int command = int.Parse(Console.ReadLine());

                switch (command)
                {
                    case 1:
                        recipe.EnterRecipe();
                        break;
                    case 2:
                        recipe.DisplayRecipe();
                        break;
                    case 3:
                        Console.Write("Enter the scale factor: ");
                        double factor = double.Parse(Console.ReadLine());
                        recipe.ScaleRecipe(factor);
                        break;
                    case 4:
                        recipe.ResetQuantities();
                        break;
                    case 5:
                        recipe.ClearRecipe();
                        break;
                    case 6:
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
        //-----------------------------------------------------------EndOfMethod------------------------------------------------------------------------------
    }
    //===============================================================END_OF_FILE===============================================================================
}
