using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSCgmManager : MonoBehaviour
{


    // there are 4 scnese

    // scene 01 : index(0) main menu scene 
    // scene 02 : index(1) select level
    // scene 03 : index(2) shop lists .."list of bottles + lsit of bakcgrounds + lsit of items"
    // scene 04 : index(3) game scene where the player gonna transfer liquides between bottles.



    public static MainSCgmManager instance;
    public GeneralGameSettings general_gamesettings;


    // ui buttons
    public Button sound_BTN;
    public Sprite mutedImage, non_muted_img;

    // ui pannels 
    public GameObject level_panel;

    public AudioListener main_Audio_listiner;
    public AudioSource main_Ui_source;
    private AudioClip click_Sound;


    // GDPR ui
    public Text GDPR_Message_txt;
    public GameObject GDPR_panel;


    // coins system ui
    public Text coins_number_txt;


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
        Screen.orientation = ScreenOrientation.Portrait;

        confirmGdprStatus();

        click_Sound = general_gamesettings.click_sound;


        // update the coins on the text home
        updateCoinsNumber();

        // check the sound status if it's muted or not
        checkSoundStatus();

        //desable panel on the level loaded
        level_panel.SetActive(false);

    }


    public void checkSoundStatus()
    {
        if (PlayerPrefs.GetInt("sound_status", 1) == 0)
        {
            mute_audio();
        }
        else
        {
            inmute_audio();
        }

    }




    #region coins system

    public void clickOnAddCoins()
    {
        SceneManager.LoadScene(2);
    }
    public void updateCoinsNumber()
    {
        coins_number_txt.text = PlayerPrefs.GetInt("game_coins_number", 0).ToString();
    }

    #endregion


    #region GDPR events

    //******************************************************************************
    //gdpr events

    // there are 3 status for the gdpr status player prefs
    // -1 theres is no data about the player if he accept or refuse yet
    // 0 the player decline the request of collecting data
    // 1 the player accept the request of collecting data 

    public void confirmGdprStatus()
    {
        GDPR_Message_txt.text = general_gamesettings.GDPR_message;

        int gpdr_status = PlayerPrefs.GetInt("gdpr_status", -1);

        if (gpdr_status == -1)
        {
            showGdprPanel();    // shows for the first time
        }
        else
        {
            return;
        }


    }


    public void showGdprPanel()
    {
        GDPR_Message_txt.text = general_gamesettings.GDPR_message;
        //PlayerPrefs.SetInt("gdpr_status", 0);
        //  0 : accept data usage
        //  1 : desmiss data usage
        //  -1 : doesnt Opened yet
        GDPR_panel.SetActive(true);
    }
    public void closeGdprPanel()
    {
        GDPR_panel.SetActive(false);
    }

    public void openPrivacyPolicy()
    {
        Application.OpenURL(general_gamesettings.privacy_Policy_Link);
    }

    public void acceptDatausage()
    {
        PlayerPrefs.SetInt("gdpr_status", 0);   // the palyer accept then show the personalized ads 
        // GameUnityAds.instance.useraccept();
        closeGdprPanel();


    }
    public void DesmissDataUsage()
    {
        PlayerPrefs.SetInt("gdpr_status", 1);   // the user desmmiss then show the non personalized ads 
        //  GameUnityAds.instance.userDessmis();
        closeGdprPanel();
    }

    #endregion

    #region  buttons clicked listeners 

    //play button clicked
    public void clickOnPlay()
    {
        // when the player click on play, the scne number 1 gonna load 
        SceneManager.LoadScene(1);
        // level_panel.SetActive(true);
    }



    public void clickOnShop()
    {
        SceneManager.LoadScene(2);
    }


    #region settings panel events

    public void clickOnMoreGames()
    {
        //show more games 
        Application.OpenURL(general_gamesettings.moreGamesLink);

    }

    public void clickOnInstagram()
    {
        Application.OpenURL("instagram://user?username=" + general_gamesettings.instagram_username);

    }
    public void clickOnFacebookBtn()
    {
        Application.OpenURL("fb://page/" + general_gamesettings.facebook_page_ID);
    }

    #endregion



    public void clickOnQuit()
    {

        Application.Quit();


    }



    #endregion

    #region  sound system

    public void clickOnSoundBtn()
    {
        int a = PlayerPrefs.GetInt("sound_status", 1);
        if (a == 0)
        {
            //audio muted we need to enable it 
            inmute_audio();
        }
        else
        {
            mute_audio();
        }


    }

    public void mute_audio()
    {
        PlayerPrefs.SetInt("sound_status", 0); //no sound (muted) 
        sound_BTN.GetComponent<Image>().sprite = mutedImage;
        main_Ui_source.mute = true;

    }
    public void inmute_audio()
    {
        PlayerPrefs.SetInt("sound_status", 1);  // sound not muted   
        sound_BTN.GetComponent<Image>().sprite = non_muted_img;
        main_Ui_source.mute = false;
    }

    public void playClikAudio()
    {
        main_Ui_source.clip = click_Sound;
        main_Ui_source.Play();
    }

    #endregion




}
