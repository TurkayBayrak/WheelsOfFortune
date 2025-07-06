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
    private readonly List<Item_SO> currentChestItemsList = new();

    private int playedCardCount;


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

    private void PlayChestOpenningSequence(Chest_SO chestSO)
    {
        continueButton.interactable = false;
        continueButtonCanvasGroup.DOFade(0, 0);
        panelParentTransform.gameObject.SetActive(true);
        cardIndicatorTransform.gameObject.SetActive(true);
        chestImage.gameObject.SetActive(true);

        chestImage.sprite = chestSO.itemSprite;


        for (var i = 0; i < chestSO.itemAmountToGive; i++)
        {
            var chestCardGO = Instantiate(Resources.Load("Prefabs/ChestCard", typeof(GameObject))) as GameObject;

            chestCardGO.transform.DOScale(1, 0);

            chestCardGO.transform.SetParent(chestItemsParentTransform);

            chestCardGO.transform.position = chestCardDefaultPositionTransform.position;

            var chestCard = chestCardGO.GetComponent<ChestCard>();

            var itemTypeRandom = Random.Range(0,100);

            if (itemTypeRandom < chestSO.currencyValue)
            {
                var poolRandom = Random.Range(0, chestSO.currencyItemPool.Length);
                var itemSo = chestSO.currencyItemPool[poolRandom];

                SetChestCard(chestCard, itemSo);
            }
            else if (itemTypeRandom < chestSO.currencyValue + chestSO.specialValue)
            {
                var poolRandom = Random.Range(0, chestSO.specialItemPool.Length);
                var itemSo = (SpecialItem_SO)chestSO.specialItemPool[poolRandom];

                SetChestCard(chestCard, itemSo);

                if (itemSo.hasSecondName)
                {
                    chestCard.ItemSecondNameText.gameObject.SetActive(true);
                    chestCard.ItemSecondNameText.text = itemSo.secondName;
                    chestCard.ItemSecondNameText.color = itemSo.secondNameColor;
                }
            }
            else
            {
                var poolRandom = Random.Range(0, chestSO.upgradeItemPool.Length);
                var itemSo = chestSO.upgradeItemPool[poolRandom];
                SetChestCard(chestCard, itemSo);
            }

            if (i == 0)
            {
                chestCard.PlayCardRevealAnimation();
                playedCardCount++;
                cardCountIndicatorText.text = (chestSO.itemAmountToGive - playedCardCount).ToString();
                nextCardButton.gameObject.SetActive(true);
                skipButton.gameObject.SetActive(true);
            }
        }
    }

    private void SetChestCard(ChestCard chestCard, Item_SO itemSo)
    {
        chestCard.ItemImage.sprite = itemSo.itemSprite;
        chestCard.ItemNameText.text = itemSo.itemName;
        chestCard.ItemAmountText.text = "x" + itemSo.defaultItemAmount;
        chestCard.ItemSecondNameText.gameObject.SetActive(false);

        chestCards.Add(chestCard);
        currentChestItemsList.Add(itemSo);
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


            var childWidth = chestCards[0].GetComponent<RectTransform>().sizeDelta.x;
            ScrollSetter.SetScroll(scrollRect, chestCards.Count, childWidth, 4);


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
        EventManager.BeginFade(1, 1f, true);
        StartCoroutine(WaitForFade());

        continueButton.interactable = false;
        skipButton.gameObject.SetActive(false);

        playedCardCount = 0;


        var item_SOs = currentChestItemsList.ToArray();
        EventManager.ItemClaimed(0, null, item_SOs);

        foreach (var chestCard in chestCards)
        {
            Destroy(chestCard.gameObject);
        }

        chestCards.Clear();
        currentChestItemsList.Clear();


        itemsEarnedTextTransform.gameObject.SetActive(false);
    }

    private IEnumerator WaitForFade()
    {
        yield return new WaitForSeconds(.95f);
        panelParentTransform.gameObject.SetActive(false);
    }

    private void SkipCardRevealAnimations()
    {
        skipButton.gameObject.SetActive(false);
        playedCardCount = chestCards.Count;
        NextCardButtonOnClickAction();
    }
}
