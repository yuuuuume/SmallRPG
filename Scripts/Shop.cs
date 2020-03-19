using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    public static Shop instance;
    public GameObject shopMenu;
    public GameObject buyMenu;
    public GameObject sellMenu;

    public Text goldtext;
    

    public string[] itemForSale;
    public ItemButton[] buyItemButtons;
    public ItemButton[] sellItemButtons;
    public Items selectedItem;
    public Text buyItemName, buyItemDescription, buyItemValue;
    public Text sellItemName, sellItemDescription, sellItemValue;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenShop()
    {
        shopMenu.SetActive(true);
        OpenBuyMenu();

        GameManager.instance.shopActive = true;
        goldtext.text = GameManager.instance.currentGold.ToString() + " g";

    }

   

  
    public void CloseShop()
    {
        shopMenu.SetActive(false);
        //player can move (in GameManager.cs)
        GameManager.instance.shopActive = false;
    }

    //same as open item menu in gamemenu.cs
    public void OpenBuyMenu()
    {
        buyItemButtons[0].Press();
        buyMenu.SetActive(true);
        sellMenu.SetActive(false);
      
        for (int i = 0; i < buyItemButtons.Length; i++)
        {
            //get index of each buttons
            buyItemButtons[i].buttonValue = i;


            if (itemForSale[i] != "")
            {

                //guess buttonImage is not a gameobject it self. in this casse, we want assingned gameobject
                buyItemButtons[i].buttonImage.gameObject.SetActive(true);

                buyItemButtons[i].buttonImage.sprite = GameManager.instance.getItemDetail(itemForSale[i]).itemImage;
                //infinity valu we want
                buyItemButtons[i].amountText.text = "";
            }
            else
            {

                buyItemButtons[i].buttonImage.gameObject.SetActive(false);
                buyItemButtons[i].amountText.text = "";
            }

        }
    }

    public void OpenSellMenu()
    {
        sellItemButtons[0].Press();
        buyMenu.SetActive(false);
        sellMenu.SetActive(true);
        GameManager.instance.sortItems();
        ShowSellMenu();
    }
    private void ShowSellMenu()
    {
        for (int i = 0; i < sellItemButtons.Length; i++)
        {
            //get index of each buttons
            sellItemButtons[i].buttonValue = i;

            //Debug.Log(GameManager.instance.itemHeld[i]);
            if (GameManager.instance.itemHeld[i] != "")
            {

                //guess buttonImage is not a gameobject it self. in this casse, we want assingned gameobject
                sellItemButtons[i].buttonImage.gameObject.SetActive(true);

                sellItemButtons[i].buttonImage.sprite = GameManager.instance.getItemDetail(GameManager.instance.itemHeld[i]).itemImage;
                sellItemButtons[i].amountText.text = GameManager.instance.numOfItems[i].ToString(); ;
            }
            else
            {

                sellItemButtons[i].buttonImage.gameObject.SetActive(false);
                sellItemButtons[i].amountText.text = "";
            }

        }
    }

    public void selectBuyItem(Items gettingItem)
    {
        selectedItem = gettingItem;
        buyItemName.text = selectedItem.itemName;
        buyItemDescription.text = selectedItem.description;
        buyItemValue.text = "Value: " + selectedItem.value + "$";

    }

    public void selectSellItem(Items soldItem)
    {
        selectedItem = soldItem;
        sellItemName.text = selectedItem.itemName;
        sellItemDescription.text = selectedItem.description;
        sellItemValue.text = "Value: " + Mathf.FloorToInt(selectedItem.value*.5f).ToString() + "$";
    }

    public void buyItem()
    {
        if (selectedItem != null)
        {
            if (GameManager.instance.currentGold >= selectedItem.value)
            {
                GameManager.instance.currentGold -= selectedItem.value;
                GameManager.instance.AddItem(selectedItem.itemName);
            }
            goldtext.text = GameManager.instance.currentGold.ToString() + "$";
        }
    }

    public void sellItem()
    {
        if(selectedItem != null)
        {
            GameManager.instance.currentGold += Mathf.FloorToInt(selectedItem.value * .5f);
            GameManager.instance.removeItem(selectedItem.itemName);
        }

        goldtext.text = GameManager.instance.currentGold.ToString() + "$";
        ShowSellMenu();
    }
}
