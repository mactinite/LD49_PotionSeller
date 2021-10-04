using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{

    public TMPro.TMP_Text upgradeCostText;
    public Button upgradeButton;
    public int upgradeCost = 250;

    [SceneObjectsOnly]
    public Tool upgradeToolReference;


    // Start is called before the first frame update
    void Start()
    {
        upgradeCostText.text = $"{upgradeCost}";
        upgradeButton.onClick.AddListener(Clicked);
        if (UpgradeManager.HasUnlocked(upgradeToolReference))
        {
            this.gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clicked()
    {
        if (ShopManager.Gold >= upgradeCost)
        {
            UpgradeManager.UnlockTool(upgradeToolReference);
            ShopManager.Gold -= upgradeCost;
            this.gameObject.SetActive(false);
        } else
        {
            //Not enough mulah
        }
    }

}
