using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public string savedScene;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayBGM(4);
        Player.instance.gameObject.SetActive(false);

        BattleManager.instance.gameObject.SetActive(false);
        
        
       // GameMenu.instance.gameObject.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSavedScene()
    {

        Debug.Log("nn");
        
        Destroy(GameManager.instance.gameObject);
        Destroy(Player.instance.gameObject);
        Destroy(GameMenu.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(BattleManager.instance.gameObject);
         
        SceneManager.LoadScene(savedScene);
       
    }

    public void Exit()
    {
        Application.Quit();
    }
}
