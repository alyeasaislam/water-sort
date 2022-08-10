using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopSystem_Manager : MonoBehaviour
{
    public static ShopSystem_Manager instance;


    public GeneralGameSettings m_generalGameSettings;
    public AudioSource sceneAudio_source;
    public GameObject notEnoughStarsPanel;

    public Text coinsNumberTxt;


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
        //PlayerPrefs.SetInt("game_coins_number", 10000);

        notEnoughStarsPanel.SetActive(false);
        checkSoundStatus();

        UpdateCoinsSHopPanel();
        // fill both lists (tubes and backs)
    }

    public void checkSoundStatus()
    {
        if (PlayerPrefs.GetInt("sound_status", 0) == 0)
        {
            sceneAudio_source.mute = true;
        }
        else
        {
            sceneAudio_source.mute = false;
        }

    }

    public void playAudioCLick()
    {
        sceneAudio_source.clip = m_generalGameSettings.click_sound;
        sceneAudio_source.Play();
    }


    public void LoadHomeScene()
    {
        SceneManager.LoadScene(0);
    }


    public void UpdateCoinsSHopPanel()
    {
        coinsNumberTxt.text = PlayerPrefs.GetInt("game_coins_number", 0).ToString();
    }

}
