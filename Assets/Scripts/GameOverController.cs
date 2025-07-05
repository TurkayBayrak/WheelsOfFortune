using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private Transform panelParentTransform;
    [SerializeField] private Transform revivedPanelTransform;
    [SerializeField] private Button giveUpButton;
    [SerializeField] private Button reviveButton;
    [SerializeField] private Button watchAdButton;
    [SerializeField] private Button revivedContinueButton;
    [SerializeField] private Image fadeImageBlack;
    [SerializeField] private Image fadeImagewhite;


    [SerializeField] private CanvasGroup textParentCanvasGroup;
    [SerializeField] private CanvasGroup buttonsParentCanvasGroup;

    [SerializeField] private Image deathImage;

    [SerializeField] private Inventory inventory;


    private Sequence deathSequence;
    private Sequence fadeSequence;


    private void OnEnable()
    {
        giveUpButton.onClick.AddListener(GiveUpButtonOnClickAction);
        reviveButton.onClick.AddListener(ReviveButtonOnClickAction);
        watchAdButton.onClick.AddListener(WatchAdButtonOnClickAction);
        revivedContinueButton.onClick.AddListener(RevivedContinueButtonOnClickAction);

        EventManager.OnBombIndicated += BombIndicated;
    }

    private void OnDisable()
    {
        giveUpButton.onClick.RemoveAllListeners();
        reviveButton.onClick.RemoveAllListeners();
        watchAdButton.onClick.RemoveAllListeners();
        revivedContinueButton.onClick.RemoveAllListeners();

        EventManager.OnBombIndicated -= BombIndicated;
    }

    private void BombIndicated()
    {
        SetButtonsInteractable(false);
        deathImage.transform.DOScale(new Vector2(.5f, .5f), 0);
        textParentCanvasGroup.DOFade(0,0);
        buttonsParentCanvasGroup.DOFade(0,0);
        deathImage.DOFade(0,0);

        panelParentTransform.gameObject.SetActive(true);


        deathSequence = DOTween.Sequence();
        deathSequence.Append(deathImage.transform.DOScale(1, 1f))
            .Join(deathImage.DOFade(1, 1f))
            .Append(textParentCanvasGroup.DOFade(1, 1).OnComplete(() => SetButtonsInteractable(true)))
            .Append(buttonsParentCanvasGroup.DOFade(1, 1));
    }

    private void SetButtonsInteractable(bool value)
    {
        giveUpButton.interactable = value;
        reviveButton.interactable = value;
        watchAdButton.interactable = value;
    }


    private void GiveUpButtonOnClickAction()
    {
        fadeSequence = DOTween.Sequence();
        fadeSequence.Append(fadeImageBlack.DOFade(1, 2).OnComplete(SetPanelsDeactive)).Append(fadeImageBlack.DOFade(0, 2));
        EventManager.GiveUpButtonClicked();
    }

    private void ReviveButtonOnClickAction()
    {
        if (inventory.IsAmountInsufficient(-25, 1))
        {
            EventManager.InsufficientAmount(false);
        }
        else
        {
            EventManager.CurrencyAmountChanged(0, -25, false);
            revivedPanelTransform.gameObject.SetActive(true);
        }
    }

    private void RevivedContinueButtonOnClickAction()
    {
        fadeSequence = DOTween.Sequence();
        fadeSequence.Append(fadeImagewhite.DOFade(1, 2).OnComplete(SetPanelsDeactive)).Append(fadeImagewhite.DOFade(0, 2));
        EventManager.RevivedContinueButtonClicked();
    }

    private void SetPanelsDeactive()
    {
        revivedPanelTransform.gameObject.SetActive(false);
        panelParentTransform.gameObject.SetActive(false);
    }

    private void WatchAdButtonOnClickAction()
    {
        EventManager.CurrencyAmountChanged(0, -25, false);
        revivedPanelTransform.gameObject.SetActive(true);
    }

}
