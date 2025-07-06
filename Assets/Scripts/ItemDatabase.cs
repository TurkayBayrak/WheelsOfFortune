using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [SerializeField] private Item_SO[] allSpecialItem_SOs;
    [SerializeField] private Item_SO[] allUpgradeItem_SOs;


    public Item_SO GetItemSoById(int itemId, ItemTypes itemTypes)
    {
        var soArray = itemTypes == ItemTypes.SpecialItem ? allSpecialItem_SOs : allUpgradeItem_SOs;
        foreach (var item_SO in soArray)
        {
            if (item_SO.itemId == itemId)
            {
                return item_SO;
            }
        }

        return null;
    }
}
