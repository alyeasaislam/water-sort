using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "General Game Settings")]

public class GeneralGameSettings : ScriptableObject
{

    [Header("social media accounts")]
    public string moreGamesLink;
    public string instagram_username;
    public string facebook_page_ID;


    [Header("Game ads settings")]
    [Space(30)]
    public string gameId;
    public string InterstitialAdId;
    public string BannerAd_ID;
    public int showInterstitialAfter_n_win;
    public bool test_Mode = true;




    [Header("GDPR settings")]
    [Space(50)]
    public string GDPR_message;
    public string privacy_Policy_Link;



    [Header("others proprites")]
    [Space(50)]
    public int coinsNumber_to_skip;
    public int coinsNumber_to_AddnewBottle;
    public int coinsNumber_to_UndoMoves;
    public int coinsNumberRewarded_bottleFillsUp;
    public AudioClip click_sound;

    [Range(1,2)]
    public float LiquidTransfertSpeed=1.8f;



    [Header("shop list")]
    [Header("tubes")]
    [Space(50)]
    public TubeBack[] shopTubes;


    [Header("backgrounds")]
    [Space(30)]
    public TubeBack[] shopBackgrounds;


}
