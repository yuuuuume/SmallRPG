using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 when get into the next  scean, it fades to black
black a= 1
the UIFade instance is used in LoadArea.cs & AreaEntrance.cs

*/
public class UIFade : MonoBehaviour
{
    public static UIFade instance;
    public Image fadeScreen;
    public float fadeSpeed = 1;

    public bool shouldFadeToBlack;
    public bool shouldFadeFromBlack;

    private float toOne = 1f;
    private float toZero = 0f;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            //if another instance already there, destroy myself
            if (instance != this)
            {
                //destroy the new onject
                Destroy(this);
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r,
                                         fadeScreen.color.g,
                                         fadeScreen.color.b,
                                         Mathf.MoveTowards(fadeScreen.color.a,
                                                            toOne, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == toOne)
            {
                shouldFadeToBlack = false;
            }
        }
        if (shouldFadeFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r,
                                         fadeScreen.color.g,
                                         fadeScreen.color.b,
                                         Mathf.MoveTowards(fadeScreen.color.a,
                                                            toZero, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == toZero)
            {
                shouldFadeFromBlack = false;
            }
        }

    }

    public void FadeToBlack()
    {
        
            shouldFadeToBlack = true;
            shouldFadeFromBlack = false;
        
    }

    public void FadeFromBlack()
    {
        shouldFadeToBlack = false;
        shouldFadeFromBlack =true;

    }
}
