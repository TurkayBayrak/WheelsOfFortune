using UnityEngine;
using DG.Tweening;

public class CashAnimationController : MonoBehaviour
{
    [SerializeField] private RectTransform playButtonCashTransform;
    [SerializeField] private RectTransform mainMenuCashTransform;

    [SerializeField] private Transform[] cashTransforms;
    [SerializeField] private Transform[] cashFirstMoveTransforms;



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
            item.DOMove(mainMenuCashTransform.position, 0);
            item.gameObject.SetActive(true);

        }

        for (var i = 0; i < cashTransforms.Length; i++)
        {
            var sequence = DOTween.Sequence();
            sequence.Append(cashTransforms[i].DOMove(cashFirstMoveTransforms[i].position, .6f))
                .Append(cashTransforms[i].DOMove(playButtonCashTransform.position, .7f)).OnComplete(ResetAnimationObjects);
        }
    }

    private void ResetAnimationObjects()
    {
        foreach (Transform item in cashTransforms)
        {
            item.gameObject.SetActive(false);
            item.DOMove(mainMenuCashTransform.position, 0);
        }
    }
}