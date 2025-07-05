using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

public class Inventory : MonoBehaviour
{
    public Dictionary<int, int> InGameCurrencyItems = new();
    public Dictionary<int, int> InGameSpecialItems = new();
    public Dictionary<int, int> InGameUpgradeItems = new();

    public Dictionary<int, int> CurrencyItems = new();
    public Dictionary<int, int> SpecialItems = new();
    public Dictionary<int, int> UpgradeItems = new();


    [SerializeField] private ScrollRect specialItemScrollRect;
    [SerializeField] private ScrollRect upgradeItemScrollRect;

    [SerializeField] private TextMeshProUGUI inGameInventoryCashAmountText;
    [SerializeField] private TextMeshProUGUI inGameInventoryGoldAmountText;

    [SerializeField] private TextMeshProUGUI inventoryHeaderText;


    [SerializeField] private Button inGameInventoryButton;
    [SerializeField] private Button closeButton;



    [SerializeField] private Transform panelParentTransform;

    private CanvasGroup inGameInventoryButtonCanvasGroup;


    private readonly float InventoryItemSlotWidth = 700;

    private List<GameObject> inventoryItemSlotGOs = new();

    private ItemDatabase itemDatabase;




    private void OnEnable()
    {
        EventManager.OnItemClaimed += ItemClaimed;
        EventManager.OnCurrencyAmountChanged += CurrencyAmountChanged;
        EventManager.OnWheelSpinned += WheelSpinned;
        EventManager.OnCashOutButtonClicked += CashOutButtonClicked;
        EventManager.OnInventoryButtonClicked += InventoryButtonClicked;
        EventManager.OnRevivedContinueButtonClicked += RevivedContinueButtonClicked;
        EventManager.OnGiveUpButtonClicked += GiveUpButtonClicked;


        inGameInventoryButton.onClick.AddListener(()=>SetInventoryItems(InGameCurrencyItems, InGameSpecialItems, InGameUpgradeItems, true));
        closeButton.onClick.AddListener(CloseButtonAction);

        inGameInventoryButtonCanvasGroup = inGameInventoryButton.gameObject.GetComponent<CanvasGroup>();

        itemDatabase = GetComponent<ItemDatabase>();
    }

    private void OnDisable()
    {
        EventManager.OnItemClaimed -= ItemClaimed;
        EventManager.OnCurrencyAmountChanged -= CurrencyAmountChanged;
        EventManager.OnWheelSpinned -= WheelSpinned;
        EventManager.OnCashOutButtonClicked -= CashOutButtonClicked;
        EventManager.OnInventoryButtonClicked -= InventoryButtonClicked;
        EventManager.OnRevivedContinueButtonClicked -= RevivedContinueButtonClicked;
        EventManager.OnGiveUpButtonClicked -= GiveUpButtonClicked;
    }

    private void InventoryButtonClicked()
    {
        SetInventoryItems(CurrencyItems, SpecialItems, UpgradeItems, false);
    }

    private void GiveUpButtonClicked()
    {
        InGameCurrencyItems.Clear();
        InGameSpecialItems.Clear();
        InGameUpgradeItems.Clear();
    }

    private void CashOutButtonClicked()
    {
        StartCoroutine(WaitForMainMenu());
        IEnumerator WaitForMainMenu()
        {
            yield return new WaitForSeconds(1);
            TransferInGameItemsToInventory();
        }
    }

    private void CloseButtonAction()
    {
        panelParentTransform.gameObject.SetActive(false);
        foreach (var item in inventoryItemSlotGOs)
        {
            Destroy(item);
        }

        inventoryItemSlotGOs.Clear();
    }

    private void SetInventoryItems(Dictionary<int,int> currencyDict, Dictionary<int, int> specialItemDict, Dictionary<int, int> upgradeItemDict, bool isInGameInventory)
    {
        panelParentTransform.gameObject.SetActive(true);

        inventoryHeaderText.text = isInGameInventory ? "COLLECTED ITEMS" : "INVENTORY";

        if (currencyDict.TryGetValue(0, out _))
        {
            inGameInventoryCashAmountText.text = "x" + currencyDict[0];
        }
        else
        {
            inGameInventoryCashAmountText.text = string.Empty;
        }

        if (currencyDict.TryGetValue(1, out _))
        {
            inGameInventoryGoldAmountText.text = "x" + currencyDict[1];
        }
        else
        {
            inGameInventoryGoldAmountText.text = string.Empty;
        }


        for (var i = 0; i < specialItemDict.Count; i++)
        {
            var inventoryItemSlotGO = Instantiate(Resources.Load("InventoryItemSlot", typeof(GameObject))) as GameObject;
            inventoryItemSlotGO.transform.parent = specialItemScrollRect.content;
            inventoryItemSlotGO.transform.DOScale(1,0);

            inventoryItemSlotGOs.Add(inventoryItemSlotGO);

            var inventoryItemSlot = inventoryItemSlotGO.GetComponent<InventoryItemSlot>();

            var specialItemSO = (SpecialItem_SO)itemDatabase.GetItemSoById(specialItemDict.ElementAt(i).Key, ItemTypes.SpecialItem);
            inventoryItemSlot.ItemImage.sprite = specialItemSO.itemSprite;
            inventoryItemSlot.ItemNameText.text = specialItemSO.itemName;
            inventoryItemSlot.ItemAmountText.text = "x" + specialItemDict.ElementAt(i).Value;


            if (specialItemSO.hasSecondName)
            {
                inventoryItemSlot.ItemSecondNameText.gameObject.SetActive(true);
                inventoryItemSlot.ItemSecondNameText.text = specialItemSO.secondName;
                inventoryItemSlot.ItemSecondNameText.color = specialItemSO.secondNameColor;
            }
            else
            {
                inventoryItemSlot.ItemSecondNameText.gameObject.SetActive(false);
            }

            if (i == InGameSpecialItems.Count - 1)
            {
                SetScroll(specialItemScrollRect, specialItemDict.Keys.Count);
            }
        }

        for (var i = 0; i < upgradeItemDict.Count; i++)
        {
            var inventoryItemSlotGO = Instantiate(Resources.Load("InventoryItemSlot", typeof(GameObject))) as GameObject;
            inventoryItemSlotGO.transform.parent = upgradeItemScrollRect.content;
            inventoryItemSlotGO.transform.DOScale(1, 0);

            inventoryItemSlotGOs.Add(inventoryItemSlotGO);

            var inventoryItemSlot = inventoryItemSlotGO.GetComponent<InventoryItemSlot>();

            var item_SO = itemDatabase.GetItemSoById(upgradeItemDict.ElementAt(i).Key, ItemTypes.UpgradePoint);
            inventoryItemSlot.ItemImage.sprite = item_SO.itemSprite;
            inventoryItemSlot.ItemNameText.text = item_SO.itemName;
            inventoryItemSlot.ItemAmountText.text = "x" + upgradeItemDict.ElementAt(i).Value;
            inventoryItemSlot.ItemSecondNameText.gameObject.SetActive(false);


            if (i == upgradeItemDict.Count - 1)
            {
                SetScroll(upgradeItemScrollRect, upgradeItemDict.Keys.Count);
            }

        }
    }

