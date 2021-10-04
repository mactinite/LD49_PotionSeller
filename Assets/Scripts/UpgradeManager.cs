using mactinite.ToolboxCommons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : SingletonMonobehavior<UpgradeManager>
{
    public List<Tool> availableToolUpgrades;
    private GameManager gameManager;
    private void Start()
    {
        gameManager = GetComponent<GameManager>();
    }
    public static bool HasUnlocked(Tool tool)
    {
        return !Instance.availableToolUpgrades.Contains(tool);
    }

    public static void UnlockTool(Tool tool)
    {
        Instance.availableToolUpgrades.Remove(tool);
        tool.gameObject.SetActive(true);
        Instance.gameManager.tools.Add(tool);
        tool.OnDroppedOnWorkspace.AddListener(Instance.gameManager.ToolDroppedOnWorkspace);
    }

}
