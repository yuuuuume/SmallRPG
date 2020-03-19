using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 give items feature here . can add specific value in unity
 */
public class Items : MonoBehaviour
{
    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    public bool isArmor;
    [Header("Item Details")]
    public string itemName;
    public string description;
    public int value;
    public Sprite itemImage;
    public int amountOfChange;
    public bool affectHp, affectMp, affectStr;
    [Header("Weapon/Armer strength")]
    public int weaponStrength;
    public int armerStrength;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use(int charToUseOn)
    {
        CharStat selectedChar = GameManager.instance.playerStats[charToUseOn];

        /*if we are using Item, do this if statement*/
        if (isItem)
        {
            if (affectHp)
            {
                /*if hp is already max hp, dont use it*/
                if (selectedChar.currentHP >= selectedChar.maxHP){

                }
                else {
                    selectedChar.currentHP += amountOfChange;
                    if (selectedChar.currentHP > selectedChar.maxHP)
                    {
                        selectedChar.currentHP = selectedChar.maxHP;
                    }
                }
            }
            else if (affectMp)
            {
                if (selectedChar.currentMP >= selectedChar.maxMP)
                {

                }
                else
                {
                    selectedChar.currentMP += amountOfChange;
                    if (selectedChar.currentMP > selectedChar.maxMP)
                    {
                        selectedChar.currentMP = selectedChar.maxMP;
                    }
                }
            }
            else if (affectStr)
            {
                selectedChar.strength += amountOfChange;

            }
        }

        else if (isWeapon)
        {
            if(selectedChar.equippedWp != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedWp);
            }

            selectedChar.equippedWp = itemName;
            selectedChar.wpPwr = weaponStrength;

        }

        else if (isArmor)
        {
            if (selectedChar.equippedArmr != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedArmr);
            }

            selectedChar.equippedArmr = itemName;
            selectedChar.armrPwr = armerStrength;
        }

        GameManager.instance.removeItem(itemName);
    }
}
