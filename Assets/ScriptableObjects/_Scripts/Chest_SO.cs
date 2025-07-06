using UnityEngine;

[CreateAssetMenu(fileName = "Chest_SO")]
public class Chest_SO : Item_SO
{

    public int itemAmountToGive;
    public Item_SO[] currencyItemPool;
    public Item_SO[] specialItemPool;
    public Item_SO[] upgradeItemPool;



    [HideInInspector] public int currencyValue = 33;
    [HideInInspector] public int specialValue = 33;
    [HideInInspector] public int upgradeValue = 34;

    public const int TOTAL = 100;
}
