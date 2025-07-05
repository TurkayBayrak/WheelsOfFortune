using System;
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

    private void SetCurrencyTextOnChange(int cashAmount, int goldAmount, bool playAnimation)
    {
        var currentCashAmount = Int32.TryParse(cashText.text, out int x);
        var currentGoldAmount = Int32.TryParse(goldText.text, out int y);

        DOVirtual.Int(x, cashAmount, 1.3f, (value) => cashText.text = value.ToString());
        DOVirtual.Int(y, goldAmount, 1.3f, (value) => goldText.text = value.ToString());
    }

}
