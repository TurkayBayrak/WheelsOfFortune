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
    [SerializeField] private CanvasGroup revivedPanelCanvasGroup;


    [SerializeField] private Image deathImage;
    [SerializeField] private Inventory inventory;


    private Sequence deathSequence;


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
        textParentCanvasGroup.DOFade(0,0);
        buttonsParentCanvasGroup.DOFade(0,0);

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
        EventManager.BeginFade(1, 2, true);
        StartCoroutine(WaitForFade());
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
            reviveButton.interactable = false;
            revivedPanelCanvasGroup.DOFade(0, 0);
            EventManager.CurrencyAmountChanged(-25, 1, false);
            revivedPanelTransform.gameObject.SetActive(true);
            revivedPanelCanvasGroup.DOFade(1, 2);
            revivedContinueButton.interactable = true;
        }
    }

    private void RevivedContinueButtonOnClickAction()
    {
        revivedContinueButton.interactable = false;
        EventManager.BeginFade(2, 2, false);
        StartCoroutine(WaitForFade());
        EventManager.RevivedContinueButtonClicked();
    }

    IEnumerator WaitForFade()
    {
        yield return new WaitForSeconds(1.8f);
        deathImage.transform.DOScale(new Vector2(.5f, .5f), 0);
        deathImage.DOFade(0, 0);
        textParentCanvasGroup.DOFade(0, 0);
        buttonsParentCanvasGroup.DOFade(0, 0);
        SetPanelsDeactive();
    }

    private void WatchAdButtonOnClickAction()
    {
        watchAdButton.interactable = false;
        revivedPanelCanvasGroup.DOFade(0, 0);
        revivedPanelTransform.gameObject.SetActive(true);
        revivedPanelCanvasGroup.DOFade(1, 2);
        revivedContinueButton.interactable = true;
    }

    private void SetPanelsDeactive()
    {
        revivedPanelTransform.gameObject.SetActive(false);
        panelParentTransform.gameObject.SetActive(false);
    }
}
