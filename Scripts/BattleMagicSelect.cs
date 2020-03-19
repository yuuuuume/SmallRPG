﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMagicSelect : MonoBehaviour
{
    public string SpellName;
    public int spellCost;
    public Text nameText;
    public Text costText;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Press()
    {
        
        if (BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP >= spellCost)
        {
            BattleManager.instance.magicMenue.SetActive(false);
 
            BattleManager.instance.OpenTargetMenu(SpellName);
            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= spellCost;
        }
        else
        {
            //let player know magic is unavailable
            BattleManager.instance.battleNotice.theText.text = "Magic is not available";
            BattleManager.instance.battleNotice.Activate();
            BattleManager.instance.magicMenue.SetActive(false);
           
        }
        
       
    }
}
