using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*about the small item buttons in button menu.
 @buttonImage item image
 @amountText number of item
 @buttonValue index of item array(index is assign in Gamemenu.showItem())
     */
public class ItemButton : MonoBehaviour
{
    public Image buttonImage;
    public Text amountText;
    public int buttonValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*when we select the small item button, show the description and name of it
     change use or equip based on selected item*/
    public void Press()
    {
        if (GameMenu.instance.theMenu.activeInHierarchy) {
            if (GameManager.instance.itemHeld[buttonValue] != "")
            {
                GameMenu.instance.selectItem(GameManager.instance.getItemDetail(GameManager.instance.itemHeld[buttonValue]));
            }
        }
        if (Shop.instance.shopMenu.activeInHierarchy)
        {
            if (Shop.instance.buyMenu.activeInHierarchy)
            {
                Shop.instance.selectBuyItem(GameManager.instance.getItemDetail(Shop.instance.itemForSale[buttonValue]));
            }
            if (Shop.instance.sellMenu.activeInHierarchy)
            {
                Shop.instance.selectSellItem(GameManager.instance.getItemDetail(GameManager.instance.itemHeld[buttonValue]));
            }
        }

        /*
        //for battle scene item
        if (BattleManager.instance.ItemMenu.activeInHierarchy)
        {
            BattleManager.instance.selectUseItem(GameManager.instance.getItemDetail(GameManager.instance.itemHeld[buttonValue]));
        }
        */

    }
}
