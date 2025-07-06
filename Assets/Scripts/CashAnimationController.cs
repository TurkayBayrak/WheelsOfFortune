using UnityEngine;
using DG.Tweening;

public class CashAnimationController : MonoBehaviour
{
    [SerializeField] private RectTransform playButtonCashTransform;

    [SerializeField] private Transform[] cashTransforms;

    [SerializeField] private Vector2[] cashFirstMovePaths;

    private Vector2 defaultPosV2 = new(593, 434);

    private void OnEnable()
    {
        EventManager.OnPlayCashAnimamation += PlayMoveCashAnimation;
    }


    private void OnDisable()
    {
        EventManager.OnPlayCashAnimamation -= PlayMoveCashAnimation;
    }


    public void PlayMoveCashAnimation()
    {
        foreach (Transform item in cashTransforms)
        {
            item.gameObject.SetActive(true);
        }

        for (var i = 0; i < cashTransforms.Length; i++)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(cashTransforms[i].DOLocalMove(cashFirstMovePaths[i], .6f))
                .Append(cashTransforms[i].DOMove(playButtonCashTransform.position, .7f)).OnComplete(ResetAnimationObjects);
        }
    }

    private void ResetAnimationObjects()
    {
        foreach (Transform item in cashTransforms)
        {
            item.gameObject.SetActive(false);
            item.DOLocalMove(defaultPosV2, 0);
        }
    }
}