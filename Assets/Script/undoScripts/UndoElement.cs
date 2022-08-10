using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class UndoElement
{

    public int moveNumber;
    public Bottle undoFirst_btl, undoSeccond_btl;
    public BottleController undoFirst_ctrl, undoSeccond_ctrl;

}
