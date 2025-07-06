using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

public class Inventory : MonoBehaviour
{
    private Dictionary<int, int> inGameCurrencyItems = new();
    private Dictionary<int, int> inGameSpecialItems = new();
    private Dictionary<int, int> inGameUpgradeItems = new();

    private Dictionary<int, int> currencyItems = new();
    private Dictionary<int, int> specialItems = new();
    private Dictionary<int, int> upgradeItems = new();

    public Dictionary<int, int> CurrencyItems => currencyItems;
    public Dictionary<int, int> SpecialItems => specialItems;
    public Dictionary<int, int> UpgradeItems => upgradeItems;


    [SerializeField] private ScrollRect specialItemScrollRect;
    [SerializeField] private ScrollRect upgradeItemScrollRect;

    [SerializeField] private TextMeshProUGUI inGameInventoryCashAmountText;
    [SerializeField] private TextMeshProUGUI inGameInventoryGoldAmountText;

    [SerializeField] private TextMeshProUGUI inventoryHeaderText;

    [SerializeField] private Button inGameInventoryButton;
    [SerializeField] private Button closeButton;

    [SerializeField] private Transform panelParentTransform;

    private CanvasGroup inGameInventoryButtonCanvasGroup;

    private readonly List<GameObject> inventoryItemSlotGOs = new();

    private ItemDatabase itemDatabase;

    private readonly int firstSessionCashAmount = 10000;
    private readonly int firstSessionGoldAmount = 50;

    private const string INVENTORY_STRING = "INVENTORY";
    private const string COLLECTED_ITEMS_STRING = "COLLECTED ITEMS";
    private const string INVENTORY_ITEM_SLOT_PATH = "Prefabs/InventoryItemSlot";


    private void Awake()
    {
        if (!PlayerPrefs.HasKey("FirstSession") || PlayerPrefs.GetInt("FirstSession") == 0)
        {
            CurrencyAmountChanged(firstSessionCashAmount, 0, false);
            CurrencyAmountChanged(firstSessionGoldAmount, 1, false);
            PlayerPrefs.SetInt("FirstSession", 1);
        }
        else
        {
            LoadInventoryData();
        }
    }


    private void OnEnable()
    {
        EventManager.OnItemClaimed += ItemClaimed;
        EventManager.OnCurrencyAmountChanged += CurrencyAmountChanged;
        EventManager.OnWheelSpinned += WheelSpinned;
        EventManager.OnCashOutButtonClicked += CashOutButtonClicked;
        EventManager.OnInventoryButtonClicked += InventoryButtonClicked;
        EventManager.OnRevivedContinueButtonClicked += RevivedContinueButtonClicked;
        EventManager.OnGiveUpButtonClicked += GiveUpButtonClicked;


        inGameInventoryButton.onClick.AddListener(()=>SetInventoryItems(inGameCurrencyItems, inGameSpecialItems, inGameUpgradeItems, true));
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


    private void OnApplicationQuit()
    {
        SaveSystem.SaveInventoryData(this);
    }


    private void LoadInventoryData()
    {
        var inventoryData = SaveSystem.LoadInventoryData();

        currencyItems = inventoryData.currencyData;
        specialItems = inventoryData.specialItemData;
        upgradeItems = inventoryData.upgradeItemData;

        EventManager.CurrencyAmountSet(currencyItems[0], 0, 0, false);
        EventManager.CurrencyAmountSet(currencyItems[1], 0, 1, false);
    }


    private void InventoryButtonClicked()
    {
        SetInventoryItems(currencyItems, specialItems, upgradeItems, false);
    }


    private void GiveUpButtonClicked()
    {
        inGameCurrencyItems.Clear();
        inGameSpecialItems.Clear();
        inGameUpgradeItems.Clear();
    }


    private void CashOutButtonClicked()
    {
        StartCoroutine(WaitForMainMenu());
        IEnumerator WaitForMainMenu()
        {
            yield return new WaitForSeconds(2);
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

        inventoryHeaderText.text = isInGameInventory ? COLLECTED_ITEMS_STRING : INVENTORY_STRING;

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
            var inventoryItemSlotGO = Instantiate(Resources.Load(INVENTORY_ITEM_SLOT_PATH, typeof(GameObject))) as GameObject;
            inventoryItemSlotGO.transform.SetParent(specialItemScrollRect.content);
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

            if (i == specialItemDict.Count - 1)
            {
                var childWidth = inventoryItemSlotGO.GetComponent<RectTransform>().sizeDelta.x;
                ScrollSetter.SetScroll(specialItemScrollRect, specialItemDict.Keys.Count, childWidth, 2);
            }
        }

        for (var i = 0; i < upgradeItemDict.Count; i++)
        {
            var inventoryItemSlotGO = Instantiate(Resources.Load(INVENTORY_ITEM_SLOT_PATH, typeof(GameObject))) as GameObject;
            inventoryItemSlotGO.transform.SetParent(upgradeItemScrollRect.content);
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
                var childWidth = inventoryItemSlotGO.GetComponent<RectTransform>().sizeDelta.x;
                ScrollSetter.SetScroll(upgradeItemScrollRect, upgradeItemDict.Keys.Count, childWidth, 2);
            }

        }
    }


