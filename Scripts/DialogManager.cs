using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class DialogManager : MonoBehaviour
{

    public Text dialogText;
    public Text charName;
    public GameObject dialogBox;
    public GameObject nameBox;
    public string[] dialogLines;
    public int currentLine;
    public static DialogManager instance;

    private bool JustStarted;

    private string questToMark;
    private bool markQuestComplete;
    private bool shouldMarkQuest;
    public bool conversationEnded;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        conversationEnded = true;
    }

    // Update is called once per frame
    void Update()
    {

       
        if (dialogBox.activeInHierarchy) {

            //if it is not JustStarted, then  get fire1 
            if (!JustStarted) {
                if (Input.GetButtonUp("Fire1"))
                {
                    
                    currentLine++;
                    conversationEnded = false;
                    //if line is over, set inactive dialog box and dialogBox(bool) is false
                    if (currentLine > dialogLines.Length-1)
                    {
                        
                        dialogBox.SetActive(false);

                        GameManager.instance.dialogActive = false;


                        //if shouldMarkQuest is true, then it become false
                        if (shouldMarkQuest)
                        {
                            shouldMarkQuest = false;
                            //if markQuestComplete is true,call markQuestComplete()(fun)
                            if (markQuestComplete)
                            {
                                QuestManager.instance.MarkQuestComplete(questToMark);
                            }
                            else
                            {
                                QuestManager.instance.MarkQuestIncomplete(questToMark);
                            }
                        }
                        conversationEnded = true;
                    }
                    
                    //until it gets last line, show dialog 
                    else
                    {
                       
                        CheckIfName();
                        dialogText.text = dialogLines[currentLine];
                    }
                }
            }

            else
            {
                
                JustStarted = false;
            }
        
        }
    }

    /*open the first convo*/
    public void showDialog(string[] newLines,bool isPerson) {
        
        dialogLines = newLines;
        currentLine = 0;
      
        CheckIfName();
        dialogText.text = dialogLines[currentLine];
        dialogBox.SetActive(true);
        JustStarted = true;
        nameBox.SetActive(isPerson);
        GameManager.instance.dialogActive = true;
       


    }


    /*check the current line is char name*/
    public void CheckIfName()
    {
        if (dialogLines[currentLine].StartsWith("n-"))
        {
            charName.text = dialogLines[currentLine].Replace("n-","");
            currentLine++;
        }

    }

    public void ShouldActiveQuestAtEnd(string questName, bool markComplete)
    {
        questToMark = questName;
        markQuestComplete = markComplete;
        shouldMarkQuest = true;

    }

   
}
