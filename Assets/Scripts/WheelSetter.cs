using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class WheelSetter : MonoBehaviour
{
    [SerializeField] private Image wheelImage;
    [SerializeField] private Image indicatorImage;
    [SerializeField] private Transform slotParentTransform;
    [SerializeField] private TextMeshProUGUI zoneCountText;
    [SerializeField] private Button cashOutButton;
    [SerializeField] private TextMeshProUGUI safeSuperZoneText;
    public int zoneCount = 1;

    [SerializeField] private Wheel_SO[] wheel_SOs;

    private WheelSlot[] wheelSlots;
    private Wheel_SO currentWheel_SO;
    public Wheel_SO CurrentWheel_SO => currentWheel_SO;
    private CanvasGroup cashOutButtonCanvasGroup;



    private void OnEnable()
    {
        EventManager.OnPlayButtonClicked += PlayButtonClicked;
        EventManager.OnItemClaimed += SetNextZone;
        EventManager.OnWheelSpinned += WheelSpinned;

        cashOutButton.onClick.AddListener(CashOutButtonClickAction);

        wheelSlots = slotParentTransform.GetComponentsInChildren<WheelSlot>();

        cashOutButtonCanvasGroup = cashOutButton.gameObject.GetComponent<CanvasGroup>();


        zoneCountText.text = "ZONE: " + zoneCount;
    }

    private void OnDisable()
    {
        EventManager.OnPlayButtonClicked -= PlayButtonClicked;
        EventManager.OnItemClaimed -= SetNextZone;
        EventManager.OnWheelSpinned -= WheelSpinned;

    }

    private void PlayButtonClicked()
    {
        SetCurrentWheel();
        SetWheel();
        SetSlots();
    }

    private void SetNextZone(WheelSlot wheelSlot)
    {
        zoneCount++;
        zoneCountText.text = "ZONE " + zoneCount;
        PlayButtonClicked();
    }

    private void SetCurrentWheel()
    {
        if (zoneCount % 30 == 0)
        {
            currentWheel_SO = wheel_SOs[2];
            safeSuperZoneText.gameObject.SetActive(true);
            safeSuperZoneText.text = "SUPER ZONE";
            cashOutButton.gameObject.SetActive(true);
        }
        else if (zoneCount % 5 == 0)
        {
            currentWheel_SO = wheel_SOs[1];
            safeSuperZoneText.gameObject.SetActive(true);
            safeSuperZoneText.text = "SAFE ZONE";
            cashOutButton.gameObject.SetActive(true);
        }
        else
        {
            currentWheel_SO = wheel_SOs[0];
            safeSuperZoneText.gameObject.SetActive(false);
            cashOutButton.gameObject.SetActive(false);
        }
    }

    public void SetWheel()
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
        zoneCount = 1;
        EventManager.CashOutButtonClicked();
    }
}