    private void ItemClaimed(int amount, Item_SO item_SO = null, Item_SO[] item_SOs = null)
    {
        if (item_SO)
        {
            AddItemToTheCorrespondingDictionary(item_SO, amount);
        }
        else
        {
            foreach (var item in item_SOs)
            {
                AddItemToTheCorrespondingDictionary(item, item.defaultItemAmount);
            }
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
                dict = inGameCurrencyItems;
                break;
            case ItemTypes.SpecialItem:
                dict = inGameSpecialItems;
                break;
            case ItemTypes.UpgradePoint:
                dict = inGameUpgradeItems;
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

    }


    private void CurrencyAmountChanged(int amount, int currencyId, bool playAnimation)
    {
        var currentAmount = 0;
        if (currencyItems.TryGetValue(currencyId, out _))
        {
            currentAmount = currencyItems[currencyId];
            currencyItems[currencyId] += amount;
        }
        else
        {
            currencyItems.Add(currencyId, amount);
        }

        EventManager.CurrencyAmountSet(currencyItems[currencyId], currentAmount, currencyId, playAnimation);

        SaveSystem.SaveInventoryData(this);
    }


    public bool IsAmountInsufficient(int amount, int currencyIndex)
    {
        if (amount < 0 && currencyItems[currencyIndex] + amount < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void TransferInGameItemsToInventory()
    {

        if (inGameCurrencyItems.TryGetValue(0, out _))
        {

            CurrencyAmountChanged(inGameCurrencyItems[0], 0, true);
        }

        if (inGameCurrencyItems.TryGetValue(1, out _))
        {

            CurrencyAmountChanged(inGameCurrencyItems[1], 1, true);
        }


        for (var i = 0; i < inGameSpecialItems.Count; i++)
        {
            var element = inGameSpecialItems.ElementAt(i);
            
            if (specialItems.TryGetValue(element.Key, out _))
            {
                specialItems[element.Key]
                    += element.Value;
            }
            else
            {
                specialItems.Add(element.Key, element.Value);
            }
        }

        for (var i = 0; i < inGameUpgradeItems.Count; i++)
        {
            var element = inGameUpgradeItems.ElementAt(i);

            if (upgradeItems.TryGetValue(element.Key, out _))
            {
                upgradeItems[element.Key]
                    += element.Value;
            }
            else
            {
                upgradeItems.Add(element.Key, element.Value);
            }
        }

        inGameCurrencyItems.Clear();
        inGameSpecialItems.Clear();
        inGameUpgradeItems.Clear();

        SaveSystem.SaveInventoryData(this);
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