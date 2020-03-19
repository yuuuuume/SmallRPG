using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/*manage menu
 @show stasu menu--> select each char name--> show each char stats
 @show item menu --> show all items player has--> can use or delete items
 */
public class GameMenu : MonoBehaviour
{

    public GameObject theMenu;
    public GameObject[] panels;

    private CharStat[] playerStats;

    public Text[] nameText , hpText, mpText, lvText, expText;
    public Slider[] expSlider;
    public Image[] charImg;
    public GameObject[] charStatHolder;

    public GameObject[] statusBottun;
    public Text statusName, statusMp, statusHp, statusStr, statusDef, statusWpnPw, statusWpnEq, statusArmEq, statusArm, statusExp,statusTalk;
    public Image statusImage;

    public ItemButton[] itemButtons;
    public string selectedItem;
    public Items activeItem;
    public Text itemName, itemDescription, useButtonText;

    public static GameMenu instance;
    public string titleName;

    public GameObject itemCharChoiceMenu;
    public Text[] charNamesToUseItem;

    public Text goldText;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Fire2"))
        {
            if (theMenu.activeInHierarchy)
            {
                /*
                theMenu.SetActive(false);
                GameManager.instance.gameMenuOpen = false;
                */
                ClosePanel();
                //insert sound effect
                
            }
            else
            {
                theMenu.SetActive(true);
                upDateMainStats();
                GameManager.instance.gameMenuOpen = true;
               
            }
        }
    }


   

    public void upDateMainStats()
    {
        playerStats = GameManager.instance.playerStats;
        for(int i = 0; i < playerStats.Length; i++)
        {
            
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                
               charStatHolder[i].SetActive(true);
               
               nameText[i].text = playerStats[i].charName;
               hpText[i].text = "HP: " + playerStats[i].currentHP + " / " + playerStats[i].maxHP; ;
               mpText[i].text = "MP: " + playerStats[i].currentMP + " / " + playerStats[i].maxMP;
               lvText[i].text = "Lv." + playerStats[i].playerLevel + " / " + playerStats[i].maxLevel; ;
               expText[i].text = "EXP to Next Level: " + (playerStats[i].expToNextLevel[playerStats[i].playerLevel] - playerStats[i].currentEXP).ToString();
               expSlider[i].maxValue = playerStats[i].expToNextLevel[playerStats[i].playerLevel];
               expSlider[i].value = playerStats[i].currentEXP;
               charImg[i].sprite = playerStats[i].charImage;
            }
            else
            {
                
                charStatHolder[i].SetActive(false);

            }
        }
      //  goldText.text = GameManager.instance.currentGoald.ToString() + " $";
    }

    //invoke each panel in array
    public void togglePanel(int panelIndex) {
        upDateMainStats();
        for (int i = 0; i < panels.Length; i++)
        {
            
            if (i == panelIndex)
            {
                
                //we want the opposite result.
                panels[i].SetActive(!panels[i].activeInHierarchy);
            }
            else
            {
                //dont need both
                panels[i].SetActive(false);
            }
        }
        itemCharChoiceMenu.SetActive(false);
    }

    /*panel is closed when we click close or left alt*/
    public void ClosePanel()
    {
       
        for(int i = 0; i< panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        theMenu.SetActive(false);
        GameManager.instance.gameMenuOpen = false;

        itemCharChoiceMenu.SetActive(false);
        AudioManager.instance.PlaySFX(5);
    }

    /*when we just open status, char[0] is gonnabe open in defult*/
    public void OpenStatus()
    {
        upDateMainStats();
        statsChar(0);
        //update info in shown
        for (int i = 0; i < statusBottun.Length; i++)
        {
            statusBottun[i].SetActive(playerStats[i].gameObject.activeInHierarchy);
            statusBottun[i].GetComponentInChildren<Text>().text = playerStats[i].charName;
        }
    }

    /*about the selected characters detail*/
    public void statsChar(int selected)
    {
        statusName.text = playerStats[selected].charName;
        statusHp.text = (playerStats[selected].currentHP).ToString();
        statusMp.text = playerStats[selected].currentMP.ToString();
        statusStr.text = playerStats[selected].strength.ToString();
        statusDef.text = playerStats[selected].defence.ToString();
        if (playerStats[selected].equippedWp != "")
        {
            statusWpnEq.text = playerStats[selected].equippedWp;
        }
        else
        {
            statusWpnEq.text = "Justice";
        }
        statusWpnPw.text = playerStats[selected].wpPwr.ToString();
        if(playerStats[selected].equippedArmr != "")
        {
            statusArmEq.text = playerStats[selected].equippedArmr;
        }
        else
        {
            statusArmEq.text = "Muscle";
        }
        statusArm.text = playerStats[selected].armrPwr.ToString();
        statusExp.text = playerStats[selected].currentEXP.ToString();
        statusImage.sprite = playerStats[selected].charImage;
        statusTalk.text = playerStats[selected].charTalk;
    }

    /*assign each itembutton value. show Items with image, number of each items*/
    public void showItems()
    {
        GameManager.instance.sortItems();
        for (int i = 0; i < itemButtons.Length; i++)
        {
            //get index of each buttons
            itemButtons[i].buttonValue = i;
            
            
            if(GameManager.instance.itemHeld[i] != "")
            {
                
                //guess buttonImage is not a gameobject it self. in this casse, we want assingned gameobject
                itemButtons[i].buttonImage.gameObject.SetActive(true);

                itemButtons[i].buttonImage.sprite = GameManager.instance.getItemDetail(GameManager.instance.itemHeld[i]).itemImage;
                itemButtons[i].amountText.text = GameManager.instance.numOfItems[i].ToString();
            }
            else
            {
                
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
            
        }

    }

    public void selectItem(Items newItem)
    {
        activeItem = newItem;
        if (activeItem.isItem)
        {
            useButtonText.text = "Use";
        }

        if (activeItem.isArmor || activeItem.isWeapon)
        {
            useButtonText.text = "Equip";
        }

        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.description;

    }

    public void DiscardItem()
    {
        if(activeItem != null)
        {
            GameManager.instance.removeItem(activeItem.itemName);
        }
    }

    public void OpenCharChoiceToUseItem()
    {
        itemCharChoiceMenu.SetActive(true);
        for(int i = 0; i < charNamesToUseItem.Length; i++)
        {
            charNamesToUseItem[i].text = GameManager.instance.playerStats[i].charName;
            charNamesToUseItem[i].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[i].gameObject.activeInHierarchy);
            
        }

    }

    public void CloseCharChoiceToUseItem()
    {
        itemCharChoiceMenu.SetActive(false);
    }

    public void UseItem(int selectChar)
    {
        activeItem.Use(selectChar);
        CloseCharChoiceToUseItem();
    }


    //save char info,inventory,quests
    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
    }

    public void ButtonSFX()
    {
        AudioManager.instance.PlaySFX(4);
    }

    /*Destroy all gameobject*/
    public void GoTitle()
    {

        SceneManager.LoadScene(titleName);
        Destroy(GameManager.instance.gameObject);
        Destroy(Player.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(gameObject);
    }
}
