using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadArea: MonoBehaviour
{
    //will change this to arr 
    public string areaToLoad;
    public string areaTransitionName;

    public AreaEntrance theEntrance;
    // Start is called before the first frame update

    public float waitToLoad = 1f;
    private bool shouldLoadAfterFade;

    public static LoadArea instance; 
    void Start()
    {
      
        theEntrance.transitionName = areaTransitionName;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(shouldLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;
            if (waitToLoad <= 0)
            {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(areaToLoad);

                
            }
            
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // SceneManager.LoadScene(areaToLoad);
            shouldLoadAfterFade = true;
           // GameManager.instance.fadingBetweenAreas = true;
            UIFade.instance.FadeToBlack();
            
            ManageAreaTransition();
        
        }
    }

    /*connect areaTransitionName btw player.cs and loadarea.cs*/
    private void ManageAreaTransition()
    {
        
        Player.instance.areaTransitionName = areaTransitionName;
    }
}
