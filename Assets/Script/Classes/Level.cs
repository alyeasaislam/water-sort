using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{

    public Level()
    {

    }


    public string level_name;

    public int Numberofbottles_FilledToWin;

    public Bottle[] numberOfBottlesOnlevel;
    // if you instentiate 12 bottles you should let 2 bottles empety 

    public Level(string name, int numberofbottl, Bottle[] bottles)
    {
        level_name = name;
        Numberofbottles_FilledToWin = numberofbottl;
        numberOfBottlesOnlevel = bottles;

    }

}
