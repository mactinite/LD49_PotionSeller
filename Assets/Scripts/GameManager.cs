using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ElRaccoone.Tweens;
public class GameManager : StateMachine<GameManager>
{

    public float CurrentTime { get => currentTime; set => currentTime = value; }
    public PersonaGenerator PersonaGenerator { get => personaGenerator; set => personaGenerator = value; }
    public int CurrentDay { get => currentDay; set => currentDay = value; }
    public ShopManager ShopManager { get => shopManager; set => shopManager = value; }

    public DefaultState defaultState;

    public GameObject dialogueBox;
    public TMPro.TMP_Text dialogueText;


    public Tool[] tools;
    public Tool currentTool;

    public Transform customerSpawnLocation;

    public float shiftLength = 3600;
    private float currentTime = 0;
    public bool isOpen = false;

    public List<Recipe> Orders = new List<Recipe>();

    private ShopManager shopManager;
    private PersonaGenerator personaGenerator;
    public OrderGenerator orderGenerator = new OrderGenerator();


    public GameObject OderUIPrefab;

    public GameObject OrdersLayoutGroup;

    private int currentDay = 0;
    public TMPro.TMP_Text dayCounter;



    private void Start()
    {
        // create instances of the states
        defaultState = new DefaultState(this);
        ShopManager = GetComponent<ShopManager>();
        personaGenerator = GetComponent<PersonaGenerator>();
        dialogueBox.SetActive(false);
        foreach (var tool in tools)
        {
            tool.OnDroppedOnWorkspace.AddListener(ToolDroppedOnWorkspace);
        }

        //this.SetState(defaultState);
        this.SetState(new OpeningState(this));
    }

    private void ToolDroppedOnWorkspace(Tool tool)
    {
        currentTool = tool;
    }


    private void Update()
    {
        if (isOpen)
        {
            currentTime += Time.deltaTime;
        }
    }


    public void PickOrder(int orderIndex)
    {
        if (!ShopManager.isPreppingPotion)
        {
            Recipe order = Orders[orderIndex];
            ShopManager.PreparePotion(order);
            Orders.RemoveAt(orderIndex);
            RedrawOrdersList();
        }
    }

    public void RedrawOrdersList()
    {
        // prune extra transforms
        int extraChildren = OrdersLayoutGroup.transform.childCount - Orders.Count;
        if (extraChildren > 0)
        {
            for (int i = 0; i < extraChildren; i++)
            {
                Destroy(OrdersLayoutGroup.transform.GetChild((OrdersLayoutGroup.transform.childCount - 1) - i).gameObject);
            }
        }


        for (int i = 0; i < Orders.Count; i++)
        {
            if (i < OrdersLayoutGroup.transform.childCount)
            {
                GameObject existingOrderPrefab = OrdersLayoutGroup.transform.GetChild(i).gameObject;
                existingOrderPrefab.GetComponent<Button>().onClick.RemoveAllListeners();
                existingOrderPrefab.GetComponent<Button>().onClick.AddListener(WrappedButtonCallback(i));
                existingOrderPrefab.GetComponentInChildren<TMPro.TMP_Text>().text = $"{Orders[i].name}";
            }
            else
            {
                GameObject newOrderPrefab = Instantiate(OderUIPrefab, OrdersLayoutGroup.transform);
                newOrderPrefab.GetComponent<Button>().onClick.RemoveAllListeners();
                newOrderPrefab.GetComponent<Button>().onClick.AddListener(WrappedButtonCallback(i));
                newOrderPrefab.GetComponentInChildren<TMPro.TMP_Text>().text = $"{Orders[i].name}";
            }
        }


    }

    public UnityAction WrappedButtonCallback(int i)
    {
        return () =>
        {
            PickOrder(i);
        };
    }

}



public class DefaultState : State<GameManager>
{
    public DefaultState(GameManager stateMachine) : base(stateMachine)
    {
    }
}


public class GameOverState : State<GameManager>
{
    public GameOverState(GameManager stateMachine) : base(stateMachine)
    {

    }

    public override IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);
        FlybyText.SpawnText("Game Over");

        // show game over screen, 
        // offer to restart
        // show score & stats
        yield return null;
    }
}


public class OpeningState : State<GameManager>
{
    public OpeningState(GameManager stateMachine) : base(stateMachine)
    {

    }

    public override IEnumerator End()
    {
        return base.End();
    }

    public override IEnumerator Start()
    {
        FlybyText.SpawnText("Good Morning!");
        yield return new WaitForSeconds(3f);
        FlybyText.SpawnText("Start Shift!");
        yield return new WaitForSeconds(3f);
        StateMachine.ShopManager.Gold -= StateMachine.ShopManager.upkeepCost;

        if(StateMachine.ShopManager.Gold < 0)
        {
            // BANKRUPT GAME OVER
            FlybyText.SpawnText("BANKRUPT!");
            StateMachine.SetState(new GameOverState(StateMachine));

        }

        StateMachine.CurrentDay++;
        StateMachine.dayCounter.text = $"Day {StateMachine.CurrentDay}";
        StateMachine.CurrentTime = 0;
        StateMachine.SetState(new OpenState(StateMachine));
    }
}

public class OpenState : State<GameManager>
{
    public OpenState(GameManager stateMachine) : base(stateMachine)
    {
    }

    public override IEnumerator End()
    {
        return base.End();
    }

    public override IEnumerator Start()
    {
        StateMachine.isOpen = true;
        while (StateMachine.CurrentTime < StateMachine.shiftLength)
        {

            yield return NewCustomer();
            yield return new WaitForSeconds(UnityEngine.Random.Range(5, 10));
        }
        // wait for potion in progress to be finished to be nice..
        while (StateMachine.ShopManager.isPreppingPotion)
        {
            yield return null;
        }

        StateMachine.SetState(new ClosingState(StateMachine));
    }

    public IEnumerator NewCustomer()
    {
        // handle generating people and dialogue etc.
        GameObject customer = StateMachine.PersonaGenerator.GenerateHuman();
        customer.transform.position = StateMachine.customerSpawnLocation.position;

        yield return customer.TweenLocalScale(Vector2.one * 2f, 1f).Yield();

        yield return null;
        // get random recipe and add it to the queue
        var recipe = StateMachine.orderGenerator.GetOrder(StateMachine.tools);
        StateMachine.dialogueBox.SetActive(true);
        StateMachine.dialogueText.text = $"Hello, I'd like a {recipe.name}. Please hurry I'm desperate.";
        yield return new WaitForSeconds(2f);
        StateMachine.dialogueBox.SetActive(false);
        StateMachine.dialogueText.text = "";
        StateMachine.Orders.Add(recipe);
        StateMachine.RedrawOrdersList();

        yield return customer.TweenPositionX(5, 2f).Yield();

    }
}

public class ClosingState : State<GameManager>
{
    public ClosingState(GameManager stateMachine) : base(stateMachine)
    {
    }

    public override IEnumerator End()
    {

        yield return new WaitForSeconds(3f);
    }

    public override IEnumerator Start()
    {
        StateMachine.Orders.Clear();
        StateMachine.RedrawOrdersList();


        FlybyText.SpawnText("End Shift!");
        yield return new WaitForSeconds(3f);

        FlybyText.SpawnText("Zzzzz");
        yield return new WaitForSeconds(3f);

        StateMachine.SetState(new OpeningState(StateMachine));
        // TODO: UPGRADES
        // while(upgrading){}
    }
}

