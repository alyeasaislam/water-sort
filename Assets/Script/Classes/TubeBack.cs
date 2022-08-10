using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TubeBack
{

    // this class describe the both of the backgrounds and the bottles to be used on the game

    public GameObject objectPrefab; //this is the game object of (bottles / background)
    public int coinsNumber_To_Unlock;

    public bool isOpened;

    public Sprite prefabSprite;  // to show the object on the list
}
