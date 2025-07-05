using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Transform wheelTransform;
    [SerializeField] private Transform playButtonTransform;
    [SerializeField] private Button playButton;
    [SerializeField] private Button inventoryButton;
    [SerializeField] private Image fadeImage;
    [SerializeField] private Transform panelParentTransform;
    [SerializeField] private Inventory inventory;

    private Vector3 wheelRotateV3 = new(0, 0, -360f);
    private Vector3 playButtonScaleV3 = new(1.05f, 1.05f, 1);

    private Sequence idleSequence;



    private void OnEnable()
    {
        EventManager.OnCashOutButtonClicked += CashOutButtonClicked;
        EventManager.OnGiveUpButtonClicked += GiveUpButtonClicked;


        fadeImage.DOFade(1, 0);

        StartCoroutine(WaitCo());

        IEnumerator WaitCo()
        {
            yield return new WaitForSeconds(1.5f);
            fadeImage.DOFade(0, 1);
        }

        playButton.onClick.AddListener(PlayButtonClickedAction);
        inventoryButton.onClick.AddListener(EventManager.InventoryButtonClicked);

        PlayIdleAnimation();
    }


    private void CashOutButtonClicked()
    {
        fadeImage.DOFade(1, 1).OnComplete(()=>fadeImage.DOFade(0, 2));
        SetMainMenuOnReturn();
    }

    private void GiveUpButtonClicked()
    {
        SetMainMenuOnReturn();
    }

    private void SetMainMenuOnReturn()
    {
        panelParentTransform.gameObject.SetActive(true);
        PlayIdleAnimation();
        playButton.interactable = true;
        inventoryButton.interactable = true;
    }


    private void PlayEntryFeeAnimation()
    {
        
    }


    private void PlayButtonClickedAction()
    {
        if (inventory.IsAmountInsufficient(-1000, 0))
        {
            EventManager.InsufficientAmount(false);
            return;
        }

        playButton.interactable = false;
        inventoryButton.interactable = false;
        EventManager.CurrencyAmountChanged(-1000, 0, true);

        StartCoroutine(WaitForCurrencyAnimation());

        IEnumerator WaitForCurrencyAnimation()
        {
            yield return new WaitForSeconds(1.5f);
            fadeImage.DOFade(1, 1).OnComplete(SwitchToGamePanel);
        }
    }


    private void SwitchToGamePanel()
    {
        idleSequence.Kill();

        EventManager.PlayButtonClicked();

        fadeImage.DOFade(0, 2);

        panelParentTransform.gameObject.SetActive(false);
    }

    private void PlayIdleAnimation()
    {
        idleSequence = DOTween.Sequence();
        idleSequence.Append(playButtonTransform.DOScale(playButtonScaleV3, 1).SetLoops(-1, LoopType.Yoyo))
            .Join(wheelTransform.DORotate(wheelRotateV3, 10f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart).SetRelative().SetEase(Ease.Linear));
    }
}
