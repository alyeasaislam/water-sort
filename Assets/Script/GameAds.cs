using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


public class GameAds : MonoBehaviour
{
    public GeneralGameSettings m_gamesettings;
    public static GameAds instance;


    private string game_id, interstitial_id, banner_id;
    private bool testMode;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {

            Destroy(gameObject);

        }
    }


    void getUserAdsId()
    {
        game_id = m_gamesettings.gameId;
        interstitial_id = m_gamesettings.InterstitialAdId;
        banner_id = m_gamesettings.BannerAd_ID;
        testMode = m_gamesettings.test_Mode;

    }


    // Start is called before the first frame update
    void Start()
    {
        getUserAdsId();
        ADS_Initialization();

    }

    void ADS_Initialization()
    {
        // Advertisement.AddListener(this);
        Advertisement.Initialize(game_id, testMode);

        MetaData gdprMetaData = new MetaData("gdpr");

        int GdprValue = PlayerPrefs.GetInt("gdpr_status", 0);
        //  0 : accept data usage
        //  1 : desmiss data usage

        if (GdprValue == 0)
        {
            // If the user opts in to targeted advertising:
            gdprMetaData.Set("consent", "true");
            Advertisement.SetMetaData(gdprMetaData);
        }
        if (GdprValue == 1)
        {
            // If the user opts out of targeted advertising:
            gdprMetaData.Set("consent", "false");
            Advertisement.SetMetaData(gdprMetaData);
        }
    }


    bool showAds_isRemoved()
    {
        int Removeadsvalue = PlayerPrefs.GetInt("isPlayer_BuyNoAds", 0);

        if (Removeadsvalue == 0)
        {
            //the player doesnt buy remove ads yet
            return false;
        }
        else
        {
            //the player buy remove ads 
            return true;
        }
    }


    #region interstitial ads

    public void loadInterstitialAd()
    {

        if (showAds_isRemoved() == false)
        {
            Advertisement.Load(interstitial_id);
            Advertisement.Load(banner_id);
        }


    }

    public void ShowInterstitialAd()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady() && showAds_isRemoved() == false)
        {
            Advertisement.Show(interstitial_id);
            // Replace mySurfacingId with the ID of the placements you wish to display as shown in your Unity Dashboard.
        }
        else
        {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }
    #endregion

    #region banner Ad

    public void showbannerAD()
    {
        if (showAds_isRemoved() == false)
        {
            StartCoroutine(ShowBannerWhenInitialized());
        }
    }


    IEnumerator ShowBannerWhenInitialized()
    {

        while (!Advertisement.IsReady(banner_id))
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(banner_id);
    }


    public void hideBanner()
    {
        Advertisement.Banner.Hide();
    }


    #endregion
}
