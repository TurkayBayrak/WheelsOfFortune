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

    private void BeginFade()
    {

    }
}
