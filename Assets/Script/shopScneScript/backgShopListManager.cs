using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class backgShopListManager : MonoBehaviour
{

    public Button backButtonPrefab;

    public GameObject shop_panel, notEnoughCoins_Panel;

    // parents objects
    public Transform backsParents;

    private Button[] backs_list_buttons;

    List<Button> BacksButtonObjects;

    public GeneralGameSettings generalGameSettings;

    public SpriteRenderer selectedBackground;


    // sprites status for the image
    public Sprite lockedShopItem_sprite, opensShopItem_sprite, selectedShopItem_sprite;


    // Start is called before the first frame update
    void Start()
    {
        // set the old selected sprite for the background
        selectedBackground.sprite = generalGameSettings.shopBackgrounds[PlayerPrefs.GetInt("Selected_Back", 0)].prefabSprite;


        generateBackButtons();
        notEnoughCoins_Panel.SetActive(false);
    }


    // back butotns
    void generateBackButtons()
    {
        BacksButtonObjects = new List<Button>();

        TubeBack[] backShopList = generalGameSettings.shopBackgrounds;
        for (int i = 0; i < backShopList.Length; i++)
        {
            int copy = i + 1;
            // instantiate buttons 
            Button currentButton = Instantiate(backButtonPrefab, backButtonPrefab.transform.position, Quaternion.identity);
            currentButton.transform.SetParent(backsParents.transform, false);

            // give those new buttons listener ... when the buton clicked 
            currentButton.onClick.AddListener(() => selectBack(copy));
            BacksButtonObjects.Add(currentButton);

        }

        // then setup those buttons give back locked/not ............................................
        int current_level = PlayerPrefs.GetInt("openLevels", 1);   // getting  the number of open levels
        // curentLevel = 1;

        for (int i = 0; i < backShopList.Length; i++)
        {

            // give the shop item a priview image of the background
            BacksButtonObjects[i].transform.GetChild(0).GetComponentInChildren<Image>().sprite = backShopList[i].prefabSprite; // button sprite
                                                                                                                               // Debug.Log("the number is" + i);



            // is the background locked or unlocked
            if (backShopList[i].isOpened == false)      // that's means it's locked
            {
                // its locked assign the locked panel
                BacksButtonObjects[i].transform.GetComponentInChildren<Image>().sprite = lockedShopItem_sprite;


                // active the locked panel
                BacksButtonObjects[i].transform.GetChild(2).GetComponentInChildren<Image>().gameObject.SetActive(true);

                // because it's locked update the price to open text
                BacksButtonObjects[i].transform.GetChild(1).GetComponentInChildren<Text>().text = backShopList[i].coinsNumber_To_Unlock.ToString();  // button level to open

            }
            else
            {
                // its opened assign the open panel
                BacksButtonObjects[i].interactable = true;

                // its locked assign the locked panel
                BacksButtonObjects[i].transform.GetComponentInChildren<Image>().sprite = opensShopItem_sprite;


                BacksButtonObjects[i].transform.GetChild(2).gameObject.SetActive(false);

                BacksButtonObjects[i].transform.GetChild(1).GetComponentInChildren<Text>().text = "";  // button level to open

            }

            int selected_item = PlayerPrefs.GetInt("Selected_Back", 0);
            if (i == selected_item)
            {
                BacksButtonObjects[i].transform.GetComponentInChildren<Image>().sprite = selectedShopItem_sprite;
            }
        }

    }


    void selectBack(int index)
    {

        TubeBack selectedBACK = generalGameSettings.shopBackgrounds[index - 1];

        if (selectedBACK.isOpened == false)
        {
            // then try to open it 

            int playerGameCoins = PlayerPrefs.GetInt("game_coins_number", 0);
            if (playerGameCoins >= selectedBACK.coinsNumber_To_Unlock)
            {

                playerGameCoins -= selectedBACK.coinsNumber_To_Unlock;
                selectedBACK.isOpened = true;
                PlayerPrefs.SetInt("game_coins_number", playerGameCoins);

                ShopSystem_Manager.instance.UpdateCoinsSHopPanel();


                // its locked assign the locked panel
                BacksButtonObjects[index - 1].transform.GetComponentInChildren<Image>().sprite = opensShopItem_sprite;


                BacksButtonObjects[index - 1].transform.GetChild(2).gameObject.SetActive(false);

                BacksButtonObjects[index - 1].transform.GetChild(1).GetComponentInChildren<Text>().text = "";  // button level to open

            }
            else
            {
                Debug.Log("not enough coins you have, please recharge your coins");
                notEnoughCoins_Panel.SetActive(true);
            }

        }
        else
        {

            // the item is opened so select that item
            int oldselected = PlayerPrefs.GetInt("Selected_Back", 0);
            BacksButtonObjects[oldselected].transform.GetComponentInChildren<Image>().sprite = opensShopItem_sprite;

            PlayerPrefs.SetInt("Selected_Back", index - 1);

            BacksButtonObjects[index - 1].transform.GetComponentInChildren<Image>().sprite = selectedShopItem_sprite;

            selectedBackground.sprite = generalGameSettings.shopBackgrounds[index - 1].prefabSprite;

        }


        /*int oldselected = PlayerPrefs.GetInt("Selected_Back", 0);
        BacksButtonObjects[oldselected].transform.GetComponentInChildren<Image>().sprite = opensShopItem_sprite;

        PlayerPrefs.SetInt("Selected_Back", index - 1);

        BacksButtonObjects[index - 1].transform.GetComponentInChildren<Image>().sprite = selectedShopItem_sprite;*/

    }

}
