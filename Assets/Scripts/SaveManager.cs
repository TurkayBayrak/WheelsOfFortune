using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private int firstSessionCashAmount = 10000;
    private int firstSessionGoldAmount = 50;


    private void Start()
    {
        if (!PlayerPrefs.HasKey("FirstSession") || PlayerPrefs.GetInt("FirstSession") == 0)
        {

            EventManager.CurrencyAmountChanged(firstSessionCashAmount, firstSessionGoldAmount, false);
            //SetFirstSessionValue(1);
        }
    }

    public static void SetFirstSessionValue(int value)
    {
        PlayerPrefs.SetInt("FirstSession" , value);
    }
}
