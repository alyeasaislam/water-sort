using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour
{



    public Color[] bottleColors;
    public SpriteRenderer bottleMaskSR;

    public AnimationCurve ScaleAndRotationMultiplierCureve;
    public AnimationCurve FillAmountCurve;
    public AnimationCurve RotationSpeedMultiplaier;

    public GameObject bottleFull_Effects;


    public AudioClip pouringSound;
    public AudioSource audioSource;

    //3
    public float[] fillamounts;
    public float[] rotationValues;

    private int rotationIndex = 0;    // how much the bottle should rotate to ransform all bottle liquide

    [Range(0, 4)]
    public int numberOfColorsInBottle = 4;

    public Color topColor;
    [HideInInspector] public int numberOfTopColorLayers = 1;

    // butto to filed with color ref
    [HideInInspector] public BottleController bottleControllerRef;
    private int numberOfColorsToTransfer = 0;


    private Animator bottle_Animator;
    // public Animation bottle_animation;

    // rotation logic
    public Transform leftRotationPoint;
    public Transform rightRotationPoint;

    private Transform choosenRotationPoint;
    private float directionMultiplier = 1.0f;


    // to move to the other bottle 
    Vector3 startposition;
    Vector3 originalPosition;
    Vector3 endPosition;

    public LineRenderer lineRenderer;

    public BoxCollider2D m_boxColider;
    //public bool isfulled_up;

    // Start is called before the first frame update
    void Start()
    {
        // m_boxColider = this.GetComponent<BoxCollider2D>();
        originalPosition = transform.position;

        bottle_Animator = gameObject.GetComponent<Animator>();


        bottleMaskSR.material.SetFloat("_FillAmount", fillamounts[numberOfColorsInBottle]);

        UpdateColorsOnShader();

        UpadateTopColorValues();

        setupBottleTransferSpeed();
    }


    // this is what's new 
    public float bottlemovmentSpeed = 2;
    public float bottleRotateSpeed = 1;


    public void setupBottleTransferSpeed()
    {
        float speedValue = PlayerPrefs.GetFloat("game_bottle_speed_transfer", 1);
        bottlemovmentSpeed = 2 * speedValue;
        bottleRotateSpeed = 1 * speedValue;
    }


    public void setBottlemaskr()
    {
        bottleMaskSR.material.SetFloat("_FillAmount", fillamounts[numberOfColorsInBottle]);

    }

    // Update is called once per frame
    void Update()
    {
    }
    public void UpdateColorsOnShader()
    {

        bottleMaskSR.material.SetColor("_C1", bottleColors[0]);
        bottleMaskSR.material.SetColor("_C2", bottleColors[1]);
        bottleMaskSR.material.SetColor("_C3", bottleColors[2]);
        bottleMaskSR.material.SetColor("_C4", bottleColors[3]);


    }

    public void startColorTransfer()
    {
        ChooseRoatationAndDirection();
        numberOfColorsToTransfer = Mathf.Min(numberOfTopColorLayers, 4 - bottleControllerRef.numberOfColorsInBottle);

        for (int i = 0; i < numberOfColorsToTransfer; i++)
        {
            bottleControllerRef.bottleColors[bottleControllerRef.numberOfColorsInBottle + i] = topColor;
        }
        bottleControllerRef.UpdateColorsOnShader();

        CalculateRotationIndex(4 - bottleControllerRef.numberOfColorsInBottle);

        transform.GetComponent<SpriteRenderer>().sortingOrder += 2;
        bottleMaskSR.sortingOrder += 2;

        StartCoroutine(MoveBottle());

    }
    public void playNot_mus_anime()
    {
        //bottle_Animator.SetBool("not_much", true);
        bottle_Animator.SetBool("not_the_same", true);
        // bottle_Animator.Play("Base Layer.bottle_animation",0);
        //  Invoke("desable_anime",1.0f);
    }

    private void LateUpdate()
    {
        bottle_Animator.SetBool("not_the_same", false);

    }


    public void bottleSelected()
    {
        transform.position = Vector2.Lerp(transform.position, transform.position + Vector3.up * 5, Time.deltaTime);
        // maybe add some effects here
    }
    public void bottleInselected()
    {
        transform.position = Vector2.Lerp(transform.position, transform.position + Vector3.down * 5, Time.deltaTime);
        // delet those effects 
    }
    public void playpouringSound()
    {
        audioSource.clip = pouringSound;
        audioSource.Play();
    }

    public void StopPouringSound()
    {

        audioSource.Stop();
    }


    IEnumerator MoveBottle()
    {

        startposition = transform.position;
        if (choosenRotationPoint = leftRotationPoint)
        {
            endPosition = bottleControllerRef.rightRotationPoint.position;

        }
        else
        {
            endPosition = bottleControllerRef.leftRotationPoint.position;
        }

        float t = 0;
        while (t <= 1)
        {
            transform.position = Vector3.Lerp(startposition, endPosition, t);
            t += Time.deltaTime * bottlemovmentSpeed;  // the old one 2

            yield return new WaitForEndOfFrame();

        }

        transform.position = endPosition;
        StartCoroutine(RotateBottle());
    }
    IEnumerator MoveBottleBack()
    {


        startposition = transform.position;
        endPosition = originalPosition;
        float t = 0;
        while (t <= 1)
        {
            transform.position = Vector3.Lerp(startposition, endPosition, t);
            t += Time.deltaTime * bottlemovmentSpeed;  // the old one 2

            yield return new WaitForEndOfFrame();

        }

        transform.position = endPosition;

        //tst
        //bottleInselected();
        //m_boxColider.enabled = true;

        transform.GetComponent<SpriteRenderer>().sortingOrder -= 2;
        bottleMaskSR.sortingOrder -= 2;
    }


    public float timeToTotate = 1;

    IEnumerator RotateBottle()
    {

        float t = 0;
        float lerpValue;
        float angleValue;

        float lastAngleValue = 0;

        while (t < timeToTotate)
        {
            // isfulled_up=true;
            GameController.instance.setTheAbilityTrans(false);

            lerpValue = t / timeToTotate;
            angleValue = Mathf.Lerp(0.0f, directionMultiplier * rotationValues[rotationIndex], lerpValue);

            //  transform.eulerAngles = new Vector3(0, 0, angleValue);

            transform.RotateAround(choosenRotationPoint.position, Vector3.forward, lastAngleValue - angleValue);

            bottleMaskSR.material.SetFloat("_SARM", ScaleAndRotationMultiplierCureve.Evaluate(angleValue));

            if (fillamounts[numberOfColorsInBottle] > FillAmountCurve.Evaluate(angleValue) + 0.005f)
            {


                if (lineRenderer.enabled == false)
                {

                    topColor.a = 200;
                    lineRenderer.startColor = topColor;
                    lineRenderer.endColor = topColor;

                    lineRenderer.SetPosition(0, choosenRotationPoint.position);
                    lineRenderer.SetPosition(1, choosenRotationPoint.position - Vector3.up * 1.45f);


                    bottleControllerRef.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
                    bottleControllerRef.GetComponent<SpriteRenderer>().sortingOrder = 3;
                    lineRenderer.enabled = true;

                    //play pouring sound
                    playpouringSound();

                }
                bottleMaskSR.material.SetFloat("_FillAmount", FillAmountCurve.Evaluate(angleValue));

                // m_boxColider.enabled = true;

                bottleControllerRef.FillUp(FillAmountCurve.Evaluate(lastAngleValue) - FillAmountCurve.Evaluate(angleValue));
            }




            t += Time.deltaTime * RotationSpeedMultiplaier.Evaluate(angleValue) * bottleRotateSpeed;   // the old one nothing
            lastAngleValue = angleValue;
            yield return new WaitForEndOfFrame();
        }

        angleValue = directionMultiplier * rotationValues[rotationIndex];
        //transform.eulerAngles = new Vector3(0, 0, angleValue);
        bottleMaskSR.material.SetFloat("_SARM", ScaleAndRotationMultiplierCureve.Evaluate(angleValue));
        bottleMaskSR.material.SetFloat("_FillAmount", FillAmountCurve.Evaluate(angleValue));

        numberOfColorsInBottle -= numberOfColorsToTransfer;
        bottleControllerRef.numberOfColorsInBottle += numberOfColorsToTransfer;

        bottleControllerRef.is_ref_full();

        //after filling all the bottle now desable line rendrer 

        ///****************///////
        StopPouringSound();
        GameController.instance.setTheAbilityTrans(true);
        // isfulled_up=false;
        lineRenderer.enabled = false;
        bottleControllerRef.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 0;
        bottleControllerRef.GetComponent<SpriteRenderer>().sortingOrder = 1;



        StartCoroutine(RotateBottleBack());

    }

    void is_ref_full()
    {

        if (testMuchColors() == true && t_1 == true && numberOfColorsInBottle == 4)
        {
            t_1 = false;
            doesBottle_full = true;
            // instentate the vfx
            fullVFX = Instantiate(bottleFull_Effects, transform.position, Quaternion.identity);
            //bottleControllerRef.m_boxColider.enabled=false;
            //m_boxColider.enabled = true;

            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            LevleGeneartor.instance.full_abottle();
        }


    }
    public bool doesBottle_full = false;
    GameObject fullVFX;
    public void undoCompletBottler()
    {

        Destroy(fullVFX);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        LevleGeneartor.instance.UndoPlyaerFullA_Bottle();
        doesBottle_full = false;
        t_1 = true;


    }

    IEnumerator RotateBottleBack()
    {

        float t = 0;
        float lerpValue;
        float angleValue;

        float lastAngleValue = directionMultiplier * rotationValues[rotationIndex];



        while (t < timeToTotate)
        {

            lerpValue = t / timeToTotate;
            angleValue = Mathf.Lerp(directionMultiplier * rotationValues[rotationIndex], 0.0f, lerpValue);

            //transform.eulerAngles = new Vector3(0, 0, angleValue);

            transform.RotateAround(choosenRotationPoint.position, Vector3.forward, lastAngleValue - angleValue);

            bottleMaskSR.material.SetFloat("_SARM", ScaleAndRotationMultiplierCureve.Evaluate(angleValue));

            lastAngleValue = angleValue;

            t += Time.deltaTime * bottleRotateSpeed;   // the old one nothing *1

            yield return new WaitForEndOfFrame();
        }
        UpadateTopColorValues();
        angleValue = 0f;
        transform.eulerAngles = new Vector3(0, 0, angleValue);
        bottleMaskSR.material.SetFloat("_SARM", ScaleAndRotationMultiplierCureve.Evaluate(angleValue));


        StartCoroutine(MoveBottleBack());

    }


    public void UpadateTopColorValues()
    {


        if (numberOfColorsInBottle != 0)
        {
            numberOfTopColorLayers = 1;
            topColor = bottleColors[numberOfColorsInBottle - 1];

            if (numberOfColorsInBottle == 4)
            {

                if (bottleColors[3].Equals(bottleColors[2]))
                {
                    numberOfTopColorLayers = 2;

                    if (bottleColors[2].Equals(bottleColors[1]))
                    {
                        numberOfTopColorLayers = 3;
                        if (bottleColors[1].Equals(bottleColors[0]))
                        {
                            numberOfColorsInBottle = 4;
                            // here we should say that the bottle is ful of the same lquid 
                            Debug.Log("it's ful form the update top colors function ");

                        }

                    }


                }


            }
            else if (numberOfColorsInBottle == 3)
            {
                if (bottleColors[2].Equals(bottleColors[1]))
                {
                    numberOfTopColorLayers = 2;
                    if (bottleColors[1].Equals(bottleColors[0]))
                    {
                        numberOfTopColorLayers = 3;
                    }
                }
            }


            else if (numberOfColorsInBottle == 2)
            {
                if (bottleColors[1].Equals(bottleColors[0]))
                {
                    numberOfTopColorLayers = 2;
                }
            }

            rotationIndex = 3 - (numberOfColorsInBottle - numberOfTopColorLayers);

        }




    }


    public bool FillbottleCheck(Color colorToCheck)
    {


        if (numberOfColorsInBottle == 0)
        {
            return true;

        }
        else
        {
            if (numberOfColorsInBottle == 4)
            {
                return false;
                //maybe add animation to the bottle just to say that are not much tranfer impossible
            }
            else
            {
                if (topColor.Equals(colorToCheck))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }


        }
    }




    private void CalculateRotationIndex(int numberOfEmptySpaceInSecondBottle)
    {

        rotationIndex = 3 - (numberOfColorsInBottle - Mathf.Min(numberOfEmptySpaceInSecondBottle, numberOfTopColorLayers));


    }



    bool t_1 = true;

    private void FillUp(float fillAmountToAdd)
    {
        bottleMaskSR.material.SetFloat("_FillAmount", bottleMaskSR.material.GetFloat("_FillAmount") + fillAmountToAdd);

    }
    private bool testMuchColors()
    {
        if (bottleColors[0] == bottleColors[1] && bottleColors[1] == bottleColors[2] && bottleColors[2] == bottleColors[3])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ChooseRoatationAndDirection()
    {

        if (transform.position.x > bottleControllerRef.transform.position.x)
        {
            choosenRotationPoint = leftRotationPoint;
            directionMultiplier = -1.0f;
        }
        else
        {
            choosenRotationPoint = rightRotationPoint;
            directionMultiplier = -1.0f;
        }



    }


}

