using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<WheelSlot> OnRewardReadyToBeClaimed;

    public static void RewardReadyToBeClaimed(WheelSlot wheelSlot)
    {
        OnRewardReadyToBeClaimed?.Invoke(wheelSlot);
    }

    public static Action<WheelSlot> OnItemClaimed;

    public static void ItemClaimed(WheelSlot wheelSlot)
    {
        OnItemClaimed?.Invoke(wheelSlot);
    }


    public static Action OnPlayButtonClicked;

    public static void PlayButtonClicked()
    {
        OnPlayButtonClicked?.Invoke();
    }

    public static Action OnGiveUpButtonClicked;

    public static void GiveUpButtonClicked()
    {
        OnGiveUpButtonClicked?.Invoke();
    }

    public static Action OnBeginFade;

    public static void BeginFade()
    {
        OnBeginFade?.Invoke();
    }



    public static Action OnRevivedContinueButtonClicked;

    public static void RevivedContinueButtonClicked()
    {
        OnRevivedContinueButtonClicked?.Invoke();
    }


    public static Action OnInventoryButtonClicked;

    public static void InventoryButtonClicked()
    {
        OnInventoryButtonClicked?.Invoke();
    }


    public static Action OnWheelSpinned;

    public static void WheelSpinned()
    {
        OnWheelSpinned?.Invoke();
    }

    public static Action OnBombIndicated;

    public static void BombIndicated()
    {
        OnBombIndicated?.Invoke();
    }


    public static Action OnCashOutButtonClicked;

    public static void CashOutButtonClicked()
    {
        OnCashOutButtonClicked?.Invoke();
    }

    public static Action<bool> OnInsufficientAmount;

    public static void InsufficientAmount(bool isCash)
    {
        OnInsufficientAmount?.Invoke(isCash);
    }


    public static Action<WheelSlot> OnChestOpenButtonClicked;

    public static void ChestOpenButtonClicked(WheelSlot wheelSlot)
    {
        OnChestOpenButtonClicked?.Invoke(wheelSlot);
    }

    public static Action<int, int, bool> OnCurrencyAmountChanged;

    public static void CurrencyAmountChanged(int cashValue, int goldValue, bool playAnimation)
    {
        OnCurrencyAmountChanged?.Invoke(cashValue, goldValue, playAnimation);
    }


    public static Action<int, int, bool> OnCurrencyAmountSet;

    public static void CurrencyAmountSet(int cashValue, int goldValue, bool playAnimation)
    {
        OnCurrencyAmountSet?.Invoke(cashValue, goldValue, playAnimation);
    }

}
