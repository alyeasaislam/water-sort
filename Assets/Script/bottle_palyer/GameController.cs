using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class GameController : MonoBehaviour
{

    public static GameController instance;
    public AudioSource audioSource;
    public AudioClip tapclip;
    public BottleController FirstBottle;
    public BottleController SecondBottle;

    public GameObject BottleClickedVfx;

    public LineRenderer lineRenderer;

    public bool is_transform_avilibal = true;

    // undo elements
    public List<UndoElement> undoElements;





    void Start()
    {
        undoElements = new List<UndoElement>();

        audioSource.clip = tapclip;
    }

    public void setTheAbilityTrans(bool b1)
    {
        is_transform_avilibal = b1;
    }


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



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsPointerOverUIObject() == false)
        {

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)

            {
                if (hit.collider.GetComponent<BottleController>() != null)
                {
                    if (FirstBottle == null)
                    {

                        FirstBottle = hit.collider.GetComponent<BottleController>();
                        FirstBottle.lineRenderer = lineRenderer;

                        //play sound click 
                        audioSource.Play();

                        // the bottle should move up a little 
                        FirstBottle.bottleSelected();

                        // here show selected effect
                        //****
                    }
                    else
                    {
                        if (FirstBottle == hit.collider.GetComponent<BottleController>())
                        {
                            // desable selected effect
                            //***


                            FirstBottle.bottleInselected();
                            FirstBottle = null;
                            // the bottle should move down a little 
                            //play sound click 
                            audioSource.Play();

                        }
                        else
                        {
                            if (is_transform_avilibal == true)
                            {

                                SecondBottle = hit.collider.GetComponent<BottleController>();
                                FirstBottle.bottleControllerRef = SecondBottle;

                                //play sound click 
                                audioSource.Play();

                                FirstBottle.UpadateTopColorValues();
                                SecondBottle.UpadateTopColorValues();



                                if (SecondBottle.FillbottleCheck(FirstBottle.topColor) == true)
                                {
                                    // if they are the same 
                                    FirstBottle.startColorTransfer();



                                    Bottle first_btl = new Bottle();
                                    Color[] firstColors = new Color[4];

                                    for(int i = 0; i < 4; i++)
                                    {
                                        firstColors[i] = FirstBottle.bottleColors[i];
                                    }





                                    first_btl.bottleColors = firstColors;
                                    int numbercolrosfirst = FirstBottle.numberOfColorsInBottle;
                                    first_btl.numberOfColorsInBottle = numbercolrosfirst;



                                    Bottle seccond_btl = new Bottle();
                                    Color[] secondColors = new Color[4];


                                    for (int i = 0; i < 4; i++)
                                    {
                                        secondColors[i] = SecondBottle.bottleColors[i];
                                    }


                                    seccond_btl.bottleColors = secondColors;
                                    int secondnumbercolor= SecondBottle.numberOfColorsInBottle;
                                    seccond_btl.numberOfColorsInBottle = secondnumbercolor;


                                    // we did an action so the undo action is 
                                    UndoElement n_element = new UndoElement();
                                    n_element.undoFirst_ctrl = FirstBottle;
                                    n_element.undoSeccond_ctrl = SecondBottle;

                                    n_element.undoFirst_btl = first_btl;
                                    n_element.undoSeccond_btl = seccond_btl;

                                    addNewActionToUndo(n_element);


                                    /*Debug.Log(" the current movment data ");
                                    Debug.Log(" the first bottle colors number " + n_element.undoFirst_btl.numberOfColorsInBottle);
                                    Debug.Log(" the secoond bottle colors number " + n_element.undoSeccond_btl.numberOfColorsInBottle);*/



                                    //FirstBottle.GetComponent<Bo>.enabled = false;
                                    //SecondBottle.m_boxColider.enabled = false;
                                    FirstBottle = null;
                                    SecondBottle = null;

                                    // made a vibration or an animation
                                    // instentiate an effect to tell that are'nt the same

                                    // disabel the effect
                                    //******


                                }
                                else
                                {
                                    // the color transfer is impossible 
                                    FirstBottle.playNot_mus_anime();
                                    FirstBottle.bottleInselected();
                                    FirstBottle = null;
                                    SecondBottle = null;


                                    // show not much animation 
                                    //*************





                                }
                            }
                        }
                    }
                }
            }
        }




    }

    //to get if the palyer click on an ui element or on an object
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }



    void addNewActionToUndo(UndoElement theElement)
    {
        undoElements.Add(theElement);


        for (int i = 0; i < undoElements.Count; i++)
        {
            undoElements[i].moveNumber = i;

        }









    }
    UndoElement retriveLastActionFromUndo()
    {
        UndoElement undo = new UndoElement();
        /*Bottle btl_f1 = new Bottle();
        btl_f1.numberOfColorsInBottle = undoElements[undoElements.Count - 1].undoFirst_btl.numberOfColorsInBottle;
        btl_f1.bottleColors = undoElements[undoElements.Count - 1].undoFirst_btl.bottleColors;



        Bottle btl_f2 = new Bottle();
        btl_f2.numberOfColorsInBottle = undoElements[undoElements.Count - 1].undoSeccond_btl.numberOfColorsInBottle;
        btl_f2.bottleColors = undoElements[undoElements.Count - 1].undoSeccond_ctrl.bottleColors;

        BottleController ctrl_1 = new BottleController();
        ctrl_1 = undoElements[undoElements.Count - 1].undoFirst_ctrl;


        BottleController ctrl_2 = new BottleController();
        ctrl_2 = undoElements[undoElements.Count - 1].undoSeccond_ctrl;


        undo.undoFirst_btl = btl_f1;
        undo.undoSeccond_btl = btl_f2;
        undo.undoFirst_ctrl = ctrl_1;
        undo.undoSeccond_ctrl = ctrl_2;*/
        undo = undoElements[undoElements.Count - 1];


        undo.moveNumber = undoElements[undoElements.Count - 1].moveNumber;


        for (int i = 0; i < undoElements.Count; i++)
        {

           /* Debug.Log("the move number" + (i + 1));
            Debug.Log("the first bottle data" + ": bottle number colors =" + undoElements[i].undoFirst_btl.numberOfColorsInBottle + " seccond element"
                + undoElements[i].undoSeccond_btl.numberOfColorsInBottle);*/

        }




        /* // undo data is 
         Debug.Log("the move number" + (undoElements.Count - 1));
         Debug.Log("the first bottle data" + ": bottle number colors =" + undo.undoFirst_btl.numberOfColorsInBottle + " seccond element"
             + undo.undoSeccond_btl.numberOfColorsInBottle);*/




        return undo;

    }

    public void makeUndoMovement()
    {




        UndoElement nextAction = new UndoElement();
        nextAction = retriveLastActionFromUndo();

        nextAction.undoFirst_ctrl.bottleColors = nextAction.undoFirst_btl.bottleColors;
        nextAction.undoFirst_ctrl.numberOfColorsInBottle = nextAction.undoFirst_btl.numberOfColorsInBottle;
        nextAction.undoFirst_ctrl.setBottlemaskr();
        nextAction.undoFirst_ctrl.UpadateTopColorValues();
        nextAction.undoFirst_ctrl.UpdateColorsOnShader();



        nextAction.undoSeccond_ctrl.bottleColors = nextAction.undoSeccond_btl.bottleColors;
        nextAction.undoSeccond_ctrl.numberOfColorsInBottle = nextAction.undoSeccond_btl.numberOfColorsInBottle;
        nextAction.undoSeccond_ctrl.setBottlemaskr();
        nextAction.undoSeccond_ctrl.UpadateTopColorValues();
        nextAction.undoSeccond_ctrl.UpdateColorsOnShader();


        // check if bottles are already full or not
        if (nextAction.undoSeccond_ctrl.doesBottle_full == true)
        {
            nextAction.undoSeccond_ctrl.undoCompletBottler();
        }

        if (nextAction.undoFirst_ctrl.doesBottle_full == true)
        {
            nextAction.undoFirst_ctrl.undoCompletBottler();
        }



        /* Debug.Log(" the current movment data ");
         Debug.Log(" the first bottle colors number " + nextAction.undoFirst_btl.numberOfColorsInBottle);
         Debug.Log(" the secoond bottle colors number " + nextAction.undoSeccond_btl.numberOfColorsInBottle);*/


        undoElements.RemoveAt(nextAction.moveNumber);



    }


    public bool doesUndoAvailibal()
    {
        if (undoElements.Count > 0)
        {
            return true;
        }

        else
        {
            return false;
        }
    }


}
