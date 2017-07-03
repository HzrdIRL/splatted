using UnityEngine;
using System.Collections;

[System.Serializable]

public class TurretBlueprint {

    public GameObject prefab;
    public int cost;

    public GameObject upgradedPrefab;
    public int upgradeCost;

    private float health = 100f;
    public float dmg = 0f;

    public int GetSellAmount(bool isUpgraded)
    {
        if (isUpgraded)
            return (cost + upgradeCost) / 2;

        return cost / 2;
    }

}
