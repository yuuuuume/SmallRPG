using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/*
 @battleActive: boolean, if battle is active or not
 @battleScene: Battle BackGround
 @playerPosition: transform array, players transform position
 @enemyPosition: transform arry, enemiess transform position
 @activeBattlers: List, all active battler including enemy and player
 @currentTurn: int, index of who is attaking now
 @turnWaiting: boolean, waiting player turn
 @uiButtonHolder: gameObject, menu bar
 @moveList : BattleMove[] ,[System.Serializable] class including @moveName@movePower@moveCost@theEffect
 @enemyAttackEffect : gameobject, when enemy attack, it shows up on the target position
 @theDamageNumber: class


 */

public class BattleManager : MonoBehaviour
{

    public static BattleManager instance;

    private bool battleActive;
    public GameObject battleScene;
    public Transform[] playerPosition;
    public Transform[] enemyPosition;

    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;

    public List<BattleChar> activeBattlers = new List<BattleChar>();

    public int currentTurn;
    public bool turnWaiting;

    public GameObject uiButtonHolder;
  
    public BattleMove[] moveList;
    public GameObject enemyAttackEffect;
    public DamageNumber theDamageNumber;

    public Text[] playerNames, playerHP, playerMP;

    public GameObject TargetMenu;
    
    public BattleTargetButton[] targetButtons;
    public GameObject magicMenue;
    public BattleMagicSelect[] magicButtons;
    public GameObject closeMagicSlelectMenu;
    public BattleNotification battleNotice;
    public int chanceToFlee = 35;

    private bool flee;
    public bool isBoss;
    
    public GameObject ItemMenu;
    public Items selectedItem;
    public Text useItem, useItemDescription, useButton;
    public GameObject itemCharChoiceMenu;
    public Text[] charNamesToUseItem;
    public ItemButton[] useItemButtons;

    public string gameOverScene;

