using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "IAP_settings")]
public class IAP_Settings : ScriptableObject
{

    // iap settings, data
    [Header("IAP in app purchases ids")]
    [Space(30)]
    [SerializeField] string IapId_removeAds = "buy_Remove_Ads";
    [SerializeField] string IapId_BuyCoins_1 = "buy_stars_section_1";
    [SerializeField] string IapId_buyCoins_2 = "buy_stars_section_2";
    [SerializeField] string IapId_UnlockBottles = "buy_unlock_bottles";
    [SerializeField] string IapId_unlock_background = "buy_unlock_backgrounds";



    // iap settings, data
    [Header("IAP prices ")]
    [Space(30)]
    public string buyCoins_1_price;
    public string buyCoins_2_price;
    public string remove_ads_price;
    public string unlock_bottles_price;
    public string unlock_backgrounds_price;



    [Header("IAP stars value ")]
    [Space(30)]
    public int buy_Coinssection1_Coins_value;
    public int buy_Coinssection2_Coins_value;


}
