using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScManger : MonoBehaviour
{

    public static GameScManger instance;

    public GeneralGameSettings generalGameSettings;

    //ui pannels
    public GameObject pausePanel, winPanel, levelListPanel, notEnoughGames, addCoinsAfterBottleFilled;
    public AudioClip winSound, Click_sound;
    public AudioSource game_AudioSOurce;
    public AudioListener mainLisitner_audio;


    // ui buttons
    public Button sound_BTN;
    public Sprite mutedImage, non_muted_img;



    // ui prices to skip and add more bottles 
    public Text skipLevelPrice_txt, undoPriceTxt, addBottlePirce_txt, CoinsUIText, coinsTextWinPanel;


    // singilton design pattern
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


    // Start is called before the first frame update
    void Start()
    {
        updateMain_CoinsVlue();
        // check the sound status
        checkSound_status();
        setupUiButtonsPrices();


        Click_sound = generalGameSettings.click_sound;

        Screen.orientation = ScreenOrientation.Portrait;

        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        notEnoughGames.SetActive(false);
        addCoinsAfterBottleFilled.SetActive(false);


        // ads 
        GameAds.instance.loadInterstitialAd();
        GameAds.instance.showbannerAD();










    }



    public void givePlayerBottleCoins()
    {
        int coins = PlayerPrefs.GetInt("game_coins_number");
        coins += generalGameSettings.coinsNumberRewarded_bottleFillsUp;

        addCoinsAfterBottleFilled.SetActive(true);


        // show the  animation 
        PlayerPrefs.SetInt("game_coins_number", coins);

        updateMain_CoinsVlue();

    }

    public void undoRewardPlayer()
    {
        int coins = PlayerPrefs.GetInt("game_coins_number");
        coins -= generalGameSettings.coinsNumberRewarded_bottleFillsUp;

        // show the  animation 
        PlayerPrefs.SetInt("game_coins_number", coins);

        updateMain_CoinsVlue();
    }

    void updateMain_CoinsVlue()
    {
        // update the coins ui 
        CoinsUIText.text = PlayerPrefs.GetInt("game_coins_number").ToString();

    }




    void setupUiButtonsPrices()
    {
        skipLevelPrice_txt.text = generalGameSettings.coinsNumber_to_skip.ToString();
        undoPriceTxt.text = generalGameSettings.coinsNumber_to_UndoMoves.ToString();
        addBottlePirce_txt.text = generalGameSettings.coinsNumber_to_AddnewBottle.ToString();
    }




    private void checkSound_status()
    {

        // check sound status 
        int i = PlayerPrefs.GetInt("sound_status", 1);
        if (i == 0)
        {
            mute_audio();
        }
        else
        {
            inmute_audio();
        }

    }

    public void mute_audio()
    {
        PlayerPrefs.SetInt("sound_status", 0); //no sound (muted) 
        sound_BTN.GetComponent<Image>().sprite = mutedImage;
        game_AudioSOurce.mute = true;
        AudioListener.pause = true;


    }
    public void inmute_audio()
    {
        PlayerPrefs.SetInt("sound_status", 1);  // sound not muted   
        sound_BTN.GetComponent<Image>().sprite = non_muted_img;
        game_AudioSOurce.mute = false;
        AudioListener.pause = false;

    }








    #region  ui manager in game


    public void openLevelList()
    {
        // open levle ist
        SceneManager.LoadScene(1);
    }


    public void openMainMenu()
    {
        // load main scene
        SceneManager.LoadScene(0);
    }

    // principal menu butotns 
    public void restartBtnCLikced()
    {
        SceneManager.LoadScene(3);
    }


    public void addOneTubeBtnClicked()
    {
        int coins = getcurrentCoinsNumber();
        if (coins >= generalGameSettings.coinsNumber_to_AddnewBottle)
        {
            coins -= generalGameSettings.coinsNumber_to_AddnewBottle;
            PlayerPrefs.SetInt("game_coins_number", coins);

            // add a new bottle to the game
            LevleGeneartor.instance.addAnotherBottle();

            updateMain_CoinsVlue();

        }

        else
        {
            notEnoughGames.SetActive(true);
        }


        /* GameUnityAds.instance.status_reward = 0;
         GameUnityAds.instance.ShowRewardedVideo();*/
    }

    public void UndoMoveCLicked()
    {




        int coins = getcurrentCoinsNumber();
        if (coins >= generalGameSettings.coinsNumber_to_UndoMoves)
        {
            if (GameController.instance.doesUndoAvailibal() == true)
            {
                coins -= generalGameSettings.coinsNumber_to_UndoMoves;
                PlayerPrefs.SetInt("game_coins_number", coins);

                // add a new bottle to the game
                updateMain_CoinsVlue();



                GameController.instance.makeUndoMovement();

            }

        }
        else
        {
            notEnoughGames.SetActive(true);
        }


        // do undo moves with the needed coins number
    }

    public void clickOnSkipbutoon()
    {
        int coins = getcurrentCoinsNumber();
        if (coins >= generalGameSettings.coinsNumber_to_skip)
        {
            coins -= generalGameSettings.coinsNumber_to_skip;
            PlayerPrefs.SetInt("game_coins_number", coins);

            // skip the game 
            updateMain_CoinsVlue();

            LevleGeneartor.instance.loadNextLevel();


        }
        else
        {
            notEnoughGames.SetActive(true);
        }
    }


    int getcurrentCoinsNumber()
    {
        return PlayerPrefs.GetInt("game_coins_number", 0);
    }

    #endregion



    #region  win panel butotns
    public void win_level()
    {
        int n_win = PlayerPrefs.GetInt("n_win_value", 0);
        n_win++;
        if (n_win >= generalGameSettings.showInterstitialAfter_n_win)
        {
            //show interstitial ad
            GameAds.instance.ShowInterstitialAd();

            // give the n_win 0
            n_win = 0;
        }
        // update the n_win value on playerprefs
        PlayerPrefs.SetInt("n_win_value", n_win);



        // change the coins number on win panel
        coinsTextWinPanel.text = PlayerPrefs.GetInt("game_coins_number").ToString();


        winPanel.SetActive(true);
        game_AudioSOurce.clip = winSound;
        game_AudioSOurce.Play();

    }



    public void bottleFullAddCoins()
    {

    }



    public void palyClickSound()
    {
        game_AudioSOurce.clip = Click_sound;
        game_AudioSOurce.Play();
    }
    #endregion

    #region sound manager 

    public void CLickOnSOundBtn()
    {

        int a = PlayerPrefs.GetInt("sound_status", 1);
        if (a == 0)
        {
            inmute_audio();
        }
        else
        {
            mute_audio();
        }




    }


    public void openHomeScene()
    {
        SceneManager.LoadScene(0);
    }


    #endregion


    #region undo event






    #endregion

}
