using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections;

public class RewardClaimController : MonoBehaviour
{
    [SerializeField] private Image rewardImage;
    [SerializeField] private TextMeshProUGUI rewardItemNameText;
    [SerializeField] private TextMeshProUGUI rewardItemSecondNameText;
    [SerializeField] private TextMeshProUGUI rewardAmountText;
    [SerializeField] private Transform glowTransform;
    [SerializeField] private Transform panelParentTransform;
    [SerializeField] private Transform claimButtonTransform;
    [SerializeField] private Button claimButton;
    [SerializeField] private CanvasGroup itemNameAmountCanvasGroup;

    private Image glowImage;
    private Image claimButtonImage;

    private TextMeshProUGUI claimButtonText;

    private WheelSlot currentWheelSlot;

    private Sequence rewardSequence;

    private Tween idleSpinTween;

    private const string OPEN_STRING = "OPEN";
    private const string CLAIM_STRING = "CLAIM";

    private Vector3 idelSpinV3 = new(0, 0, -360f);


    private void OnEnable()
    {
        EventManager.OnRewardReadyToBeClaimed += OnRewardReadyToBeClaimed;

        claimButton.onClick.AddListener(ClaimOpenButtonClickAction);

        glowImage = glowTransform.GetComponent<Image>();
        claimButtonImage = claimButton.gameObject.GetComponent<Image>();
        claimButtonText = claimButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void OnDisable()
    {
        EventManager.OnRewardReadyToBeClaimed -= OnRewardReadyToBeClaimed;

        claimButton.onClick.RemoveAllListeners();
    }

    private void OnRewardReadyToBeClaimed(WheelSlot wheelSlot)
    {
        if (wheelSlot.CurrentItem_SO.itemType == ItemTypes.None)
        {
            EventManager.BombIndicated();
            return;
        }

        claimButton.interactable = false;
        currentWheelSlot = wheelSlot;
        panelParentTransform.gameObject.SetActive(true);
        rewardImage.sprite = wheelSlot.CurrentItem_SO.itemSprite;
        rewardItemNameText.text = wheelSlot.CurrentItem_SO.itemName;
        rewardAmountText.text = "x" + wheelSlot.CurrentItemAmount;

        claimButtonText.text = wheelSlot.CurrentItem_SO.itemType is ItemTypes.Chest ? OPEN_STRING : CLAIM_STRING;

        rewardItemSecondNameText.gameObject.SetActive(false);

        if (wheelSlot.CurrentItem_SO.itemType == ItemTypes.SpecialItem)
        {
            var specialItem_SO = (SpecialItem_SO)wheelSlot.CurrentItem_SO;
            if (specialItem_SO.hasSecondName)
            {
                rewardItemSecondNameText.gameObject.SetActive(true);
                rewardItemSecondNameText.text = specialItem_SO.secondName;
                rewardItemSecondNameText.color = specialItem_SO.secondNameColor;
            }
        }


        StartRewardAnimation();
    }

    private void StartRewardAnimation()
    {
        rewardSequence = DOTween.Sequence();
        rewardSequence.Append(rewardImage.transform.DOScale(1, 1f))
            .Join(glowImage.DOFade(1, 1f))
            .Append(itemNameAmountCanvasGroup.DOFade(1, 1).OnComplete(() => claimButton.interactable = true))
            .Append(claimButtonImage.DOFade(1, .5f))
            .Join(claimButtonText.DOFade(1, .5f));

        idleSpinTween = glowTransform.DORotate(idelSpinV3, 10f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart).SetRelative().SetEase(Ease.Linear);
    }

    private void ClaimOpenButtonClickAction()
    {
        claimButton.interactable = false;
        if (currentWheelSlot.CurrentItem_SO.itemType is ItemTypes.Chest)
        {
            EventManager.ChestOpenButtonClicked((Chest_SO)currentWheelSlot.CurrentItem_SO);
            StartCoroutine(WaitCo(0));
        }
        else
        {
            EventManager.ItemClaimed(currentWheelSlot.CurrentItemAmount, currentWheelSlot.CurrentItem_SO, null);
            EventManager.BeginFade(1, 1f, true);
            StartCoroutine(WaitCo(.95f));
        }
    }

    private IEnumerator WaitCo(float value)
    {
        yield return new WaitForSeconds(value);
        rewardSequence.Kill();
        idleSpinTween.Kill();
        SetAlphaValues();
        panelParentTransform.gameObject.SetActive(false);
    }

    private void SetAlphaValues()
    {
        rewardImage.transform.DOScale(0, 0);
        glowImage.DOFade(0, 0f);
        itemNameAmountCanvasGroup.DOFade(0,0);
        claimButtonImage.DOFade(0, 0);
        claimButtonText.DOFade(0, 0);
    }
}
