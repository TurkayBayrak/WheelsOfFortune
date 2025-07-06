using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class WheelController : MonoBehaviour
{
    [SerializeField] private WheelSetter wheelSetter;

    [SerializeField] private RectTransform wheelTransform;
    [SerializeField] private Transform spinButtonTransform;
    [SerializeField] private Button spinButton;
    [SerializeField] private Transform panelParentTransform;
    [SerializeField] private Transform slotParentTransform;


    private bool isWheelSpinning;

    private Vector3 wheelRotateV3 = new(0, 0, -360f);

    private Tween idleSpinTween;

    private WheelSlot[] wheelSlots;

    public AnimationCurve customCurve;



    private void OnEnable()
    {
        EventManager.OnPlayButtonClicked += OnPlayButtonClicked;
        EventManager.OnItemClaimed += ItemClaimed;
        EventManager.OnCashOutButtonClicked += CashOutButtonClicked;
        EventManager.OnRevivedContinueButtonClicked += OnPlayButtonClicked;
        EventManager.OnGiveUpButtonClicked += GiveUpButtonClicked;

        spinButton.onClick.AddListener(SpinTheWheel);

        wheelSlots = slotParentTransform.GetComponentsInChildren<WheelSlot>();
    }

    private void OnDisable()
    {
        EventManager.OnPlayButtonClicked -= OnPlayButtonClicked;
        EventManager.OnItemClaimed -= ItemClaimed;
        EventManager.OnCashOutButtonClicked -= CashOutButtonClicked;
        EventManager.OnRevivedContinueButtonClicked -= OnPlayButtonClicked;
        EventManager.OnGiveUpButtonClicked -= GiveUpButtonClicked;

        spinButton.onClick.RemoveAllListeners();
    }

    private void GiveUpButtonClicked()
    {
        StartCoroutine(WaitCo());
    }

    private void CashOutButtonClicked()
    {
        spinButton.interactable = false;
        StartCoroutine(WaitCo());
    }

    private IEnumerator WaitCo()
    {
        yield return new WaitForSeconds(1.9f);
        idleSpinTween.Kill();
        panelParentTransform.gameObject.SetActive(false);
    }


    private void OnPlayButtonClicked()
    {
        panelParentTransform.gameObject.SetActive(true);
        spinButton.interactable = true;

        StartIdleSpin();
    }

    private void ItemClaimed(int amount, Item_SO item_SO = null, Item_SO[] item_SOs = null )
    {
        panelParentTransform.gameObject.SetActive(true);
        StartIdleSpin();
    }

    private void SpinTheWheel()
    {
        if (isWheelSpinning == true) return;

        EventManager.WheelSpinned();

        isWheelSpinning = true;

        idleSpinTween.Kill();

        wheelTransform.DOPunchScale(new Vector3(.1f, .1f, 0), .2f, 1, .2f);

        var residualRotationValue = 45 - (wheelTransform.eulerAngles.z % 45);

        var randomNumber = UnityEngine.Random.Range(0, 8);

        var rotationForRandomization = 45 * randomNumber;


        var spinSequence = DOTween.Sequence();

        spinSequence.Append(wheelTransform.DORotate(wheelRotateV3, .6f, RotateMode.FastBeyond360)
            .SetLoops(2, LoopType.Restart).SetRelative().SetEase(Ease.Linear))

            .Append(wheelTransform.DORotate(new Vector3(0, 0, residualRotationValue - rotationForRandomization), .6f * .125f * randomNumber, RotateMode.FastBeyond360)
            .SetRelative().SetEase(Ease.Linear))

            .Append(wheelTransform.DORotate(wheelRotateV3, .7f, RotateMode.FastBeyond360)
            .SetLoops(1, LoopType.Restart).SetRelative().SetEase(Ease.Linear))

            .Append(wheelTransform.DORotate(wheelRotateV3, 2.2f, RotateMode.FastBeyond360)
            .SetRelative().SetEase(customCurve).OnComplete(CalculateIndicatedSlot));
    }

    private void StartIdleSpin()
    {
        isWheelSpinning = false;
        idleSpinTween = wheelTransform.DORotate(wheelRotateV3, 10f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart).SetRelative().SetEase(Ease.Linear);
    }

    private void CalculateIndicatedSlot()
    {
        var slotIndex = Math.Floor((wheelTransform.eulerAngles.z + 5) / 45);
        slotIndex = Math.Clamp(slotIndex, 0, 7);

        StartCoroutine(WaitCo((int)slotIndex));

        IEnumerator WaitCo(int _slotIndex)
        {
            yield return new WaitForSeconds(.3f);
            EventManager.RewardReadyToBeClaimed(wheelSlots[_slotIndex]);
            yield return new WaitForSeconds(.3f);

            panelParentTransform.gameObject.SetActive(false);
        }
    }
}
