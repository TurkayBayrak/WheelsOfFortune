using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InsufficientAmountController : MonoBehaviour
{
    [SerializeField] private Transform panelParentTransform;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Button okButton;

    private void OnEnable()
    {
        EventManager.OnInsufficientAmount += InsufficientAmount;
        okButton.onClick.AddListener(OkButtonOnClickAction);
    }

    private void OnDisable()
    {
        EventManager.OnInsufficientAmount -= InsufficientAmount;
        okButton.onClick.RemoveAllListeners();
    }

    private void InsufficientAmount(bool isCash)
    {
        panelParentTransform.gameObject.SetActive(true);
        var prefix = "INSUFFICIENT ";
        var currencyString = isCash ? "CASH" : "GOLD";
        infoText.text = prefix + currencyString;
    }

    private void OkButtonOnClickAction()
    {
        panelParentTransform.gameObject.SetActive(false);
    }
}
