using UnityEngine;
using DG.Tweening;
using TMPro;


public class CurrencyTextSetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cashText;
    [SerializeField] private TextMeshProUGUI goldText;


    private void OnEnable()
    {
        EventManager.OnCurrencyAmountSet += SetCurrencyTextOnChange;
    }

    private void OnDisable()
    {
        EventManager.OnCurrencyAmountSet -= SetCurrencyTextOnChange;
    }

    private void SetCurrencyTextOnChange(int amount, int currentAmount, int currencyId, bool playAnimation)
    {
        var textMesh = currencyId == 0 ? cashText : goldText;

        DOVirtual.Int(currentAmount, amount, 1.3f, (value) => textMesh.text = value.ToString());
    }
}
