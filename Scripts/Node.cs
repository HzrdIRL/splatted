using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {

    public Color buildHoverColor;
    public Color NoMoneyColor;
    public Color p1Color;
    public Color p2Color;

    // Determines which player owns the node, 0 = neutral, 1 = p1, 2 = p2
    public int ownedBy;

    public Vector3 buildOffset;

    [HideInInspector]
    public GameObject turret;
    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]
    public bool isUpgraded = false;

    private Color color;
    private Renderer rend;

    BuildManager buildManager;

    void Start()
    {
        rend = GetComponent<Renderer>();
        color = rend.material.color;

        if (ownedBy == 1)
        {
            rend.material.color = p1Color;
            color = p1Color;
        }
        else if (ownedBy == 2)
        {
            rend.material.color = p2Color;
            color = p2Color;
        }

        buildManager = BuildManager.instance;
    }

    public void ChangeOwner(int ownerID)
    {
        ownedBy = ownerID;

        if(ownedBy == 1)
        {
            rend.material.color = p1Color;
            color = p1Color;
        }
        else if(ownedBy == 2)
        {
            rend.material.color = p2Color;
            color = p2Color;
        }
    }

    void BuildTurret(TurretBlueprint blueprint, int _playerID)
    {
        if (PlayerStats.Money < blueprint.cost)
        {
            Debug.Log("We Require More Vespene Gas");
            return;
        }

        PlayerStats.Money -= blueprint.cost;

        GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;
        turret.GetComponent<Turret>().ownedBy = _playerID;

        turretBlueprint = blueprint;

        GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Debug.Log("Turret Built!");
    }

    public void UpgradeTurret()
    {
        int _playerID = turret.GetComponent<Turret>().ownedBy;
        if (PlayerStats.Money < turretBlueprint.upgradeCost)
        {
            Debug.Log("We Require More Vespene Gas");
            return;
        }

        PlayerStats.Money -= turretBlueprint.upgradeCost;

        //Remove old turret before upgrading
        Destroy(turret);

        //Build upgraded turret
        GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;
        turret.GetComponent<Turret>().ownedBy = _playerID;

        GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        isUpgraded = true;

        Debug.Log("Turret Upgraded!");
    }

    public void SellTurret()
    {
        PlayerStats.Money += turretBlueprint.GetSellAmount(isUpgraded);

        GameObject effect = (GameObject)Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(turret);
        turretBlueprint = null;
    }

    void OnMouseDown()
    {
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        if (turret != null)
        {
            buildManager.SelectNode(this);
            return;
        }

        if (!buildManager.CanBuild)
            return;

        if(ownedBy == PlayerStats.playerID || (buildManager.GetTurretToBuild().prefab.GetComponent<Turret>().usePaint == true && ownedBy != 2))
        BuildTurret(buildManager.GetTurretToBuild(), PlayerStats.playerID);

    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + buildOffset;
    }

    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (buildManager.HasMoney && 
            (PlayerStats.playerID == ownedBy || 
            (buildManager.GetTurretToBuild().prefab.GetComponent<Turret>().usePaint == true) && ownedBy != 2)) // need to change the '2' to playerstats.playerId for the enemy player/AI
        {
            rend.material.color = buildHoverColor;
            return;
        }

        rend.material.color = NoMoneyColor; 
    }

    void OnMouseExit()
    {
        rend.material.color = color;
    }
}
