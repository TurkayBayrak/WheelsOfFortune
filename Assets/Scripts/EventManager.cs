using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<WheelSlot> OnRewardReadyToBeClaimed;

    public static void RewardReadyToBeClaimed(WheelSlot wheelSlot)
    {
        OnRewardReadyToBeClaimed?.Invoke(wheelSlot);
    }

    public static Action<int, Item_SO, Item_SO[]> OnItemClaimed;

    public static void ItemClaimed(int amount, Item_SO item_SO, Item_SO[] item_SOs)
    {
        OnItemClaimed?.Invoke(amount, item_SO, item_SOs);
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

    public static Action<float, float, bool> OnBeginFade;

    public static void BeginFade(float value, float duration, bool isBlack)
    {
        OnBeginFade?.Invoke(value, duration, isBlack);
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

    public static Action OnPlayCashAnimamation;

    public static void PlayCashAnimamation()
    {
        OnPlayCashAnimamation?.Invoke();
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

    public static Action<Chest_SO> OnChestOpenButtonClicked;

    public static void ChestOpenButtonClicked(Chest_SO chestSO)
    {
        OnChestOpenButtonClicked?.Invoke(chestSO);
    }

    public static Action<int, int, bool> OnCurrencyAmountChanged;

    public static void CurrencyAmountChanged(int amount, int currencyId, bool playAnimation)
    {
        OnCurrencyAmountChanged?.Invoke(amount, currencyId, playAnimation);
    }

    public static Action<int, int, int, bool> OnCurrencyAmountSet;

    public static void CurrencyAmountSet(int amount, int currentAmount, int currencyId, bool playAnimation)
    {
        OnCurrencyAmountSet?.Invoke(amount, currentAmount, currencyId, playAnimation);
    }
}
