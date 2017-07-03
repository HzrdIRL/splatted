using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour {

    private Node target;

    public Text upgradeCost;
    public Button upgradeButton;

    public GameObject ui;

    public void SetTarget(Node _target)
    {
        target = _target;

        transform.position = target.GetBuildPosition();

        if(!target.isUpgraded)
        {
            upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
            upgradeButton.interactable = true;
        }else
        {
            upgradeButton.interactable = false;
            upgradeCost.text = "MAX";
        }
            

        ui.SetActive(true);
    }

    public void Hide()
    {
        ui.SetActive(false);
    }

    public void Upgrade ()
    {
        target.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }

    public void Sell()
    {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
    }

}
