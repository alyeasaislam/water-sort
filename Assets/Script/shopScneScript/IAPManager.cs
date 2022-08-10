using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IAPManager : MonoBehaviour
{

    public IAP_Settings in_app_buyer_settings;
    public GeneralGameSettings m_general_settings;

    public GameObject acitonField_panel, purchasing_complete_panel;

    //coins values 
    public Text CoinsSection_1_stars;
    public Text CoinsSection_2_stars;




    // prices values
    public Text CoinsSection_1_Price;
    public Text CoinsSection_2_price;
    public Text remove_ads_price;
    public Text unlock_all_bottles_Price;
    public Text unlock_all_backgrounds_Price;





    // Start is called before the first frame update
    void Start()
    {
        setupIapPrices();
        // inActive_ActionFieldPanel();
    }




    void setupIapPrices()
    {
        //setup the number of stars 
        CoinsSection_1_stars.text = in_app_buyer_settings.buy_Coinssection1_Coins_value.ToString();
        CoinsSection_2_stars.text = in_app_buyer_settings.buy_Coinssection2_Coins_value.ToString();


        // setup iap prices
        CoinsSection_1_Price.text = in_app_buyer_settings.buyCoins_1_price;
        CoinsSection_2_price.text = in_app_buyer_settings.buyCoins_2_price;
        remove_ads_price.text = in_app_buyer_settings.remove_ads_price;
        unlock_all_bottles_Price.text = in_app_buyer_settings.unlock_bottles_price;
        unlock_all_backgrounds_Price.text = in_app_buyer_settings.unlock_backgrounds_price;
    }


    public void purchaseCoins_part1()
    {

        // inform the player that the purchasing is complete 
        showPurchasing_complete();

        int coins1 = getCurrentCoinsValue() + in_app_buyer_settings.buy_Coinssection1_Coins_value;
        PlayerPrefs.SetInt("game_coins_number", coins1);

        // show purchasing complet

        // update the ui
        //  MainMneuManager.instance.updateCoinsValue();
        // shopPanel.SetActive(false);

        Invoke("loadHomeMenu", 2.0f);
        //ShopSystem_Manager.instance.LoadHomeScene();


        // add1000stars();
    }
    public void purchaseCoins_part2()
    {

        // inform the player that the purchasing is complete 
        showPurchasing_complete();


        int coins2 = getCurrentCoinsValue() + in_app_buyer_settings.buy_Coinssection2_Coins_value;
        PlayerPrefs.SetInt("game_coins_number", coins2);


        Invoke("loadHomeMenu", 2.0f);
        //ShopSystem_Manager.instance.LoadHomeScene();


    }

    int getCurrentCoinsValue()
    {
        return PlayerPrefs.GetInt("game_coins_number", 0);
    }


    public void purchasRemoveAds()
    {
        PlayerPrefs.SetInt("isPlayer_BuyNoAds", 1); // that's mean there is no ads anymore

        showPurchasing_complete();


        Invoke("loadHomeMenu", 2.0f);
        // ShopSystem_Manager.instance.LoadHomeScene();


    }


    public void purchasesNotCompleted()
    {
        acitonField_panel.SetActive(true);
    }


    public void inActive_ActionFieldPanel()
    {
        acitonField_panel.SetActive(false);
    }

    void showPurchasing_complete()
    {
        purchasing_complete_panel.SetActive(true);
    }



    // purshasing items


    // buy Unlock ALL bottles
    public void purchasUnlockBottles()
    {
        Debug.Log(" thank you you comfired that you purchases remove ads");


        unlockAllBottles();

        showPurchasing_complete();


        Invoke("loadHomeMenu", 2.0f);
        // ShopSystem_Manager.instance.LoadHomeScene();





    }
    // buy Unlock ALL backgrounds
    public void purchasUnlockbackgrounds()
    {


        unlockAllBacks();
        showPurchasing_complete();

        Invoke("loadHomeMenu", 2.0f);
        // ShopSystem_Manager.instance.LoadHomeScene();


    }



    public void loadHomeMenu()
    {
        SceneManager.LoadScene(0);

    }





    #region purchasing_events

    void unlockAllBacks()
    {
        for (int i = 0; i < m_general_settings.shopBackgrounds.Length; i++)
        {
            m_general_settings.shopBackgrounds[i].isOpened = true;
        }
        PlayerPrefs.SetInt("all_BackgroundsPurchased", 1);


    }
    void unlockAllBottles()
    {
        for (int i = 0; i < m_general_settings.shopTubes.Length; i++)
        {
            m_general_settings.shopTubes[i].isOpened = true;
        }
        PlayerPrefs.SetInt("all_tubesPurchased", 1);
    }



    #endregion
}
