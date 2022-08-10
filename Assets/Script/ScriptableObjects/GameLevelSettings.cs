using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "Level_Settings")]
public class GameLevelSettings : ScriptableObject
{

    [Header("easy Levels list")]
    [Space(50)]
    public Level[] Easy_levels;



    [Header("normales levels list")]
    [Space(50)]
    public Level[] normale_levels;



    [Header("hard levels list")]
    [Space(50)]
    public Level[] hard_levels;

}
