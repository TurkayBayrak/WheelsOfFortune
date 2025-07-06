using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemSecondNameText;
    [SerializeField] private TextMeshProUGUI itemAmountText;
    [SerializeField] private Image itemImage;


    public TextMeshProUGUI ItemNameText => itemNameText;
    public TextMeshProUGUI ItemSecondNameText => itemSecondNameText;
    public TextMeshProUGUI ItemAmountText => itemAmountText;
    public Image ItemImage => itemImage;
}