    public int rewardEp;
    public string[] rewardItems;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            BattleStart(new string[] { "Dragon" }, false);
        }

        if (battleActive)
        {
            if (turnWaiting)
            {
                if (activeBattlers[currentTurn].isPlayer)
                {
                    uiButtonHolder.SetActive(true);
                }
                else
                {
                    uiButtonHolder.SetActive(false);

                    //enemy should  attack
                    StartCoroutine(EnemyMoveCo());
                    UpdateBattle();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
           
            nextTurn();

        }

    }

    public void BattleStart(string[] enemiesToSpawn, bool cannotFlee)
    {
        if (!battleActive)
        {
            isBoss = cannotFlee;
            battleActive = true;
            GameManager.instance.battleActive = true;

            //move battlescene as camera moves
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            battleScene.SetActive(true);

            AudioManager.instance.PlayBGM(0);

            for (int i = 0; i < playerPosition.Length; i++)
            {
                if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
                {
                    for (int j = 0; j < playerPrefabs.Length; j++)
                    {
                        //serch char
                        if (playerPrefabs[j].charName == GameManager.instance.playerStats[i].charName)
                        {
                            BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPosition[i].position, playerPosition[i].rotation);
                            newPlayer.transform.parent = playerPosition[i];
                            activeBattlers.Add(newPlayer);

                            CharStat thePlayer = GameManager.instance.playerStats[i];
                            activeBattlers[i].currentHP = thePlayer.currentHP;
                            activeBattlers[i].currentMP = thePlayer.currentMP;
                            activeBattlers[i].maxHP = thePlayer.maxHP;
                            activeBattlers[i].maxMP = thePlayer.maxMP;
                            activeBattlers[i].strength = thePlayer.strength;
                            activeBattlers[i].defence = thePlayer.defence;
                            activeBattlers[i].wpnPower = thePlayer.wpPwr;
                            activeBattlers[i].armrPower = thePlayer.armrPwr;

                        }
                    }
                }
            }

            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if (enemiesToSpawn[i] != "")
                {
                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if (enemyPrefabs[j].charName == enemiesToSpawn[i])
                        {
                            BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPosition[i].position, enemyPosition[i].rotation);
                            newEnemy.transform.parent = enemyPosition[i];
                            activeBattlers.Add(newEnemy);
                        }
                    }
                }
            }
            turnWaiting = true;
            currentTurn = 0;

            UpdateStats();
        }
    }

    //control currentTurn
    public void nextTurn()
    {
        currentTurn++;

        if (currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }
       
        turnWaiting = true;
        UpdateBattle();
        UpdateStats();


    }

    public void UpdateBattle()
    {
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].currentHP <= 0)
            {
                activeBattlers[i].currentHP = 0;
                //handle dead battler
               
                if (activeBattlers[i].isPlayer)
                {
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].deadSprite;
                }
                else
                {
                    
                    activeBattlers[i].EnemyFade();
                }
            }
            else
            {

                if (activeBattlers[i].isPlayer)
                {
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].aliveSprite;
                    allPlayersDead = false;
                }
                else
                {
                    allEnemiesDead = false;
                }
            }

        }
        if (allEnemiesDead || allPlayersDead)
        {
            if (allEnemiesDead)
            {
                //end battle in victory
                StartCoroutine(endBattleCo());
            }
            else
            {
                //game over
                StartCoroutine(GameOverCo());
            }
/*
            battleScene.SetActive(false);
            GameManager.instance.battleActive = false;
            battleActive = false;
  */
    }
        else
        {
            
            while(activeBattlers[currentTurn].currentHP == 0)
            {
                //skip dead battlers turn
                currentTurn++;
                if (currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;

                }
            }
        }
    }

    public IEnumerator EnemyMoveCo()
    {
        turnWaiting = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        nextTurn();
    }

    
    public void EnemyAttack()
    {
        List<int> players = new List<int>();
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            //search active player with alive
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {
                players.Add(i);
            }
        }
        int selectedTarget = players[Random.Range(0, players.Count)];

        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);
        int movePower = 0;
        for (int i = 0; i < moveList.Length; i++)
        {
            if (moveList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])
            {
                //give effect on scene
                Instantiate(moveList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = moveList[i].movePower;

            }
        }

        //show attacking player
        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
        DealDamage(selectedTarget, movePower);
    }

    //give damage
    public void DealDamage(int target, int movePower)
    {
        float attackPower = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].wpnPower;
        float defencePower = activeBattlers[target].defence + activeBattlers[target].armrPower;

        float damageCal = (attackPower / defencePower) * movePower * Random.Range(.9f, 1.1f);
        int damageToGive = Mathf.RoundToInt(damageCal);


        activeBattlers[target].currentHP -= damageToGive;
        var number = Instantiate(theDamageNumber);
        number.transform.position = activeBattlers[target].transform.position;
        number.transform.rotation = activeBattlers[target].transform.rotation;

        //show damage on scene
        number.setDamage(damageToGive);

        UpdateStats();
    }

    //Update status value;
    public void UpdateStats()
    {
        for(int i = 0; i < playerNames.Length; i++)
        {
            
            
            //only battle is active
            if (activeBattlers.Count > i)
            {
                
                if (activeBattlers[i].isPlayer)
                {
                   
                    BattleChar playerData = activeBattlers[i];
                    playerNames[i].gameObject.SetActive(true);
                    playerNames[i].text = playerData.charName;
                    playerHP[i].text = Mathf.Clamp(playerData.currentHP , 0, int.MaxValue) + "/" + playerData.maxHP;
                   
                    playerMP[i].text = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + "/" + playerData.maxMP;

                }
                else
                {
                    playerNames[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playerNames[i].gameObject.SetActive(false);
            }
            
        }
    }

    public void PlayerAttack(string moveName,int selectedTarget)
    {
        
        int movePower = 0;
        for (int i = 0; i < moveList.Length; i++)
        {
            if (moveList[i].moveName == moveName)
            {
                //give effect on scene
                Instantiate(moveList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
                movePower = moveList[i].movePower;

            }
        }
        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);

        DealDamage(selectedTarget, movePower);
       
        //uiButtonHolder.SetActive(false);
        TargetMenu.SetActive(false);
        nextTurn();

    }

    public void OpenTargetMenu(string moveName)
    {
        TargetMenu.SetActive(true);
        List<int> Enemies = new List<int>();
        
        //create enemies list
        for(int i = 0; i < activeBattlers.Count; i++)
        {
            if (!activeBattlers[i].isPlayer)
            {
                Enemies.Add(i);
            }
        }


        for(int i = 0; i < targetButtons.Length; i++)
        {
            
            if (Enemies.Count > i && activeBattlers[Enemies[i]].currentHP > 0)
            {
                targetButtons[i].gameObject.SetActive(true);
                targetButtons[i].moveName = moveName;
                targetButtons[i].activeBattlerTarget = Enemies[i];
                targetButtons[i].TargetName.text = activeBattlers[Enemies[i]].charName;

            }
            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenMagicMenu()
    {
        
        
        magicMenue.SetActive(true);

        for(int i = 0; i< magicButtons.Length; i++)
        {
            if(activeBattlers[currentTurn].movesAvailable.Length > i)
            {
                magicButtons[i].gameObject.SetActive(true);
                magicButtons[i].SpellName = activeBattlers[currentTurn].movesAvailable[i];
                magicButtons[i].nameText.text = magicButtons[i].SpellName;

                for(int j = 0; j< moveList.Length; j++)
                {
                    if(moveList[j].moveName == magicButtons[i].SpellName)
                    {
                        
                        magicButtons[i].spellCost = moveList[j].moveCost;
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();
                        
                    }
                    
                    
                }
            }
            else
            {
                magicButtons[i].gameObject.SetActive(false);
            }
        }

    }

    public void CloseMagicMenu()
    {

        magicMenue.SetActive(false);
        
        //close if player dont want to use magic
    }

    public void Flee()
    {
        if (isBoss)
        {
            nextTurn();
            battleNotice.theText.text = " you cannot flee this Battle";
            battleNotice.Activate();
            GameManager.instance.battleActive = false;
        }
        else
        {
            int fleeSuccess = Random.Range(0, 100);

            if (fleeSuccess < chanceToFlee)
            {
                /*
                battleActive = false;
                battleScene.SetActive(false);
                */
                flee = true;
                StartCoroutine(endBattleCo());

            }
            else
            {
                nextTurn();
                battleNotice.theText.text = "Could not escape!";
                battleNotice.Activate();
                GameManager.instance.battleActive = false;
            }
        }
        

    }

    
    //ここから・・・
    /*
   public void OpenItemMenu()
    {
        GameManager.instance.sortItems();
        ItemMenu.SetActive(true);
       
    }
    public void selectUseItem(Items gettingItem)
    {
        
        selectedItem = gettingItem;
        useItem.text = selectedItem.itemName;
        useItemDescription.text = selectedItem.description;
        for (int i = 0; i < useItemButtons.Length; i++)
        {
            //get index of each buttons
            useItemButtons[i].buttonValue = i;

            //Debug.Log(GameManager.instance.itemHeld[i]);
            if (GameManager.instance.itemHeld[i] != "")
            {

                //guess buttonImage is not a gameobject it self. in this casse, we want assingned gameobject
                useItemButtons[i].buttonImage.gameObject.SetActive(true);

                useItemButtons[i].buttonImage.sprite = GameManager.instance.getItemDetail(GameManager.instance.itemHeld[i]).itemImage;
                useItemButtons[i].amountText.text = GameManager.instance.numOfItems[i].ToString(); ;
            }
            else
            {

                useItemButtons[i].buttonImage.gameObject.SetActive(false);
                useItemButtons[i].amountText.text = "";
            }

        }

    }
    public void OpenCharChoiceToUseItem()
    {
        itemCharChoiceMenu.SetActive(true);
        for (int i = 0; i < charNamesToUseItem.Length; i++)
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
        selectedItem.Use(selectChar);
        CloseCharChoiceToUseItem();
    }

    public void CloseItemMenu()
    {
        ItemMenu.SetActive(false);
    }
    */
    //ここまで
    public IEnumerator endBattleCo()
    {
        battleActive = false;
        uiButtonHolder.SetActive(false);
        TargetMenu.SetActive(false);
        magicMenue.SetActive(false);

        yield return new WaitForSeconds(.5f);

        UIFade.instance.FadeToBlack();
        yield return new WaitForSeconds(1.0f);
        for(int i = 0; i< activeBattlers.Count; i++)
        {

            if (activeBattlers[i].isPlayer)
            {
                for(int j = 0; j < GameManager.instance.playerStats.Length; j++)
                {
                    if(activeBattlers[i].charName == GameManager.instance.playerStats[j].charName)
                    {

                        GameManager.instance.playerStats[i].currentHP = activeBattlers[i].currentHP;
                        GameManager.instance.playerStats[i].currentMP = activeBattlers[i].currentMP;
                       
                    }
                }
                //destroy instanciated battlers
                Destroy(activeBattlers[i].gameObject);
            }
        }
        UIFade.instance.FadeFromBlack();
        battleScene.SetActive(false);
        activeBattlers.Clear();
        currentTurn = 0;
        if (flee)
        {
            GameManager.instance.battleActive = false;
            flee = false;
        }
        else
        {
            //open reward screen
            BattleReward.instance.OpenRewardScreen(rewardEp, rewardItems);
        }
        

        AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);
    }

    public IEnumerator GameOverCo()
    {
        battleActive = false;
        UIFade.instance.FadeToBlack();

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(gameOverScene);
    }
}
