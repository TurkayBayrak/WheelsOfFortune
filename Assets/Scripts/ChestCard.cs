using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class ChestCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemSecondNameText;
    [SerializeField] private TextMeshProUGUI itemAmountText;
    [SerializeField] private Image itemImage;

    public TextMeshProUGUI ItemNameText => itemNameText;
    public TextMeshProUGUI ItemSecondNameText => itemSecondNameText;
    public TextMeshProUGUI ItemAmountText => itemAmountText;
    public Image ItemImage => itemImage;

    [SerializeField] private CanvasGroup canvasGroup;

    private Vector2 cardPunchV2 = new(.05f, .05f);
    private Vector2 cardScaleV2 = new(.5f, .5f);
    private Vector3 cardRotationV3 = new(0, 0, 20);



    public void PlayCardRevealAnimation()
    {
        transform.DOScale(cardScaleV2, 0);

        var sequence = DOTween.Sequence();

        sequence.Append(transform.DORotate(Vector3.zero, .5f))
            .Join(transform.DOMoveY(400, .4f).SetEase(Ease.Linear))
            .Join(canvasGroup.DOFade(1, .5f))
            .Join(transform.DOScale(Vector2.one, .5f))
            .Append(transform.DOPunchScale(cardPunchV2, .5f, 1, .2f));
    }

    public void PlayShowAnimation()
    {
        transform.DOScale(cardScaleV2, 0);
        transform.DORotate(cardRotationV3,0);

        var sequence = DOTween.Sequence();

        sequence.Append(transform.DORotate(Vector3.zero, .5f))
            .Join(canvasGroup.DOFade(1, .5f))
            .Join(transform.DOScale(Vector2.one, .5f))
            .Append(transform.DOPunchScale(cardPunchV2, .5f, 1, .2f));
    }

    public void HideCard()
    {
        canvasGroup.DOFade(0, 0);
    }
}
