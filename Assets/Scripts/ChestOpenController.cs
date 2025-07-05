using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class ChestOpenController : MonoBehaviour
{
    [SerializeField] private Transform panelParentTransform;
    [SerializeField] private Transform chestItemsParentTransform;
    [SerializeField] private Transform chestCardDefaultPositionTransform;


    [SerializeField] private Image chestImage;

    [SerializeField] private Button nextCardButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button skipButton;


    [SerializeField] private TextMeshProUGUI cardCountIndicatorText;
    [SerializeField] private Transform cardIndicatorTransform;


    [SerializeField] private Transform contentTransform;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform itemsEarnedTextTransform;

    private CanvasGroup continueButtonCanvasGroup;

    private readonly List<ChestCard> chestCards = new();
    private int playedCardCount;
    private WheelSlot currentWheelSlot;

    private void OnEnable()
    {
        EventManager.OnChestOpenButtonClicked += PlayChestOpenningSequence;

        nextCardButton.onClick.AddListener(NextCardButtonOnClickAction);
        continueButton.onClick.AddListener(ContinueButtonClickAction);
        skipButton.onClick.AddListener(SkipCardRevealAnimations);

        continueButtonCanvasGroup = continueButton.gameObject.GetComponent<CanvasGroup>();
    }

    private void OnDisable()
    {
        EventManager.OnChestOpenButtonClicked -= PlayChestOpenningSequence;

        nextCardButton.onClick.RemoveAllListeners();
        continueButton.onClick.RemoveAllListeners();
        skipButton.onClick.RemoveAllListeners();
    }

    private void PlayChestOpenningSequence(WheelSlot wheelSlot)
    {
        continueButton.interactable = false;
        continueButtonCanvasGroup.DOFade(0, 0);
        currentWheelSlot = wheelSlot;
        panelParentTransform.gameObject.SetActive(true);
        cardIndicatorTransform.gameObject.SetActive(true);
        chestImage.gameObject.SetActive(true);


        var chestSO = (Chest_SO)wheelSlot.CurrentItem_SO;

        chestImage.sprite = chestSO.itemSprite;


        for (var i = 0; i < chestSO.item_SOs.Length; i++)
        {
            var chestCardGO = Instantiate(Resources.Load("ChestCard", typeof(GameObject))) as GameObject;

            chestCardGO.transform.DOScale(1, 0);

            chestCardGO.transform.parent = chestItemsParentTransform;

            chestCardGO.transform.position = chestCardDefaultPositionTransform.position;

            var chestCard = chestCardGO.GetComponent<ChestCard>();

            chestCard.ItemImage.sprite = chestSO.item_SOs[i].itemSprite;
            chestCard.ItemNameText.text = chestSO.item_SOs[i].itemName;
            chestCard.ItemAmountText.text = "x" + chestSO.item_SOs[i].defaultItemAmount;
            chestCard.ItemSecondNameText.gameObject.SetActive(false);


            if (chestSO.item_SOs[i].itemType == ItemTypes.SpecialItem)
            {
                var specialItemSO = (SpecialItem_SO)chestSO.item_SOs[i];
                if (specialItemSO.hasSecondName)
                {
                    chestCard.ItemSecondNameText.gameObject.SetActive(true);
                    chestCard.ItemSecondNameText.text = specialItemSO.secondName;
                    chestCard.ItemSecondNameText.color = specialItemSO.secondNameColor;
                }
            }

            chestCards.Add(chestCard);

            if (i == 0)
            {
                chestCard.PlayCardRevealAnimation();
                playedCardCount++;
                cardCountIndicatorText.text = (chestSO.item_SOs.Length - playedCardCount).ToString();
                nextCardButton.gameObject.SetActive(true);
                skipButton.gameObject.SetActive(true);
            }

        }
    }

    private void NextCardButtonOnClickAction()
    {
        if (playedCardCount == chestCards.Count)
        {
            chestCards[playedCardCount - 1].HideCard();
            cardIndicatorTransform.gameObject.SetActive(false);
            chestImage.gameObject.SetActive(false);
            nextCardButton.gameObject.SetActive(false);
            itemsEarnedTextTransform.gameObject.SetActive(true);
            skipButton.gameObject.SetActive(false);


            SetScroll();


            foreach (var chestCard in chestCards)
            {
                chestCard.transform.SetParent(contentTransform);
                chestCard.PlayShowAnimation();
            }


            StartCoroutine(WaitCo());

            IEnumerator WaitCo()
            {
                yield return new WaitForSeconds(1f);
                continueButton.interactable = true;
                continueButtonCanvasGroup.DOFade(1, .5f);
            }
        }
        else
        {
            chestCards[playedCardCount - 1].HideCard();
            chestCards[playedCardCount].PlayCardRevealAnimation();
            playedCardCount++;
            cardCountIndicatorText.text = (chestCards.Count - playedCardCount).ToString();
            StartCoroutine(WaitForCardRevealAnimationCo());
        }
    }


    private IEnumerator WaitForCardRevealAnimationCo()
    {
        nextCardButton.interactable = false;
        yield return new WaitForSeconds(.6f);
        nextCardButton.interactable = true;
    }

    private void ContinueButtonClickAction()
    {
        continueButton.interactable = false;
        panelParentTransform.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);

        playedCardCount = 0;
        EventManager.ItemClaimed(currentWheelSlot);

        foreach (var chestCard in chestCards)
        {
            Destroy(chestCard.gameObject);
        }

        chestCards.Clear();

        itemsEarnedTextTransform.gameObject.SetActive(false);
    }

    private void SkipCardRevealAnimations()
    {
        skipButton.gameObject.SetActive(false);
        playedCardCount = chestCards.Count;
        NextCardButtonOnClickAction();
    }

    private void SetScroll()
    {
        if (chestCards.Count > 4)
        {
            scrollRect.horizontal = true;
            var spacingTotal = contentTransform.GetComponent<HorizontalLayoutGroup>().spacing * (chestCards.Count - 1);
            var deltaSize = chestCards[0].GetComponent<RectTransform>().sizeDelta.x * chestCards.Count + spacingTotal + 80;
            var contentRectTransform = contentTransform.GetComponent<RectTransform>();
            contentRectTransform.DOSizeDelta(new Vector2(deltaSize, contentRectTransform.sizeDelta.y), 0);
        }
        else
        {
            scrollRect.horizontal = false;
        }
    }
}
