using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*@ objectToActivate->need to assign gameObject in gameEngine, now it ise set a treasure box
  @ questToCheck -> need to assign in gameEngine,
  @ activeIfComplete -> indicate the check box is true or false
  @ initialCheckDone -> used for the check box checked at the begining(basically, it is only false at first)
  */
public class QuestObjectActivater : MonoBehaviour
{

    public GameObject objectToActivate;
    public string questToCheck;
    public bool activeIfComplete;

    private bool initialCheckDone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*DO CheckCompletion() first*/
    void Update()
    {
        if (!initialCheckDone)
        {
            
            initialCheckDone = true;
            CheckCompletion();
            
        }
    }

    /*if the questTocheck(string) is already true, 
     * then set gameObject active or inactive based on 
     activeIfComplete(bool)*/
    public void CheckCompletion()
    {
        if (QuestManager.instance.CheckIfComplete(questToCheck))
        {
            objectToActivate.SetActive(activeIfComplete);

        }
    }
}
