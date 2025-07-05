using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;

    public TextMeshProUGUI ItemNameText => itemNameText;

    [SerializeField] private TextMeshProUGUI itemSecondNameText;

    public TextMeshProUGUI ItemSecondNameText => itemSecondNameText;


    [SerializeField] private Image itemImage;

    public Image ItemImage => itemImage;


    [SerializeField] private TextMeshProUGUI itemAmountText;

    public TextMeshProUGUI ItemAmountText => itemAmountText;
}
