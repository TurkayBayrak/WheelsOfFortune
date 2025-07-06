using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Fader : MonoBehaviour
{
    [SerializeField] private Image fadeImageBlack;
    [SerializeField] private Image fadeImagewhite;

    private void OnEnable()
    {
        EventManager.OnBeginFade += BeginFade;
    }

    private void OnDisable()
    {
        EventManager.OnBeginFade -= BeginFade;
    }

    private void BeginFade(float value, float duration, bool isBlack)
    {
        var fadeImage = isBlack ? fadeImageBlack : fadeImagewhite;

        var sequence = DOTween.Sequence();
        sequence.Append(fadeImage.DOFade(value, duration)).Append(fadeImage.DOFade(0, duration));
    }
}
