using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    public Dictionary<int, int> currencyData = new();
    public Dictionary<int, int> specialItemData = new();
    public Dictionary<int, int> upgradeItemData = new();

    public InventoryData (Inventory inventory)
    {
        currencyData = inventory.CurrencyItems;
        specialItemData = inventory.SpecialItems;
        upgradeItemData = inventory.UpgradeItems;
    }
}
