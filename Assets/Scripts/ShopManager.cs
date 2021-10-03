using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : StateMachine<ShopManager>
{

    public TMPro.TMP_Text stepText;
    public GameManager gameManager;
    public SelectOrderState selectOrderState;

    public bool isPreppingPotion = false;
    public CurrentOrderUI orderUI;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        selectOrderState = new SelectOrderState(this);

    }

    public void PreparePotion(Recipe recipe)
    {
        SetState(new PreparingPotionState(recipe, this));
    }



}


public class SelectOrderState : State<ShopManager>
{
    public SelectOrderState(ShopManager stateMachine) : base(stateMachine)
    {
    }

    public override IEnumerator End()
    {
        return base.End();
    }

    public override IEnumerator Start()
    {
        StateMachine.orderUI.gameObject.SetActive(false);
        return base.Start();
    }
}

public class PreparingPotionState : State<ShopManager>
{
    Recipe currentRecipe;
    int currentStep = 0;
    bool failed = false;

    GameManager gameManager;
    public PreparingPotionState(Recipe recipe, ShopManager stateMachine) : base(stateMachine)
    {
        currentRecipe = recipe;
        gameManager = stateMachine.gameManager;
    }

    public override IEnumerator End()
    {

        
        return base.End();
    }

    public override IEnumerator Start()
    {
        StateMachine.isPreppingPotion = true;
        StateMachine.orderUI.gameObject.SetActive(true);
        StateMachine.orderUI.potionTitle.text = currentRecipe.name;
        StateMachine.orderUI.stepsText.text = GetStepsText();


        // look at recipe, starting at first step spawn mini games waiting for a failure or success result from each
        while (currentStep < currentRecipe.steps.Count && !failed)
        {
            yield return WaitForRightTool(currentRecipe.steps[currentStep]);
            yield return WaitForStep(currentRecipe.steps[currentStep]);

            if (!failed)
            {
                currentStep++;
                StateMachine.orderUI.stepsText.text = GetStepsText();
            }

            yield return null;
        }


        if (failed)
        {
            // Failed the recipe, tell the player some how
            StateMachine.stepText.text = $"Failed...";
        }
        else
        {
            // Success! tell the player some how
            StateMachine.stepText.text = $"Success!";
            StateMachine.orderUI.stepsText.text = GetStepsText();
            yield return new WaitForSeconds(1f);
        }

        StateMachine.isPreppingPotion = false;
        StateMachine.SetState(StateMachine.selectOrderState);
    }

    private string GetStepsText()
    {
        string stepsText = "";
        for (int i = 0; i < currentRecipe.steps.Count; i++)
        {
            if(i < currentStep)
            {
                stepsText += $"<s>{currentRecipe.steps[i].verb} {currentRecipe.steps[i].ingredient}</s> \n";
            } else if(i > currentStep)
            {
                stepsText += $"{currentRecipe.steps[i].verb} {currentRecipe.steps[i].ingredient} \n";
            } else
            {
                stepsText += $"<b>{currentRecipe.steps[i].verb} {currentRecipe.steps[i].ingredient}</b> \n";

            }
        }

        return stepsText;
    }

    private IEnumerator WaitForRightTool(Step step)
    {
        StateMachine.stepText.text = $"{step.verb} some {step.ingredient}";

        while (gameManager.currentTool == null || gameManager.currentTool.ToolID != step.requiredTool.ToolID)
        {
            if (gameManager.currentTool != null && gameManager.currentTool.ToolID != step.requiredTool.ToolID)
            {
                // TODO: Player Feedback letting them know they used the wrong tool
                gameManager.currentTool.Return();
                gameManager.currentTool = null;
            }
            yield return null;
        }

        gameManager.currentTool.gameObject.SetActive(false);
    }

    private IEnumerator WaitForStep(Step step)
    {
        bool finished = false;

        MiniGame inProgress = MiniGame.Instantiate(gameManager.currentTool.interaction, Vector2.zero, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        inProgress.StartGame();
        // upon success we move to the next ingredient.
        inProgress.OnSuccess.AddListener(() =>
        {
            finished = true;
        });

        // if a failure occurs the recipe fails and ingredients are lost. 
        inProgress.OnFail.AddListener(() =>
        {
            finished = true;
            failed = true;
        });

        while (finished == false)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        GameObject.Destroy(inProgress.gameObject);
        gameManager.currentTool.gameObject.SetActive(true);
        gameManager.currentTool.Return();
        gameManager.currentTool = null;
        yield return new WaitForSeconds(1f);
        gameManager.currentTool = null;
    }
}
