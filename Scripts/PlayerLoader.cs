using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*check if the player exist in the scean. if not spawn him!*/
public class PlayerLoader : MonoBehaviour
{

    
    //we want whole player component. 
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        if(Player.instance == null)
        {
            Instantiate(player);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
