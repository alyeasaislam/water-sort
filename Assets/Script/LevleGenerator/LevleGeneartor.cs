using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevleGeneartor : MonoBehaviour
{

    public static LevleGeneartor instance;

    public GeneralGameSettings generalGameSettings;
    public GameLevelSettings levelSettings;
    private GameObject bottlePrefab;
    public GameObject BackgroundImage;
    public Transform[] bottlesPositions;

    public Transform parrentRefrence;

    public LineRenderer pouringEfectLine;

    public Text levelNumber, levelNumber_winPanel, level_difficulty_txt;
    public Text levelDifficulty_txt_startAnimaiton, levelNumber_txt_startAnimation;



    public Level m_currentLevel;

    public int temp_level_index;

    private int current_levelBottlesNumber, levelBottles_Number;
    private int bottle_fulled = 0;

    private LevelsCategories currentCategory;

    private int selectedCategoryLevels;

    // Start is called before the first frame update

    

    void Start()
    {

        setupBottlle_back();

        //   PlayerPrefs.SetInt("current_Level_Index", temp_level_index-1);


        m_currentLevel = setupCurrentLevel();


        generateLevel(m_currentLevel);

        // update game ui 
        updateGameUi(getCurrentLevelIndex());
        //updateGameUi(temp_level_index);
        levelBottles_Number = current_levelBottlesNumber = m_currentLevel.numberOfBottlesOnlevel.Length;

    }
    Level setupCurrentLevel()
    {



        string selectedCategoryName = PlayerPrefs.GetString("selected_category", "Easy");
        Level oldlevelinfo;

        switch (selectedCategoryName)
        {

            case "Easy":
                {
                    oldlevelinfo = levelSettings.Easy_levels[getCurrentLevelIndex() - 1];
                    currentCategory = LevelsCategories.Easy;

                    selectedCategoryLevels = levelSettings.Easy_levels.Length;

                }
                break;
            case "Normal":
                {
                    oldlevelinfo = levelSettings.normale_levels[getCurrentLevelIndex() - 1];
                    currentCategory = LevelsCategories.Normal;

                    selectedCategoryLevels = levelSettings.normale_levels.Length;

                }
                break;
            case "Hard":
                {
                    oldlevelinfo = levelSettings.hard_levels[getCurrentLevelIndex() - 1];
                    currentCategory = LevelsCategories.Hard;

                    selectedCategoryLevels = levelSettings.hard_levels.Length;


                }
                break;

            default:
                {
                    oldlevelinfo = levelSettings.Easy_levels[getCurrentLevelIndex() - 1];
                    currentCategory = LevelsCategories.Easy;

                    selectedCategoryLevels = levelSettings.Easy_levels.Length;

                }
                break;
        }



        //Level oldlevelinfo = levelSettings.game_levels[temp_level_index - 1];
        Level level = new Level();

        Bottle[] bottles1 = new Bottle[oldlevelinfo.numberOfBottlesOnlevel.Length];
        string name = "level" + (getCurrentLevelIndex() + 1);
        int bottletofild = oldlevelinfo.Numberofbottles_FilledToWin;

        for (int i = 0; i < bottles1.Length; i++)
        {
            string bottlename = "bottle" + (i + 1);
            int btof_n = oldlevelinfo.numberOfBottlesOnlevel[i].numberOfColorsInBottle;
            Color[] c2 = new Color[4];
            for (int j = 0; j < 4; j++)
            {
                c2[j] = oldlevelinfo.numberOfBottlesOnlevel[i].bottleColors[j];
            }
            Bottle current_bot = new Bottle();
            current_bot.Bottlename = bottlename;
            current_bot.bottleColors = c2;
            current_bot.numberOfColorsInBottle = btof_n;

            bottles1[i] = current_bot;


        }
        level.numberOfBottlesOnlevel = bottles1;
        level.level_name = name;
        level.Numberofbottles_FilledToWin = bottletofild;

        return level;
    }


    void setupBottlle_back()
    {
        int b = PlayerPrefs.GetInt("Selected_Back", 0);

        BackgroundImage.GetComponent<SpriteRenderer>().sprite = generalGameSettings.shopBackgrounds[b].prefabSprite;

        int c = PlayerPrefs.GetInt("Selected_Tube", 0);
        bottlePrefab = generalGameSettings.shopTubes[c].objectPrefab;


    }
    private void Awake()
    {
        // assign the bottle transfer speed 
        PlayerPrefs.SetFloat("game_bottle_speed_transfer", generalGameSettings.LiquidTransfertSpeed);

        if (instance == null)
        {
            instance = this;

        }
        else
        {

            Destroy(gameObject);

        }
    }


    void updateGameUi(int currentlevelindex)
    {
        int currentlevelNumber = currentlevelindex;
        //update level text;
        levelNumber.text = "Level " + currentlevelNumber;
        levelNumber_winPanel.text = "Level " + currentlevelNumber;

        level_difficulty_txt.text = currentCategory.ToString();


        levelDifficulty_txt_startAnimaiton.text = currentCategory.ToString();
        levelNumber_txt_startAnimation.text = "Level " + currentlevelNumber;




    }

    int getCurrentLevelIndex()
    {
        return PlayerPrefs.GetInt("current_Level_Index", 1);
    }
    void generateLevel(Level level)
    {
        //1 generate bottles 
        generateBottles(level);

    }


    /* Vector3 getPositions(int numberOfLevelBottles){



     }*/

    void generateBottles(Level currentlevel)
    {

        for (int i = 0; i < currentlevel.numberOfBottlesOnlevel.Length; i++)
        {
            GameObject mbottle = Instantiate(bottlePrefab, bottlesPositions[i].position, Quaternion.identity);
            mbottle.transform.parent = parrentRefrence;

            // maybe gives the colors here or let it a sepral function 
            mbottle.GetComponent<BottleController>().bottleColors = currentlevel.numberOfBottlesOnlevel[i].bottleColors;

            mbottle.GetComponent<BottleController>().lineRenderer = pouringEfectLine;

            // gives how many liquides on battle
            mbottle.GetComponent<BottleController>().numberOfColorsInBottle = currentlevel.numberOfBottlesOnlevel[i].numberOfColorsInBottle;
        }


    }


    public void addAnotherBottle()
    {
        current_levelBottlesNumber++;
        if (current_levelBottlesNumber <= bottlesPositions.Length)
        {
            GameObject newBottle = Instantiate(bottlePrefab, bottlesPositions[current_levelBottlesNumber - 1].position, Quaternion.identity);
            newBottle.transform.parent = parrentRefrence;
            newBottle.GetComponent<BottleController>().lineRenderer = pouringEfectLine;
            newBottle.GetComponent<BottleController>().numberOfColorsInBottle = 0;
        }

    }

    public void full_abottle()
    {

        // normalment check for the number of bottles fulled at the first time for each level not level bottles -2
        bottle_fulled++;

        // give the player a number of coins as a reward
        GameScManger.instance.givePlayerBottleCoins();



        if (bottle_fulled == m_currentLevel.Numberofbottles_FilledToWin)
        {
            // you win 
            GameScManger.instance.win_level();
        }
    }

    public void UndoPlyaerFullA_Bottle()
    {
        bottle_fulled--;

        // give the player a number of coins as a reward
        GameScManger.instance.undoRewardPlayer();
    }




    public void loadNextLevel()
    {
        //levelSettings.game_levels[getCurrentLevelIndex()]=m_currentLevel;


        int i = PlayerPrefs.GetInt("openLevelsFor" + currentCategory.ToString(), 1);




        //  int i = PlayerPrefs.GetInt("openLevels", 0);

        // 1* change the opens levels on the currrent category
        if (getCurrentLevelIndex() >= i)
        {
            PlayerPrefs.SetInt("openLevelsFor" + currentCategory.ToString(), getCurrentLevelIndex() + 1);
        }



        // 2* change the current level Index
        if (getCurrentLevelIndex() >= selectedCategoryLevels)
        {
            // the player finish all levels
            Debug.Log("you finish the game");
            SceneManager.LoadScene(0);
        }
        else
        {
            PlayerPrefs.SetInt("current_Level_Index", getCurrentLevelIndex() + 1);
            SceneManager.LoadScene(3);
        }




    }


    public void Skip_Level()
    {


        int i = PlayerPrefs.GetInt("openLevelsFor" + currentCategory.ToString(), 1);

        if (getCurrentLevelIndex() >= i)
        {
            PlayerPrefs.SetInt("openLevelsFor" + currentCategory.ToString(), getCurrentLevelIndex() + 1);
        }


        if (getCurrentLevelIndex() >= levelSettings.Easy_levels.Length)
        {
            // the player finish all levels
            Debug.Log("you finish the game");

            SceneManager.LoadScene(0);
        }
        else
        {

            PlayerPrefs.SetInt("current_Level_Index", getCurrentLevelIndex() + 1);
            SceneManager.LoadScene(1);
        }



    }

}
