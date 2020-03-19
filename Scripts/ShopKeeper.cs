using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//left ctr open the shop menu
//active the shop menu 
public class ShopKeeper : MonoBehaviour
{
    private bool canOpen;
    public string[] itemForSale = new string[40];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canOpen && Input.GetButtonDown("Fire1")&&Player.instance.canMove && !Shop.instance.shopMenu.activeInHierarchy)
        {
            //GameManager.instance.sortItems();
            Shop.instance.itemForSale = itemForSale;

            Shop.instance.OpenShop();
        }
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canOpen = true;
            
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canOpen = false;
        }
    }
}