    private void ItemClaimed(WheelSlot wheelSlot)
    {
        if (wheelSlot.CurrentItem_SO.itemType == ItemTypes.Chest)
        {
            var chestSO = (Chest_SO)wheelSlot.CurrentItem_SO;

            for (var i = 0; i < chestSO.item_SOs.Length; i++)
            {
                AddItemToTheCorrespondingDictionary(chestSO.item_SOs[i], chestSO.item_SOs[i].defaultItemAmount);
            }
        }
        else
        {
            AddItemToTheCorrespondingDictionary(wheelSlot.CurrentItem_SO, wheelSlot.CurrentItemAmount);
        }

        inGameInventoryButton.interactable = true;
        inGameInventoryButtonCanvasGroup.DOFade(1, 0);
    }

    private void AddItemToTheCorrespondingDictionary(Item_SO item_SO, int amount)
    {
        Dictionary<int, int> dict = new();
        switch (item_SO.itemType)
        {
            case ItemTypes.Cash or ItemTypes.Gold:
                dict = InGameCurrencyItems;
                break;
            case ItemTypes.SpecialItem:
                dict = InGameSpecialItems;
                break;
            case ItemTypes.UpgradePoint:
                dict = InGameUpgradeItems;
                break;
        }

        if (dict.TryGetValue(item_SO.itemId, out _))
        {
            dict[item_SO.itemId]
                += amount;
        }
        else
        {
            dict.Add(item_SO.itemId, amount);
        }

        //LogDictionaries();
    }

    private void CurrencyAmountChanged(int cashAmount, int goldAmount, bool playAnimation)
    {

        for (var currencyId = 0; currencyId <= 1; currencyId++)
        {
            if (CurrencyItems.TryGetValue(currencyId, out _))
            {
                CurrencyItems[currencyId]
                    += currencyId == 0 ? cashAmount : goldAmount;
            }
            else
            {
                CurrencyItems.Add(currencyId, currencyId == 0 ? cashAmount : goldAmount);
            }
        }

        EventManager.CurrencyAmountSet(CurrencyItems[0], CurrencyItems[1], playAnimation);

    }

    public bool IsAmountInsufficient(int amount, int currencyIndex)
    {
        if (amount < 0 && CurrencyItems[currencyIndex] + amount < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void SetScroll(ScrollRect scrollRect, int keyCount)
    {
        if (keyCount < 3)
        {
            scrollRect.horizontal = false;
        }
        else
        {
            scrollRect.horizontal = true;
            var spacingTotal = scrollRect.content.GetComponent<HorizontalLayoutGroup>().spacing * (keyCount - 1);
            var deltaSize = InventoryItemSlotWidth * keyCount + spacingTotal + 80;
            var contentRectTransform = scrollRect.content.GetComponent<RectTransform>();
            contentRectTransform.DOSizeDelta(new Vector2(deltaSize, contentRectTransform.sizeDelta.y), 0);
        }
    }

    private void TransferInGameItemsToInventory()
    {
        CurrencyAmountChanged(InGameCurrencyItems.ElementAt(0).Value, InGameCurrencyItems.ElementAt(1).Value, true);

        for (var i = 0; i < InGameSpecialItems.Count; i++)
        {
            var element = InGameSpecialItems.ElementAt(i);
            
            if (SpecialItems.TryGetValue(element.Key, out _))
            {
                SpecialItems[element.Key]
                    += element.Value;
            }
            else
            {
                SpecialItems.Add(element.Key, element.Value);
            }
        }

        for (var i = 0; i < InGameUpgradeItems.Count; i++)
        {
            var element = InGameUpgradeItems.ElementAt(i);

            if (UpgradeItems.TryGetValue(element.Key, out _))
            {
                UpgradeItems[element.Key]
                    += element.Value;
            }
            else
            {
                UpgradeItems.Add(element.Key, element.Value);
            }
        }

    }

    private void WheelSpinned()
    {
        inGameInventoryButton.interactable = false;
        inGameInventoryButtonCanvasGroup.DOFade(0, 1);
    }

    private void RevivedContinueButtonClicked()
    {
        inGameInventoryButton.interactable = true;
        inGameInventoryButtonCanvasGroup.DOFade(1, 0);
    }
}
