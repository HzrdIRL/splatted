using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public TurretBlueprint standardTurret;
    public TurretBlueprint missileTurret;
    public TurretBlueprint lazerTurret;
    public TurretBlueprint paintTurret;

    [Header("UI Text")]
    public Text standardTurretCost;
    public Text missileTurretCost;
    public Text lazerTurretCost;
    public Text paintTurretCost;

    BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;
        standardTurretCost.text = "$"+ standardTurret.cost;
        missileTurretCost.text = "$" + missileTurret.cost;
        lazerTurretCost.text = "$" + lazerTurret.cost;
        paintTurretCost.text = "$" + paintTurret.cost;
    }

    public void SelectStandardTurret()
    {
        Debug.Log("Standard Turret Selected");
        buildManager.SelectTurretToBuild(standardTurret);
    }

    public void SelectMissileTurret()
    {
        Debug.Log("Missile Turret Selected");
        buildManager.SelectTurretToBuild(missileTurret);
    }

    public void SelectLazerTurret()
    {
        Debug.Log("Lazer Turret Selected");
        buildManager.SelectTurretToBuild(lazerTurret);
    }

    public void SelectPaintTurret()
    {
        Debug.Log("Paint Turret Selected");
        buildManager.SelectTurretToBuild(paintTurret);
    }
}
