using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*load 
 @player
 @GameManager
 @UIScreen
*/
public class EssentialLoader : MonoBehaviour
{
    public GameObject UIScreen;
    public GameObject player;
    public GameObject gameManager;
    public GameObject audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if(UIFade.instance == null)
        {
            UIFade.instance = Instantiate(UIScreen).GetComponent<UIFade>();
            
        }
        if (Player.instance == null)
        {
            Player clone = Instantiate(player).GetComponent<Player>();
            Player.instance = clone;

        }
        if (GameManager.instance == null)
        {
            GameManager.instance = Instantiate(gameManager).GetComponent<GameManager>();
        }
        if(AudioManager.instance == null)
        {
            AudioManager.instance = Instantiate(audioSource).GetComponent<AudioManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
