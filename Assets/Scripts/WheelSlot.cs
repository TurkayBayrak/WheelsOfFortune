using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WheelSlot : MonoBehaviour
{
    [SerializeField] private Image slotImage;

    [SerializeField] private TextMeshProUGUI amountText;

    private Item_SO currentItem_SO;
    public Item_SO CurrentItem_SO => currentItem_SO;

    private int currentItemAmount;
    public int CurrentItemAmount => currentItemAmount;


    public void Init(Item_SO item_SO, int zoneCount)
    {
        currentItem_SO = item_SO;

        slotImage.sprite = item_SO.itemSprite;

        var zoneMultiplier = zoneCount % 5 == 0 ? 2 : 1;

        if (item_SO.itemType is ItemTypes.Chest or ItemTypes.SpecialItem)
            currentItemAmount = 1;
        else if (item_SO.itemType is ItemTypes.None)
            currentItemAmount = 0;
        else
            currentItemAmount = item_SO.defaultItemAmount * zoneCount * zoneMultiplier;


        if (currentItemAmount == 0)
            amountText.text = string.Empty;
        else
            amountText.text = "x" + currentItemAmount;
    }
}
