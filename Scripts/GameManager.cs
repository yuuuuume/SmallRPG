using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public bool gameMenuOpen, dialogActive, fadingBetweenAreas,shopActive,battleActive;
    

    public  CharStat[] playerStats;

    public string[] itemHeld;
    public int[] numOfItems;
    public Items[] referenceItem;

    public int currentGold;
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
                Destroy(this.gameObject);
            }
        }
        
        sortItems();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //problem is.... gameMenuOpen is never true

        
        if (gameMenuOpen|| dialogActive|| fadingBetweenAreas||shopActive|| battleActive)
        {

            
            Player.instance.canMove = false;
            
        }
        else
        {
           

            Player.instance.canMove = true;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            
            SaveData();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadData();
        }

    }

    //return item
    
    public Items getItemDetail(string itemToGrab)
    {
        for(int i= 0; i < referenceItem.Length; i++)
        {
            if(referenceItem[i].itemName == itemToGrab)
            {
                return referenceItem[i];
            }
        }

        return null;
    }

    /*sort items on item menu*/
    public void sortItems()
    {
        bool itemAfterSpace = true;
        while (itemAfterSpace)
        {
            itemAfterSpace = false;
            for (int i = 0; i < itemHeld.Length - 1; i++)
            {
                if (itemHeld[i] == "")
                {
                    itemHeld[i] = itemHeld[i + 1];
                    itemHeld[i + 1] = "";

                    numOfItems[i] = numOfItems[i + 1];
                    numOfItems[i + 1] = 0;
                    if (itemHeld[i] != "")
                    {
                        itemAfterSpace = true;//if the current item does exist, continue while loop
                    }
                }
                
            }

            
        }
    }

    public void AddItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;

        for(int i = 0; i < itemHeld.Length; i++)
        {
            if(itemHeld[i] == "" || itemHeld[i] == itemToAdd)
            {
                newItemPosition = i;
                i = itemHeld.Length;
                foundSpace = true;
            }
        }
        if (foundSpace)
        {
            bool itemExists = false;
            for(int i = 0; i< referenceItem.Length; i++)
            {
                if(referenceItem[i].itemName == itemToAdd)
                {
                    itemExists = true;
                    break;
                }
            }
            if (itemExists)
            {
                itemHeld[newItemPosition] = itemToAdd;
                numOfItems[newItemPosition]++;
            }
            else
            {
                Debug.LogError(itemToAdd + "Does not exist");
            }

        }
        //need to update the item menu
        GameMenu.instance.showItems();
        sortItems();
    }

    public void removeItem(string itemToremove)
    {
        bool foundItem = false;
        int itemPosition = 0;
        for(int i= 0; i< itemHeld.Length; i++)
        {
            if(itemHeld[i] == itemToremove)
            {
                foundItem = true;
                itemPosition = i;
                //stop the loop
                break;

            }

        }
        if (foundItem)
        {
            numOfItems[itemPosition]--;
            if (numOfItems[itemPosition] <= 0){
                itemHeld[itemPosition] = "";
            }
        }

        
        else
        {
            Debug.LogError("coud not remove that item");
        }
        GameMenu.instance.showItems();
       
    }
    
    public void SaveData()
    {
        //save scene and player position
        PlayerPrefs.SetString("Current_Scene" , SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("Player_Position_x", Player.instance.transform.position.x);
        
        PlayerPrefs.SetFloat("Player_Position_y", Player.instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_z", Player.instance.transform.position.z);

        //save char info
        for(int i = 0; i < playerStats.Length; i++)
        {
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "active", 1);
            }
            else {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "active", 0);
            }
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_level", playerStats[i].playerLevel);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_currentExp", playerStats[i].currentEXP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_maxHP", playerStats[i].maxHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_maxMP", playerStats[i].maxMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_currentHP", playerStats[i].currentHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_currentMP", playerStats[i].currentMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_strength", playerStats[i].strength);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_defence", playerStats[i].defence);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_equippedWp", playerStats[i].equippedWp);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_equippedArmr", playerStats[i].equippedArmr);

        }

        //store inventory data
        for(int i = 0; i < itemHeld.Length; i++)
        {
            PlayerPrefs.SetString("ItemInInventory_atPosition_" + i, itemHeld[i]);
            PlayerPrefs.SetInt("ItemAmount_atPosition_" + i, numOfItems[i]);

        }

    }

    public void LoadData()
    {

        /*
        Player.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_x")
            ,PlayerPrefs.GetFloat("Player_Position_y")
            ,PlayerPrefs.GetFloat("Player_Position_z"));
        SceneManager.LoadScene(PlayerPrefs.GetString("Current_Scene"));
        */
        //Debug.Log("the next is "  + PlayerPrefs.GetFloat("Player_Position_x"));
        Player.instance.areaTransitionName = "continue";
        for (int i = 0; i < playerStats.Length; i++)
        {
            if(PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "active") == 0)
            {
                playerStats[i].gameObject.SetActive(false);
            }
            else
            {
                playerStats[i].gameObject.SetActive(true);
            }

            playerStats[i].playerLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_level");
            playerStats[i].currentEXP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_currentExp");
            playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_maxHP");
            playerStats[i].maxMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_maxMP");
            playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_currentHP");
            playerStats[i].currentMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_currentMP" );
            playerStats[i].strength = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_strength");
            playerStats[i].defence = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_defence");
            playerStats[i].equippedWp = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_equippedWp" );
            playerStats[i].equippedArmr = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_equippedArmr" );

        }

        //load inventory data
        for (int i = 0; i < itemHeld.Length; i++)
        {
            itemHeld[i]=  PlayerPrefs.GetString("ItemInInventory_atPosition_" + i);
            numOfItems[i]= PlayerPrefs.GetInt("ItemAmount_atPosition_" + i);

        }
    }
}
