using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainLevelListManager : MonoBehaviour
{




    public static MainLevelListManager instance;   // sinigliton design pattern

    public GameLevelSettings m_levelSettings; // to get the level settings from 



    //SOUND
    public AudioSource audioSource;
    public GeneralGameSettings m_generalGameSettings;


    //private values
    private int curentLevel; //current leve number 

    public GameObject buttonsParrent, list_panel;
    public Text categoryText;
    public ScrollRect listscrollrect;

    public Button levelButtonPrefab;
    List<Button> levelListButton;

    private int difficulty_level;

    private Level[] levels_fordifficlt;




    // Start is called before the first frame update
    void Start()
    {


        levelListButton = new List<Button>();

        checkSoundStatus();



        //   instantiateButtons();
    }
    public void checkSoundStatus()
    {
        if (PlayerPrefs.GetInt("sound_status", 0) == 0)
        {
            audioSource.mute = true;
        }
        else
        {
            audioSource.mute = false;
        }

    }




    #region  levels categories events

    public void selectEasyLevels()
    {
        // give the lsit of easy levels to the levels list creator 
        // start instentiating levels

        //clearTheListButtons();
        instantiateButtons(LevelsCategories.Easy);
        categoryText.text = "EASY";

        list_panel.SetActive(true);


        // then show active the list levels

    }
    public void selectNormaleLevels()
    {
        // give the lsit of normale or meduim levels to the levels list creator 
        // start instentiating levels

        // clearTheListButtons();
        instantiateButtons(LevelsCategories.Normal);
        categoryText.text = "NORMAL";

        list_panel.SetActive(true);


    }
    public void selectHardLevels()
    {
        // give the lsit of hard levels to the levels list creator 
        // start instentiating levels

        // clearTheListButtons();
        instantiateButtons(LevelsCategories.Hard);
        categoryText.text = "HARD";

        list_panel.SetActive(true);

    }



    public void playCLickAudio()
    {
        audioSource.clip = m_generalGameSettings.click_sound;
        audioSource.Play();

    }



    public void goHomeScene()
    {
        SceneManager.LoadScene(0);
    }

    public void closeLevelsPanel()
    {
        clearTheListButtons();
        list_panel.SetActive(false);
    }




    #endregion






    void clearTheListButtons()
    {
        StartCoroutine(ScrollToTop());

        // clear the list by destroy all the buttons objects to be able to add new buttons to the list
        for (int i = 0; i < levelListButton.Count; i++)
        {
            Destroy(levelListButton[i].gameObject);
        }


    }

    IEnumerator ScrollToTop()
    {
        yield return new WaitForEndOfFrame();
        listscrollrect.verticalNormalizedPosition = 1f;
    }






    public void instantiateButtons(LevelsCategories selectedCategory)
    {
        int buttonsNumbers;
        // get the selected catgory buttons

        switch (selectedCategory)
        {
            case LevelsCategories.Easy:
                {

                    buttonsNumbers = m_levelSettings.Easy_levels.Length;
                }
                break;
            case LevelsCategories.Normal:
                {
                    buttonsNumbers = m_levelSettings.normale_levels.Length;

                }
                break;
            case LevelsCategories.Hard:
                {
                    buttonsNumbers = m_levelSettings.hard_levels.Length;

                }
                break;
            default:
                buttonsNumbers = 10;
                Debug.Log("there is an error getting levls numbers");
                break;
        }




        // i want  to store buttons in this list 
        levelListButton = new List<Button>();

        for (int i = 0; i < buttonsNumbers; i++)
        {
            int copy = i + 1;
            // instantiate buttons 
            Button currentButton = Instantiate(levelButtonPrefab, levelButtonPrefab.transform.position, levelButtonPrefab.transform.rotation);
            currentButton.transform.SetParent(buttonsParrent.transform, false);

            // give those new buttons listener ... when the buton clicked 
            currentButton.onClick.AddListener(() => startLevels(copy, selectedCategory));

            // note  : i+1 alawyse when i click any i have the same value .. it's 21


            levelListButton.Add(currentButton);

        }


        settupTheLivelListButtons(levelListButton, getUnlockedLevelForType(selectedCategory));

    }





    int getUnlockedLevelForType(LevelsCategories categories)
    {


        int OpenLevelsIndex = PlayerPrefs.GetInt("openLevelsFor" + categories.ToString(), 1);

       // return 1050;

        return OpenLevelsIndex;

        /* switch (categories)
         {
             case LevelsCategories.Easy:
                 {

                 }
                 break;
             case LevelsCategories.Normale:
                 {

                 }
                 break;
             case LevelsCategories.Hard:
                 {

                 }
                 break;

         }*/




    }

    // give buttons status if it's locked or not 
    void settupTheLivelListButtons(List<Button> buttons, int OpenLevesIndex)
    {

        Button[] newArray = buttons.ToArray();
        curentLevel = OpenLevesIndex;   // getting  the number of open levels
        for (int i = 0; i < buttons.Count; i++)
        {
            newArray[i].GetComponentInChildren<Text>().text = (i + 1) + "";

            newArray[i].transform.GetChild(0).GetComponentInChildren<Text>().text = i + 1 + "";


            if (i + 1 > curentLevel)
            {
                newArray[i].interactable = false;
                newArray[i].transform.GetChild(1).GetComponentInChildren<Image>().gameObject.SetActive(true);


            }
            else
            {
                newArray[i].transform.GetChild(1).GetComponentInChildren<Image>().gameObject.SetActive(false);

            }


        }

    }


    public void startLevels(int LevelIndex,LevelsCategories selectedCategory)
    {
        int curentLevelNumber = LevelIndex;
        // save the level index to use in game scean

        PlayerPrefs.SetString("selected_category", selectedCategory.ToString());

        PlayerPrefs.SetInt("current_Level_Index", curentLevelNumber);     //save the current level to get it on the game scen to open the curent level


        //start the game level
        SceanLoader.instance.LoadScean(3);


    }


}


public enum LevelsCategories { Easy, Normal, Hard };