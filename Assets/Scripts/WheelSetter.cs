using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

public class WheelSetter : MonoBehaviour
{
    [SerializeField] private Image wheelImage;
    [SerializeField] private Image indicatorImage;
    [SerializeField] private Transform slotParentTransform;
    [SerializeField] private TextMeshProUGUI zoneCountText;
    [SerializeField] private Button cashOutButton;
    [SerializeField] private TextMeshProUGUI safeSuperZoneText;

    private int zoneCount = 1;

    [SerializeField] private Wheel_SO[] wheel_SOs;

    private WheelSlot[] wheelSlots;
    private Wheel_SO currentWheel_SO;
    public Wheel_SO CurrentWheel_SO => currentWheel_SO;
    private CanvasGroup cashOutButtonCanvasGroup;

    private const string ZONE_STRING = "ZONE: ";
    private const string GOLDEN_SPIN_STRING = "GOLDEN SPIN";
    private const string SILVER_SPIN_STRING = "SILVER SPIN";

    private void OnEnable()
    {
        EventManager.OnPlayButtonClicked += PlayButtonClicked;
        EventManager.OnItemClaimed += SetNextZone;
        EventManager.OnWheelSpinned += WheelSpinned;
        EventManager.OnGiveUpButtonClicked += GiveUpButtonClicked;

        cashOutButton.onClick.AddListener(CashOutButtonClickAction);

        wheelSlots = slotParentTransform.GetComponentsInChildren<WheelSlot>();

        cashOutButtonCanvasGroup = cashOutButton.gameObject.GetComponent<CanvasGroup>();


        zoneCountText.text = ZONE_STRING + zoneCount;
    }

    private void OnDisable()
    {
        EventManager.OnPlayButtonClicked -= PlayButtonClicked;
        EventManager.OnItemClaimed -= SetNextZone;
        EventManager.OnWheelSpinned -= WheelSpinned;
        EventManager.OnGiveUpButtonClicked -= GiveUpButtonClicked;

        cashOutButton.onClick.RemoveAllListeners();
    }

    private void PlayButtonClicked()
    {
        SetCurrentWheel();
        SetWheelSprites();
        SetSlots();
    }

    private void SetNextZone(int amount, Item_SO item_SO = null, Item_SO[] item_SOs = null)
    {
        zoneCount++;
        zoneCountText.text = ZONE_STRING + zoneCount;
        PlayButtonClicked();
    }

    private void SetCurrentWheel()
    {
        if (zoneCount % 30 == 0)
        {
            currentWheel_SO = wheel_SOs[2];
            safeSuperZoneText.gameObject.SetActive(true);
            safeSuperZoneText.text = GOLDEN_SPIN_STRING;
            SetCashOutButton();
        }
        else if (zoneCount % 5 == 0)
        {
            currentWheel_SO = wheel_SOs[1];
            safeSuperZoneText.gameObject.SetActive(true);
            safeSuperZoneText.text = SILVER_SPIN_STRING;
            SetCashOutButton();
        }
        else
        {
            currentWheel_SO = wheel_SOs[0];
            safeSuperZoneText.gameObject.SetActive(false);
            cashOutButton.gameObject.SetActive(false);
        }
    }

    private void SetCashOutButton()
    {
        cashOutButton.gameObject.SetActive(true);
        cashOutButtonCanvasGroup.DOFade(1, 0);
        cashOutButton.interactable = true;

    }

    public void SetWheelSprites()
    {
        wheelImage.sprite = currentWheel_SO.wheelSprite;
        indicatorImage.sprite = currentWheel_SO.indicatorSprite;
    }

    public void SetSlots()
    {
        for (var i = 0; i < wheelSlots.Length; i++)
        {
            wheelSlots[i].Init(currentWheel_SO.item_SOs[i], zoneCount);
        }
    }

    private void WheelSpinned()
    {
        if (!cashOutButton.gameObject.activeSelf) return;
        cashOutButton.interactable = false;
        cashOutButtonCanvasGroup.DOFade(0, 1);
    }

    private void CashOutButtonClickAction()
    {
        SetZoneCountAndText();
        EventManager.BeginFade(1, 2, true);
        EventManager.CashOutButtonClicked();
    }

    private void GiveUpButtonClicked()
    {
        SetZoneCountAndText();
    }

    private void SetZoneCountAndText()
    {
        StartCoroutine(WaitCo());
        IEnumerator WaitCo()
        {
            yield return new WaitForSeconds(1.9f);
            zoneCount = 1;
            zoneCountText.text = ZONE_STRING + zoneCount;
        }
    }
}
