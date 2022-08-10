using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Bottle
{
    // the bottle class
    public string Bottlename;
    public Color[] bottleColors;

    [Range(0, 4)] public int numberOfColorsInBottle=4;  // this is for how much colors on the bottle
    // if the value is 0 then the bottle should be empety 
}
