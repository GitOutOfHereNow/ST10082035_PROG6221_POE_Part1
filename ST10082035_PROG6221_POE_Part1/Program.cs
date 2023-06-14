using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks.Dataflow;

namespace RecipeApp
{
    //============================= Delegate that will later notify the user when a recipe exceeds 300 calories ============================================
    public delegate void RecipeCaloriesExceededEventHandler();

    // Enum for food groups
    public enum FoodGroup
    {
        StarchyFoods,
        VegetablesAndFruits,
        DryBeansPeasLentilsSoya,
        ChickenFishMeatEggs,
        MilkDairyProducts,
        FatsAndOil,
        Water
    }
    //===============================   Ingredient Class   ===================================================================================================

    public class Ingredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public int Calories { get; set; }
        public FoodGroup FoodGroup { get; set; }
    }
    //============================EndOfClass============================================================================================================

    //=========================== Recipe class =========================================================================================================
    public class Recipe
    {
        private List<Ingredient> ingredients;
        private List<string> steps;

        public event RecipeCaloriesExceededEventHandler CaloriesExceeded;

        public string Name { get; set; }
        public List<Ingredient> Ingredients
        {
            get { return ingredients; }
            set { ingredients = value; }
        }
        public List<string> Steps
        {
            get { return steps; }
            set { steps = value; }
        }
        public double ScaleFactor { get; set; }

        public Recipe()
        {
            Ingredients = new List<Ingredient>();
            Steps = new List<string>();
            ScaleFactor = 1;
        }

        //========================== Method to calculate total calories of the recipe ==============================================================
        public int CalculateTotalCalories()
        {
            int totalCalories = 0;
            foreach (Ingredient ingredient in Ingredients)
            {
                totalCalories += ingredient.Calories;
            }
            return totalCalories;
        }
        //==================== EndOfMethod =========================================================================================================

        //======================= Method to check if the recipe exceeds 300 calories================================================================
        public void CheckCaloriesExceeded()
        {
            if (CalculateTotalCalories() > 300)
            {
                if (CaloriesExceeded != null)
                {
                    CaloriesExceeded.Invoke();
                }
            }
        }
    }
    //============================EndOfClass============================================================================================================


    //=========================== RecipeApp class =====================================================================================================
    public class RecipeApp
    {
        private List<Recipe> recipes;

        public RecipeApp()
        {
            recipes = new List<Recipe>();
        }
        //==================== EndOfMethod =========================================================================================================

        //======================================= Method to add a recipe ===========================================================================
        public void AddRecipe(Recipe recipe)
        {
            recipes.Add(recipe);
        }
        //==================== EndOfMethod =========================================================================================================

        // =======================================Method to display all recipes recorded ===========================================================
        public void DisplayRecipes()
        {
            if (recipes.Count == 0)
            {
                Console.WriteLine("No recipes found.");
                return;
            }

            Console.WriteLine("Recipes:");
            for (int i = 0; i < recipes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {recipes[i].Name}");
            }

            Console.Write("Enter the number of the recipe to view: ");
            int recipeIndex = Convert.ToInt32(Console.ReadLine());

            if (recipeIndex >= 1 && recipeIndex <= recipes.Count)
            {
                Recipe selectedRecipe = recipes[recipeIndex - 1];
                Console.WriteLine("Recipe: " + selectedRecipe.Name);
                Console.WriteLine("Ingredients:");
                foreach (Ingredient ingredient in selectedRecipe.Ingredients)
                {
                    Console.WriteLine("- " + ingredient.Name + ": " + ingredient.Quantity + " " + ingredient.Unit);
                }
                Console.WriteLine("Total Calories: " + selectedRecipe.CalculateTotalCalories());
                Console.WriteLine("Steps:");
                for (int i = 0; i < selectedRecipe.Steps.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + selectedRecipe.Steps[i]);
                }
            }
            else
            {
                Console.WriteLine("Invalid recipe number.");
            }
        }
        //==================== EndOfMethod =========================================================================================================

        //============================== Method to display a single recipe by searching by name ====================================================
        public void DisplayRecipe(string recipeName)
        {
            Recipe recipe = GetRecipeByName(recipeName);

            if (recipe != null)
            {
                Console.WriteLine("Recipe: " + recipe.Name);
                Console.WriteLine("Ingredients:");
                foreach (Ingredient ingredient in recipe.Ingredients)
                {
                    Console.WriteLine("- " + ingredient.Name + ": " + ingredient.Quantity + " " + ingredient.Unit);
                }
                Console.WriteLine("Total Calories: " + recipe.CalculateTotalCalories());
                Console.WriteLine("Steps:");
                for (int i = 0; i < recipe.Steps.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + recipe.Steps[i]);
                }
            }
            else
            {
                Console.WriteLine("Recipe not found!");
            }
        }
        //==================== EndOfMethod =========================================================================================================

        //============================= Method to scale a recipe ===================================================================================
        public void ScaleRecipe(string recipeName, double factor)
        {
            Recipe recipe = GetRecipeByName(recipeName);
            if (recipe != null)
            {
                Console.WriteLine($"Scaling options for recipe '{recipe.Name}':");
                Console.WriteLine("1. Half");
                Console.WriteLine("2. Double");
                Console.WriteLine("3. Triple");
                Console.Write("Enter the number of the scale option: ");
                string scaleOption = Console.ReadLine();

                switch (scaleOption)
                {
                    case "1":
                        factor = 0.5;
                        break;
                    case "2":
                        factor = 2.0;
                        break;
                    case "3":
                        factor = 3.0;
                        break;
                    default:
                        Console.WriteLine("Invalid scale option. Recipe not scaled.");
                        return;
                }

                foreach (Ingredient ingredient in recipe.Ingredients)
                {
                    ingredient.Quantity *= factor;
                    ingredient.Calories = (int)(ingredient.Calories * factor);
                }
                Console.WriteLine("Recipe scaled successfully.");
            }
            else
            {
                Console.WriteLine("Recipe not found!");
            }
        }
        //==================== EndOfMethod =========================================================================================================

        //============================ Method to reset the recipe quantities to original values ====================================================
        public void ResetRecipe(string recipeName)
        {
            Recipe recipe = GetRecipeByName(recipeName);

            if (recipe != null)
            {
                double resetFactor = 1 / recipe.ScaleFactor;

                foreach (Ingredient ingredient in recipe.Ingredients)
                {
                    ingredient.Quantity *= resetFactor;

                }

                recipe.ScaleFactor = 1;

                Console.WriteLine("Recipe quantities reset successfully!");
            }
            else
            {
                Console.WriteLine("Recipe not found!");
            }
        }
        //==================== EndOfMethod =========================================================================================================

        //==================== Method to clear the data of a recipe ================================================================================
        public void ClearRecipe(string recipeName)
        {
            Recipe recipe = GetRecipeByName(recipeName);

            if (recipe != null)
            {
                recipe.Ingredients.Clear();
                recipe.Steps.Clear();
                recipe.ScaleFactor = 1;

                Console.WriteLine("Recipe cleared successfully!");
            }
            else
            {
                Console.WriteLine("Recipe not found!");
            }
        }

        //==================== EndOfMethod =========================================================================================================

        //==================== Helper method to get a recipe by name ===============================================================================
        private Recipe GetRecipeByName(string recipeName)
        {
            return recipes.Find(recipe => recipe.Name.Equals(recipeName));
        }


    }
    //==================== EndOfMethod =========================================================================================================

    //============================EndOfClass============================================================================================================

    // ========================= Unit test for total calorie calculation ===========================================================================
    public class RecipeUnitTest
    {
        public static void TestTotalCaloriesCalculation()
        {
            Recipe recipe = new Recipe();
            recipe.Ingredients.Add(new Ingredient { Name = "Sugar", Quantity = 2, Unit = "tablespoons", Calories = 100 });
            recipe.Ingredients.Add(new Ingredient { Name = "Flour", Quantity = 1, Unit = "cup", Calories = 150 });
            recipe.Ingredients.Add(new Ingredient { Name = "Butter", Quantity = 3, Unit = "tablespoons", Calories = 200 });

            int totalCalories = recipe.CalculateTotalCalories();

            Console.WriteLine("Total calories: " + totalCalories);
        }
    }
    //============================EndOfClass============================================================================================================
    //============================ Program Class containing the Main Method () =========================================================================
    class Program
    {
        static void Main(string[] args)
        {
            RecipeApp recipeApp = new RecipeApp();

            RunRecipeApp(recipeApp);
        }

        //====================================== Method to run the Recipe App ====================================================================
        static void RunRecipeApp(RecipeApp recipeApp)
        {
            while (true)
            {
                Console.WriteLine("==========================================================================================================");
                Console.WriteLine("============================WELCOME TO THE RECIPE CREATION APPLICATION====================================");
                Console.WriteLine("==========================================================================================================");
                Console.WriteLine("1. Add Recipe");
                Console.WriteLine("2. Display Recipes");
                Console.WriteLine("3. Display Recipe");
                Console.WriteLine("4. Scale Recipe");
                Console.WriteLine("5. Reset Recipe Quantities");
                Console.WriteLine("6. Clear Recipe");
                Console.WriteLine("7. Exit");
                Console.Write("Enter your choice: ");
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        AddRecipe(recipeApp);
                        break;
                    case 2:
                        recipeApp.DisplayRecipes();
                        break;
                    case 3:
                        Console.Write("Enter recipe name: ");
                        string recipeName = Console.ReadLine();
                        recipeApp.DisplayRecipe(recipeName);
                        break;
                    case 4:
                        ScaleRecipe(recipeApp);
                        break;
                    case 5:
                        ResetRecipeQuantities(recipeApp);
                        break;
                    case 6:
                        ClearRecipe(recipeApp);
                        break;

                    case 7:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }

                Console.WriteLine();
            }
        }
        //==================== EndOfMethod =========================================================================================================

        //======================= Method to add a recipe ===========================================================================================
        static void AddRecipe(RecipeApp recipeApp)
        {
            Console.Write("Enter recipe name: ");
            string recipeName = Console.ReadLine();

            Recipe recipe = new Recipe { Name = recipeName };

            Console.Write("Enter the number of ingredients: ");
            int ingredientCount = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < ingredientCount; i++)
            {
                Console.WriteLine($"Enter details for Ingredient #{i + 1}");
                Ingredient ingredient = new Ingredient();

                Console.Write("Enter ingredient name: ");
                ingredient.Name = Console.ReadLine();

                Console.Write("Enter quantity: ");
                ingredient.Quantity = Convert.ToDouble(Console.ReadLine());

                Console.Write("Enter unit of measurement: ");
                ingredient.Unit = Console.ReadLine();

                Console.Write("Enter number of calories: ");
                ingredient.Calories = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter food group number: " +
                    "\r\n(1) Starchy Foods\r\n(2) Vegetables And Fruits\r\n(3) Dry Beans, Peas, Lentils, Soya\r\n(4) Chicken, Fish, Meat, Eggs\r\n(5) Milk, Dairy Products\r\n(6) Fats And Oil\r\n(7) Water ");

                int foodGroupNum = Convert.ToInt32(Console.ReadLine());
                switch (foodGroupNum)
                {
                    case 1:
                        ingredient.FoodGroup = (FoodGroup)Enum.Parse(typeof(FoodGroup), "StarchyFoods");
                        recipe.Ingredients.Add(ingredient);
                        break;
                    case 2:
                        ingredient.FoodGroup = (FoodGroup)Enum.Parse(typeof(FoodGroup), "VegetablesAndFruits");
                        recipe.Ingredients.Add(ingredient);

                        break;
                    case 3:
                        ingredient.FoodGroup = (FoodGroup)Enum.Parse(typeof(FoodGroup), "DryBeansPeasLentilsSoya");
                        recipe.Ingredients.Add(ingredient);

                        break;
                    case 4:
                        ingredient.FoodGroup = (FoodGroup)Enum.Parse(typeof(FoodGroup), "ChickenFishMeatEggs");
                        recipe.Ingredients.Add(ingredient);

                        break;
                    case 5:
                        ingredient.FoodGroup = (FoodGroup)Enum.Parse(typeof(FoodGroup), "MilkDairyProducts");
                        recipe.Ingredients.Add(ingredient);

                        break;
                    case 6:
                        ingredient.FoodGroup = (FoodGroup)Enum.Parse(typeof(FoodGroup), "FatsAndOil");
                        recipe.Ingredients.Add(ingredient);

                        break;
                    case 7:
                        ingredient.FoodGroup = (FoodGroup)Enum.Parse(typeof(FoodGroup), "Water");
                        recipe.Ingredients.Add(ingredient);

                        break;
                    case 8:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");

                        break;


                }



            }

            Console.Write("Enter the number of steps: ");
            int stepCount = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < stepCount; i++)
            {
                Console.Write($"Enter step #{i + 1}: ");
                string step = Console.ReadLine();
                recipe.Steps.Add(step);
            }

            recipeApp.AddRecipe(recipe);

            Console.WriteLine("Recipe added successfully!");
        }
        //==================== EndOfMethod =========================================================================================================

        //=========================== Method to scale a recipe =====================================================================================
        static void ScaleRecipe(RecipeApp recipeApp)
        {
            Console.Write("Enter recipe name: ");
            string recipeName = Console.ReadLine();
            double factor = 1;

            recipeApp.ScaleRecipe(recipeName, factor);
        }
        //==================== EndOfMethod =========================================================================================================

        //============================= Method to reset recipe quantities ==========================================================================
        static void ResetRecipeQuantities(RecipeApp recipeApp)
        {
            Console.Write("Enter recipe name: ");
            string recipeName = Console.ReadLine();

            recipeApp.ResetRecipe(recipeName);
        }
        //==================== EndOfMethod =========================================================================================================

        //==================== Method to clear/reset a recipe ======================================================================================
        static void ClearRecipe(RecipeApp recipeApp)
        {
            Console.Write("Enter recipe name: ");
            string recipeName = Console.ReadLine();

            recipeApp.ClearRecipe(recipeName);
        }
        //==================== EndOfMethod =========================================================================================================


    }
    //==================== EndOfClass ==============================================================================================================
}
//=================== EndOfFile=====================================================================================================================