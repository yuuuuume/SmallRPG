using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*control char feature. 
 also the system of levelling up exp and level
 */
public class CharStat : MonoBehaviour
{

    public string charName;
    public int playerLevel = 1;
    public int currentEXP;
    
    public int[] expToNextLevel;
    public int maxLevel = 10;
    public int baseExp = 1000;

    public int[] mpBonus;
    public int currentHP;
    public int maxHP = 100;
    public int currentMP;
    public int maxMP=30;
    public int strength;
    public int defence;
    public int wpPwr;
    public int armrPwr;
    public string equippedWp;
    public string equippedArmr;
    public Sprite charImage;
    public string charTalk;
    // Start is called before the first frame update
    void Start()
    {
        /*config the expo to go next level*/
        expToNextLevel = new int[maxLevel];
        for(int i = 1; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(baseExp * 1.005f + expToNextLevel[i-1]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddExp(1000);
        }
    }

    public void AddExp(int expToadd)
    {
        if (playerLevel <maxLevel)
        {
            currentEXP += expToadd;
            if (currentEXP > expToNextLevel[playerLevel]) {
           
                    currentEXP -= expToNextLevel[playerLevel];
                    playerLevel++;

                    //determin wheather add strength or defence based on odd or even
                    if (playerLevel % 2 == 0)
                    {
                        strength++;
                    }
                    else
                    {

                        defence++;
                    }

                    maxHP = Mathf.FloorToInt(1.05f * maxHP);
                    currentHP = Mathf.FloorToInt(1.05f * currentHP);

                    maxMP += mpBonus[playerLevel];
                    currentMP += mpBonus[playerLevel];
            }
        }
        if(playerLevel >= maxLevel)
        {
            currentEXP = 0;
        }
    }
}
