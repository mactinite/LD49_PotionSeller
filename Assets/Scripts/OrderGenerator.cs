using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class OrderGenerator
{
    public Recipe[] allRecipes;

    private Recipe[] craftableRecipes;
    public Recipe GetOrder(List<Tool> unlockedTools)
    {
        craftableRecipes = allRecipes.Where((Recipe recipe) =>
        {
            string[] requiredTools;
            string[] unlockedToolIDs;

            requiredTools = recipe.steps.ConvertAll<string>((Step step) =>
            {
                return step.requiredTool.ToolID;
            }).ToArray();

            unlockedToolIDs = unlockedTools.ToList().ConvertAll((Tool tool) =>
            {
                return tool.ToolID;
            }).ToArray();

            // false if it requires more tools than we have
            // NOTE: May want to increase this to have some recipes not able to be made to tease the player into buying?? idk that might feel bad if done wrong
            // a better option might be to have a fixed amount of uncraftable recipes when you are in $ range of an upgrade.
            if (requiredTools.Except<string>(unlockedToolIDs).Count() > 0)
            {
                return false;
            } else
            {
                return true;
            }

        }).ToArray();


        Recipe randomRecipe = craftableRecipes[Random.Range(0, craftableRecipes.Length)];

        return randomRecipe;
    }

}
